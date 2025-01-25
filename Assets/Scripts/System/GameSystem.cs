using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    private static GameSystem _instance;

    public static GameSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameSystem>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameSystem");
                    _instance = obj.AddComponent<GameSystem>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    // 初始化遊戲
    public void InitGame(){

    }
}
