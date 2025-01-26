using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    private static PlayerSystem instance;

    private List<PlayerBase> playerList = new List<PlayerBase>();

    [SerializeField] private GameObject playerUnitPrefab;
    private ObjectPoolSystem playerUnitPool;
    [SerializeField] private PlayerData playerData;

    [SerializeField] private List<int> debugPlayerUnit = new List<int>();

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

    public void InitPlayerSystem(){
        GameObject poolGO = new GameObject("PlayerUnitPool");
        playerUnitPool = poolGO.AddComponent<ObjectPoolSystem>();
        playerUnitPool.CreatePool(playerUnitPrefab, 100);

        InvokeRepeating("ChangeUnitRadius", 0, 1);
    }

    private void Update(){

        // 更新所有玩家的移動
        foreach(var player in playerList) {
            player.RotateUnits();
        }

        for(int i = 0; i < debugPlayerUnit.Count; i++){
            debugPlayerUnit[i] = playerList[i].GetUnitAmount();
        }
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
            PlayerBase player = new PlayerBase(i,playerData.playerInitData, playerData.animatorControllers[i], new Vector3(0,0,0));
            playerList.Add(player);
            debugPlayerUnit.Add(5);
        }
    }

    public void SubMaxHeight(float amount){
        foreach(var player in playerList){
            player.SubMaxHeight(amount);
        }
    }

    public GameObject GenerateUnit(Vector3 position){
        return playerUnitPool.GetFromPool(playerUnitPrefab.name, position, Quaternion.identity);
    }

    public void ReturnUnit(GameObject unit){
        playerUnitPool.ReturnToPool(unit);
    }

    private void ChangeUnitRadius(){
        foreach(var player in playerList){
            player.ExpandRadius(Random.Range(-0.5f, 0.5f));
        }
    }
}
