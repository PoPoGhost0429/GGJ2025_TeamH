using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    public event Action<int> OnPlayerConnectedEvent;
    
    public List<TestPlayerController> PlayerControllers { get; private set; } = new List<TestPlayerController>();
    
    [SerializeField] private GameObject _PauseMenu;
    [SerializeField] private GameObject _InputPrefab;

    private List<PlayerInput> m_PlayerInputs = new List<PlayerInput>();

#region Life Cycle
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
#endregion

    private void OnSceneChange(Scene arg0, Scene arg1)
    {
        if (arg1.buildIndex == (int)EScene.SelectScene)
        {
            foreach (TestPlayerController variable in PlayerControllers)
            {
                if(variable == null) continue;
                DestroyImmediate(variable.gameObject);
            }
            
            m_PlayerInputs.Clear();
            m_PlayerInputs.Capacity = 0;
            PlayerControllers.Clear();
            PlayerControllers.Capacity = 0;
            
            AddKeyBoardPlayer(2);
            AddGamePadPlayer();
            
            PlayerControllers = FindObjectsOfType<TestPlayerController>().ToList();
            OnPlayerConnectedEvent?.Invoke(PlayerControllers.Count);
        }
    }

    /// <summary>
    /// 生成鍵盤控制玩家
    /// </summary>
    /// <param name="amount">生成數量</param>
    private void AddKeyBoardPlayer(int amount)
    {
        m_PlayerInputs.Add(JoinPlayer(EInputSource.KKeyboardRight, _InputPrefab));
        m_PlayerInputs.Add(JoinPlayer(EInputSource.KeyBoardLeft, _InputPrefab));
    }
    
    /// <summary>
    /// 生成Controller控制玩家
    /// </summary>
    private void AddGamePadPlayer()
    {
        int gamepadCount = Gamepad.all.Count;
        switch (gamepadCount)
        {
            case >= 2:
                m_PlayerInputs.Add(JoinPlayer(EInputSource.Controller1, _InputPrefab));
                m_PlayerInputs.Add(JoinPlayer(EInputSource.Controller2, _InputPrefab));
                break;
            case 1:
                m_PlayerInputs.Add(JoinPlayer(EInputSource.Controller1, _InputPrefab));
                break;
        }
    }
    
    private PlayerInput JoinPlayer(EInputSource source, GameObject playerPrefab)
    {
        switch (source)
        {
            case EInputSource.KeyBoardLeft:
                return PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "KeyboardLeft", pairWithDevice: Keyboard.current);
            
            case EInputSource.KKeyboardRight:
                return PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "KeyboardRight", pairWithDevice: Keyboard.current);
            
            case EInputSource.Controller1:
                return PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Controller1", pairWithDevice: Gamepad.all[0]);
            
            case EInputSource.Controller2:
                return PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Controller2", pairWithDevice: Gamepad.all[1]);
        }

        return null;
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
        foreach (TestPlayerController input in PlayerControllers)
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
