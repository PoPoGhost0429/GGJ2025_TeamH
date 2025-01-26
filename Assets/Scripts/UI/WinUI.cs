using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private Text _WinText;

    [SerializeField] private Button _PlayagainBtn;
    [SerializeField] private Button _ExitBtn;

    private void Awake()
    {
        _PlayagainBtn.onClick.AddListener(() => SceneManager.LoadScene(EScene.SelectScene.ToString()));
        _ExitBtn.onClick.AddListener(() => SceneManager.LoadScene(EScene.MainMenuScene.ToString()));
    }

    public void ShowWinText(int winIndex)
    {
        _WinText.text = $"{winIndex}P WIN!!!";
    }
}
