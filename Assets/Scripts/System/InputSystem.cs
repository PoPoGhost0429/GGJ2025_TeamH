using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputData
{
    public Vector2 moveDirection;
}

public class InputSystem : MonoBehaviour
{
    private static InputSystem instance;
    public static InputSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputSystem>();
                if (instance == null)
                {
                    GameObject go = new GameObject("InputSystem");
                    instance = go.AddComponent<InputSystem>();
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
