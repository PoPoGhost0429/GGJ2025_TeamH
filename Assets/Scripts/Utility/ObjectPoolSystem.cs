using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolSystem : MonoBehaviour
{
    // 使用字典來存儲不同預製體的對象池
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    // 創建對象池
    public void CreatePool(GameObject prefab, int initialSize)
    {
        string prefabName = prefab.name;
        
        if (!poolDictionary.ContainsKey(prefabName))
        {
            poolDictionary.Add(prefabName, new Queue<GameObject>());
            prefabDictionary.Add(prefabName, prefab);

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = CreateNewObject(prefab);
                ReturnToPool(obj);
            }
        }
    }

    // 從對象池獲取對象
    public GameObject GetFromPool(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogWarning($"對象池中不存在預製體 {prefabName}");
            return null;
        }

        GameObject obj;
        if (poolDictionary[prefabName].Count == 0)
        {
            obj = CreateNewObject(prefabDictionary[prefabName]);
        }
        else
        {
            obj = poolDictionary[prefabName].Dequeue();
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    // 將對象返回對象池
    public void ReturnToPool(GameObject obj)
    {
        string prefabName = obj.name.Replace("(Clone)", "").Trim();
        
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogWarning($"嘗試返回未知預製體到對象池: {prefabName}");
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(transform);
        poolDictionary[prefabName].Enqueue(obj);
    }

    // 創建新對象的輔助方法
    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.name = prefab.name;
        return obj;
    }
} 