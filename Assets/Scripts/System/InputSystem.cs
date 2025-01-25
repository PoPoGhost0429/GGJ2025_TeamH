using UnityEngine;
using UnityEngine.InputSystem;

public struct InputData
{
    public Vector2 moveDirection;
}

public enum EInputSource
{
    KeyBoardLeft = 0,
    KKeyboardRight = 1,
    Controller1 = 2,
    Controller2 = 3,
}

public class InputSystem : MonoBehaviour
{
    private static InputSystem instance;
    public static InputSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputSystem>();
                if (instance == null)
                {
                    GameObject go = new GameObject("InputSystem");
                    instance = go.AddComponent<InputSystem>();
                }
            }
            return instance;
        }
    }
    public TestPlayerController[] PlayerControllers { get; private set; } = new TestPlayerController[4];
    
    [SerializeField] private GameObject _PauseMenu;
    [SerializeField] private GameObject _PlayerPrefab;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // AddKeyBoardPlayer(2);
        // AddGamePadPlayer();
        PlayerControllers = FindObjectsOfType<TestPlayerController>();
    }

    /// <summary>
    /// 生成鍵盤控制玩家
    /// </summary>
    /// <param name="amount">生成數量</param>
    public void AddKeyBoardPlayer(int amount)
    {
        PlayerInput.Instantiate(_PlayerPrefab, controlScheme: "KeyboardLeft", pairWithDevice: Keyboard.current);
        
        if (amount == 2)
        {
            PlayerInput.Instantiate(_PlayerPrefab, controlScheme: "KeyboardRight", pairWithDevice: Keyboard.current);
        }
    }
    
    /// <summary>
    /// 生成Controller控制玩家
    /// </summary>
    public void AddGamePadPlayer()
    {
        int gamepadCount = Gamepad.all.Count;
        switch (gamepadCount)
        {
            case >= 2:
                JoinPlayer(EInputSource.Controller1, _PlayerPrefab);
                JoinPlayer(EInputSource.Controller2, _PlayerPrefab);
                break;
            case 1:
                JoinPlayer(EInputSource.Controller1, _PlayerPrefab);
                break;
        }
    }

    public void OnPlayerPause()
    {
        _PauseMenu.SetActive(true);
        // switch action map 
        foreach (TestPlayerController input in PlayerControllers)
        {
            input.SetActionMap(EInputType.UI);
        }
    }

    public void OnPlayerResume()
    {
        _PauseMenu.SetActive(false);
        // switch action map
        foreach (TestPlayerController input in PlayerControllers)
        {
            input.SetActionMap(EInputType.GamePlay);
        }
    }
    
    private void JoinPlayer(EInputSource source, GameObject playerPrefab)
    {
        switch (source)
        {
            case EInputSource.KeyBoardLeft:
                PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "KeyboardLeft", pairWithDevice: Keyboard.current);
                break;
            case EInputSource.KKeyboardRight:
                PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "KeyboardRight", pairWithDevice: Keyboard.current);
                break;
            case EInputSource.Controller1:
                PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Controller1", pairWithDevice: Gamepad.all[0]);
                break;
            case EInputSource.Controller2:
                PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Controller2", pairWithDevice: Gamepad.all[1]);
                break;
        }
    }
    
    public void OnPlayerPause()
    {
        _PauseMenu.SetActive(true);
        SwitchInputMap(EInputType.UI);
    }

    public void OnPlayerResume()
    {
        _PauseMenu.SetActive(false);
        SwitchInputMap(EInputType.GamePlay);
    }

    public void SwitchInputMap(EInputType inputType)
    {
        foreach (TestPlayerController input in m_PlayerInputs)
        {
            input.SetActionMap(inputType);
        }
    }
    
// #if UNITY_EDITOR
//     private void OnEnable()
//     {
//         m_PlayerInputManager.onPlayerJoined += OnPlayerJoined;
//     }
//
//     private void OnDisable()
//     {
//         m_PlayerInputManager.onPlayerJoined -= OnPlayerJoined;
//     }
//
//     private void OnPlayerJoined(PlayerInput obj)
//     {
//         Debug.Log($"Index: {obj.playerIndex}");
//     }
// #endif
}
