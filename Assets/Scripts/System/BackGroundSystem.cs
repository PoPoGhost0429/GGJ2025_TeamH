using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSystem : MonoBehaviour
{
    private static BackGroundSystem instance;
    public static BackGroundSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BackGroundSystem>();
                if (instance == null)
                {
                    GameObject go = new GameObject("BackGroundSystem");
                    instance = go.AddComponent<BackGroundSystem>();
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
