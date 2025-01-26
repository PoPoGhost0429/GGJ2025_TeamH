using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EInputType
{
    GamePlay,
    UI,
}

[RequireComponent(typeof(PlayerInput))]
public class TestPlayerController : MonoBehaviour
{
    public event Action<InputData> OnInputEvent;
    public event Action OnInteractEvent;
    
    private PlayerInput m_InputActions;
    
    private void Awake()
    {
        m_InputActions = GetComponent<PlayerInput>();
        
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        m_InputActions.actions["Move"].performed += OnMove;
        m_InputActions.actions["Move"].canceled += OnMove;
        m_InputActions.actions["Interact"].performed += OnPerform;

        m_InputActions.actions["Pause"].performed += OnPause;
        
        m_InputActions.actions["Resume"].performed += OnResume;
    }

    private void OnDisable()
    {
        m_InputActions.actions["Move"].performed -= OnMove;
        m_InputActions.actions["Move"].canceled -= OnMove;
        m_InputActions.actions["Interact"].performed -= OnPerform;
        
        m_InputActions.actions["Pause"].performed -= OnPause;
        
        m_InputActions.actions["Resume"].performed -= OnResume;
    }
    
    private void Start()
    {
        SetActionMap(EInputType.GamePlay);
    }

    public void SetActionMap(EInputType inputType)
    {
        m_InputActions.SwitchCurrentActionMap(inputType.ToString());
    }
    
    private void OnMove(InputAction.CallbackContext context)
    {
        InputData inputData = new InputData()
        {
            moveDirection = context.ReadValue<Vector2>()
        };
        
        OnInputEvent?.Invoke(inputData);
    }
    
    private void OnPerform(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            OnInteractEvent?.Invoke();
        }
    }
    
    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputSystem.Instance.OnPlayerPause();
        }
    }
    
    private void OnResume(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputSystem.Instance.OnPlayerResume();
        }
    }

    public void UnRegisterInteractEvent()
    {
        OnInteractEvent = null;
    }
}
