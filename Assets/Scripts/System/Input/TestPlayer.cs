using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPlayer : MonoBehaviour, IMovable
{
    public int Index => _Index;
    
    [SerializeField] private int _Index;
    [SerializeField] private float _Speed = 50f;

    private Vector2 m_MoveDirection = Vector2.zero;
    
    private void Update()
    {
        transform.Translate(m_MoveDirection * (_Speed * Time.deltaTime));
    }

    public void SetMoveInput(Vector2 direction)
    {
        m_MoveDirection = direction;
    }
}
