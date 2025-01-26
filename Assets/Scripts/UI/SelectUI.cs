using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using V.Tool.JuicyFeeling;

namespace UI
{
    public class SelectUI : MonoBehaviour
    {
        [SerializeField] private Button _StartButton;
        [SerializeField] private Button _ExitButton;
        
        [SerializeField] private BacteriumContent _BacteriumContentPrefab;
        [SerializeField] private Sprite[] _BacteriumIcons;
        
        private void OnEnable()
        {
            InputSystem.Instance.OnPlayerConnectedEvent += OnPlayerConnected;
        }

        private void OnDisable()
        {
            if (InputSystem.Instance != null)
            {
                InputSystem.Instance.OnPlayerConnectedEvent -= OnPlayerConnected;
            }
        }

        private void Start()
        {
            _StartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(EScene.MainGame.ToString());
            });
            
            _ExitButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(EScene.MainMenuScene.ToString());
            });
        }
        
        private void OnPlayerConnected(int count)
        {
            // TODO 生成等同接入設備數量的玩家
            for (int i = 0; i < count; i++)
            {
                BacteriumContent obj = Instantiate(_BacteriumContentPrefab, transform);
                obj.Index = i + 1;
                obj.Sprite = _BacteriumIcons[i];

                InputSystem.Instance.PlayerControllers[i].OnInteractEvent +=
                    () => obj.GetComponentInChildren<SquashAndStretch>().PlaySquashAndStretch();
            }
        }

    }
}
