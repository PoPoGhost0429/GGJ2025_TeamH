using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SelectUI : MonoBehaviour
    {
        [SerializeField] private Button _StartButton;
        [SerializeField] private Button _ExitButton;
        
        [SerializeField] private BacteriumContent _BacteriumContentPrefab;
        [SerializeField] private Sprite[] _BacteriumIcons;
        
        private readonly int m_Amount = 4;
        
        private void Awake()
        {
            // TODO 生成等同接入設備數量的玩家
            for (int i = 0; i < m_Amount; i++)
            {
                BacteriumContent obj = Instantiate(_BacteriumContentPrefab, transform);
                obj.Index = i + 1;
                obj.Sprite = _BacteriumIcons[i];
            }
        }

        private void Start()
        {
            _StartButton.onClick.AddListener(() =>
            {
                Debug.Log("Start");
                SceneManager.LoadScene(EScene.InputTest.ToString());
            });
            
            _ExitButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(EScene.MainMenuScene.ToString());
            });
        }
    }
}
