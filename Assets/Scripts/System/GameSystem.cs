using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    public enum GameState
    {
        Start,
        Gaming,
        End
    }

    private static GameSystem _instance;

    public static GameSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameSystem>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameSystem");
                    _instance = obj.AddComponent<GameSystem>();
                }
            }
            return _instance;
        }
    }

    public float gameTime = 120;
    public float bubbleMaxHeight = 11.5f;
    public GameObject spawnBubbleBasePrefab;
    public GameObject spawnPearlBasePrefab;
    public Text readyText;
    public Text gameTimeText;
    public WinUI winUI;
    public FireAnimController fireAnimController;
    public StrawEvent strawEvent;
    private SpawnBubble spawnBubble;
    private SpawnBubble spawnPearl;
    private GameState gameState = default;
    private int playerCount;
    private float readyTime = 5;
    private List<int> strawEventTime = new List<int>() { 90, 60, 30 };
    private int fireMidTime = 80;
    private int fireBigTime = 40;
    

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        // DontDestroyOnLoad(gameObject);
        spawnBubble = spawnBubbleBasePrefab.GetComponent<SpawnBubble>();
        spawnPearl = spawnPearlBasePrefab.GetComponent<SpawnBubble>();
    }

    private void Start()
    {
        gameTimeText.text = $"{(int)gameTime / 60}:{(int)gameTime % 60:00}";
        // InitGame();
        StartCoroutine(ReadyCoroutine(readyTime));
    }  

    void Update()
    {
        if (gameState == GameState.End)
            return;

        CheckGameEnd();
    }

    public void InitGame()
    {
        
        playerCount = InputSystem.Instance.PlayerControllers.Count;
        PlayerSystem.Instance.InitPlayerSystem();
        PlayerSystem.Instance.GeneratePlayer(playerCount);

        gameState = GameState.Gaming;
        StartCoroutine(GameTimeCoroutine());

        foreach (var setting in spawnBubble.spawnBubbleSetting)
            StartCoroutine(SpawnBubbles(setting));
        foreach (var setting in spawnPearl.spawnBubbleSetting)
            StartCoroutine(SpawnPearl(setting));
    }

    private void CheckGameEnd()
    {
        if (gameTime <= 0 || CheckOnlyOnePlayer())
        {             
            Time.timeScale = 0;
            gameState = GameState.End;
            string winner = GetWinner();
            // Show winner UI
            winUI.ShowWinText(winner);
            winUI.gameObject.SetActive(true);
            Debug.Log("Game End, Winner: " + winner);
            return ;
        }        
    }

    private bool CheckOnlyOnePlayer()
    {
        int alivePlayerCount = 0;
        for (int i = 0; i < playerCount; i++)
        {
            if (PlayerSystem.Instance.GetPlayer(i).CheckIsAlive())
            {
                alivePlayerCount++;
            }
        }
        return alivePlayerCount == 1;
    }

    private string GetWinner()
    {
        int maxUnitCount = -1;
        List<int> winnerIndices = new List<int>();

        for (int i = 0; i < playerCount; i++)
        {
            if (PlayerSystem.Instance.GetPlayer(i).CheckIsAlive())
            {
                int unitCount = PlayerSystem.Instance.GetPlayer(i).GetUnitAmount();
                if (unitCount > maxUnitCount)
                {
                    maxUnitCount = unitCount;
                    winnerIndices.Clear();
                    winnerIndices.Add(i);
                }
                else if (unitCount == maxUnitCount)
                {
                    winnerIndices.Add(i);
                }
            }
        }

        if (winnerIndices.Count == playerCount)
        {
            return "All players Win!";
        }
        else
        {
            return string.Join(" ", winnerIndices.Select(index => (index + 1) + "P")) + " Win!";
        }
    }

    private IEnumerator ReadyCoroutine(float readyTime)
    {
        while (readyTime > 0)
        {
            readyTime -= Time.deltaTime;
            readyText.text = ((int)readyTime).ToString();
            yield return null;
        }
        readyText.text = "Start!!!";
        yield return new WaitForSeconds(1);
        InitGame();
        readyText.gameObject.SetActive(false);
    }

    private IEnumerator GameTimeCoroutine()
    {
        while (gameTime > 0)
        {
            // Straw Event
            if (strawEventTime.Contains((int)gameTime))
            {
                strawEvent.StrawEventStart(2);
            }

            // Fire Event
            if ((int)gameTime == fireMidTime)
            {
                fireAnimController.PlayerMidFire();
            }
            if ((int)gameTime == fireBigTime)
            {
                fireAnimController.PlayerHighFire();
            }

            gameTime -= Time.deltaTime;
            gameTimeText.text = $"{(int)gameTime / 60}:{(int)gameTime % 60:00}";
            Debug.Log("Game Time: " + (int)gameTime);
            yield return null;
        }
        readyText.gameObject.SetActive(true);
        readyText.text = "Time's Up!";
    }

    private IEnumerator SpawnBubbles(SpawnBubbleSettingSO setting)
    {
        while (gameState == GameState.Gaming)
        {
            yield return new WaitForSeconds(setting.spawnDuration);
            Vector2 position = spawnBubble.CalculateSpawnPosition(setting.index);
            // GameObject circle = Instantiate(bubblePrefab, position, Quaternion.identity);
            // circle.transform.localScale = new Vector3(setting.bubbleSize, setting.bubbleSize, 1);
            BubbleSystem.Instance.generateBubble("Normal", position.x, position.y, setting.bubbleSize, bubbleMaxHeight);
        }
    }

    private IEnumerator SpawnPearl(SpawnBubbleSettingSO setting)
    {
        while (gameState == GameState.Gaming)
        {
            yield return new WaitForSeconds(setting.spawnDuration);
            Vector2 position = spawnPearl.CalculateSpawnPosition(setting.index);
            // GameObject circle = Instantiate(bubblePrefab, position, Quaternion.identity);
            // circle.transform.localScale = new Vector3(setting.bubbleSize, setting.bubbleSize, 1);
            BubbleSystem.Instance.generatePearl(position.x, position.y, setting.bubbleSize);
        }
    }
}
