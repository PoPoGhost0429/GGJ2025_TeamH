using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlController : BubbleBase
{
    // Start is called before the first frame update
    protected override void Start(){
        gameObject.tag = "Pearl";
        transform.localScale = new Vector3(1.0f, 1.0f, 0);
        //transform.position = new Vector3(UnityEngine.Random.Range(-7.0f, 8.0f), 5f, 0);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        gameObject.GetComponent<Rigidbody2D>().mass = 1.2f;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1.0f);
    }

    // Update is called once per frame
    protected override void Update(){
        
    }
}
