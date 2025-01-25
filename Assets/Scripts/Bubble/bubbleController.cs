using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class bubbleController : BubbleBase{
    // Update is called once per frame
    [SerializeField] float followPlayerSpeed = 1f, absorbSpeed = 0.3f, absorbRatio = 0.8f;

    protected override void Update(){
        base.Update();
    }

    void absorption(Vector3 playPos){
        GetComponent<Rigidbody2D>().AddForce((playPos - transform.position) * followPlayerSpeed);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        transform.localScale -= new Vector3(absorbSpeed*absorbRatio, absorbSpeed*absorbRatio, 0);
        if(transform.localScale.x < 0.1f){
            Destroy(gameObject);
        }
    }
}
