using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public float gameTime = 300;
    public GameObject spawnBubbleBasePrefab;
    public GameObject spawnPearlBasePrefab;
    public GameObject bubblePrefab;
    private SpawnBubble spawnBubble;
    private SpawnBubble spawnPearl;
    private GameState gameState = default;
    private List<bool> playersAlive;
    private int playerCount;
    private float bubbleMaxHeight = 20;
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
        InitGame();
    }  

    void Update()
    {
        if (gameState == GameState.End)
            return;

        CheckGameEnd();
        Debug.Log("Game Time: " + (int)gameTime);
    }

    public void InitGame()
    {
        // Get player count input from UI
        playerCount = 2;
        playersAlive = new List<bool>(playerCount);
        for (int i = 0; i < playerCount; i++)
            playersAlive.Add(true);
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
        int alivePlayerCount = playersAlive.Count(b => b);

        if (gameTime <= 0 || alivePlayerCount == 1)
        {
            Time.timeScale = 0;
            gameState = GameState.End;
            string winner = GetWinner();
            // Show winner UI
            Debug.Log("Game End, Winner: " + winner);
            return ;
        }        
    }

    private string GetWinner()
    {
        int winnerIndex = 0;
        int maxScore = 0;
        for (int i = 0; i < playersAlive.Count; i++)
        {
            // if (playersAlive[i] && PlayerSystem.Instance.GetPlayer(i).score > maxScore)
            // {
            //     maxScore = PlayerSystem.Instance.GetPlayer(i).score;
            //     winnerIndex = i;
            // }
        }
        return (winnerIndex + 1) + "P";
    }

    private IEnumerator GameTimeCoroutine()
    {
        while (gameTime > 0)
        {
            gameTime -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SpawnBubbles(SpawnBubbleSettingSO setting)
    {
        while (gameState == GameState.Gaming)
        {
            yield return new WaitForSeconds(setting.spawnDuration);
            Vector2 position = spawnBubble.CalculateSpawnPosition(setting.index);
            // GameObject circle = Instantiate(bubblePrefab, position, Quaternion.identity);
            // circle.transform.localScale = new Vector3(setting.bubbleSize, setting.bubbleSize, 1);
            BubbleSystem.Instance.generateBubble(position.x, position.y, setting.bubbleSize, bubbleMaxHeight);
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
