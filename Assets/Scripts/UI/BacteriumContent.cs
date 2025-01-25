using System;
using UnityEngine;
using UnityEngine.UI;

public class BacteriumContent : MonoBehaviour
{
    public int Index {get; set;}
    public Sprite Sprite
    {
        set => _Icon.sprite = value;
    }
    
    [SerializeField] private Text _IndexText;
    [SerializeField] private Image _Icon;

    private void Start()
    {
        _IndexText.text = $"{Index}P";
    }
}
