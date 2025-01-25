using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSystem : MonoBehaviour
{
    private static BubbleSystem instance;
    public static BubbleSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BubbleSystem>();
                if (instance == null)
                {
                    GameObject go = new GameObject("BubbleSystem");
                    instance = go.AddComponent<BubbleSystem>();
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
