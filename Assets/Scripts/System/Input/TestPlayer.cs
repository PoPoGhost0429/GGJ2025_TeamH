using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private int _Index;
    
    private InputData m_InputData;

    private void OnEnable()
    {
        if (_Index < InputSystem.Instance.PlayerControllers.Count)
        {
            InputSystem.Instance.PlayerControllers[_Index].OnInputEvent += OnMove;
        }
    }
    
    private void OnDisable()
    {
        if (InputSystem.Instance != null && _Index < InputSystem.Instance.PlayerControllers.Count &&
            InputSystem.Instance.PlayerControllers[_Index] != null)
        {
            InputSystem.Instance.PlayerControllers[_Index].OnInputEvent -= OnMove;
        }
    }

    private void Update()
    {
        transform.Translate(m_InputData.moveDirection * (10 * Time.deltaTime));
    }

    private void OnMove(InputData obj)
    {
        m_InputData = obj;
    }
}
