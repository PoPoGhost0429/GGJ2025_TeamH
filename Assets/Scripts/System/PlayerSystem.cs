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

    private void Start(){
        
        GameObject poolGO = new GameObject("PlayerUnitPool");
        playerUnitPool = poolGO.AddComponent<ObjectPoolSystem>();
        playerUnitPool.CreatePool(playerUnitPrefab, 40);

        InvokeRepeating("ChangeUnitRadius", 0, 1);
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            GeneratePlayer(1);
        }

        if(Input.GetKeyDown(KeyCode.R)){
            foreach(var player in playerList){
                player.GenerateUnit();
            }
        }

        if(Input.GetKeyDown(KeyCode.K)){
            foreach(var player in playerList){
                player.Dispersion();
            }
        }

        if(Input.GetKeyDown(KeyCode.L)){
            foreach(var player in playerList){
                player.Polymerization();
            }
        }

        // 更新所有玩家的移動
         // 獲取輸入軸的值
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // 創建移動方向向量
        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        
        // 創建輸入數據
        InputData inputData = new InputData{
            moveDirection = moveDirection
        };

        // 更新所有玩家的移動
        foreach(var player in playerList) {
            player.RotateUnits();
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
            PlayerBase player = new PlayerBase(i,playerData.playerInitData);
            playerList.Add(player);
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
