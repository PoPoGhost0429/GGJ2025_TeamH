using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _NewGameButton;
        [SerializeField] private Button _ExitGameButton;

        private void Awake()
        {
            _NewGameButton.onClick.AddListener(() =>
            {
                // InputSystem.Instance.SwitchInputMap(EInputType.GamePlay);
                SceneManager.LoadScene(EScene.SelectScene.ToString());
            });
        
            _ExitGameButton.onClick.AddListener(Application.Quit);
        }

        private void Start()
        {
            // InputSystem.Instance.SwitchInputMap(EInputType.UI);
        }
    }
}
