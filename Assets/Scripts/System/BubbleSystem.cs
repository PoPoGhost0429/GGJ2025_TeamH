using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSystem : MonoBehaviour
{
    public enum BubbleType
    {
        Bubble,
        Pearl
    }
    private static BubbleSystem instance;
    public GameObject bubblePrefab;
    public GameObject pearlPrefab;
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

    public void generateBubble(float x, float y, float scale, float maxHeight){
        GameObject bubble = Instantiate(bubblePrefab);
        bubble.transform.position = new Vector3(x, y, 0);
        bubble.transform.localScale = new Vector3(scale, scale, 0);
        bubble.transform.GetChild(0).gameObject.GetComponent<bubbleController>().maxHeight = maxHeight;
        /*
        var:
        absorbSpeed
        absorbRatio

        func:
        absorption()
        float bubble.getChild(0).getChild(0).getGasValue()
        */
        
    }
    public void generatePearl(float x, float y, float scale){
        GameObject pearl = Instantiate(pearlPrefab);
        pearl.transform.position = new Vector3(x, y, 0);
        pearl.transform.localScale = new Vector3(scale, scale, 0);
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
        //generateBubble(UnityEngine.Random.Range(-7.0f, 8.0f), -3.7f, 1.0f, 50f);
        //generatePearl(UnityEngine.Random.Range(-7.0f, 8.0f), 5f, 0);
    }

}
