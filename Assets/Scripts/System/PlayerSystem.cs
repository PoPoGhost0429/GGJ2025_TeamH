using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    private static PlayerSystem instance;

    private List<PlayerBase> playerList = new List<PlayerBase>();
    public static PlayerSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerSystem>();
                if (instance == null)
                {
                    GameObject go = new GameObject("PlayerSystem");
                    instance = go.AddComponent<PlayerSystem>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    // 取得指定編號玩家
    public PlayerBase GetPlayer(int playerIndex)
    {
        return playerList[playerIndex];
    }

    // 生成指定數量的玩家
    public void GeneratePlayer(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            PlayerBase player = new PlayerBase(new PlayerInitData());
            playerList.Add(player);
        }
    }
}
