using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class bubbleController : BubbleBase{
    // Update is called once per frame
    public float absorbSpeed = 0.3f, absorbRatio = 0.8f, maxGas=100f, Nowgas = 0;
    public bool canAsborb = false;
    protected override void Start(){
        //transform.position = new Vector3(UnityEngine.Random.Range(-7.0f, 8.0f), -3.7f, 0);
        Transform child = transform.GetChild(0);
        child.gameObject.tag = "BubbleType1";
        gameObject.tag = "BubbleType2";
    }
    protected override void Update(){
        if (transform.localScale.x < maxScale && !isMax){
            transform.localScale += new Vector3(scaleInit*ratio, scaleInit*ratio, 0);
            Nowgas += maxGas*0.01f;
        }
        else{
            isMax = true;
        }

        if (transform.position.y > maxHeight){
            Destroy(gameObject);
        }


    }

    public float absorption(){
        float resGas = maxGas*0.01f;
        Debug.Log("Enter");
        Nowgas -= resGas;
        transform.localScale -= new Vector3(absorbSpeed*absorbRatio, absorbSpeed*absorbRatio, 0);
        if(Nowgas < 0f){
            gameObject.SetActive(false);
        }
        return resGas;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            Debug.Log("Collision");
            Transform child = transform.GetChild(0);
            child.GetComponent<BubbleTrigger>().getGas += Nowgas;
            Nowgas = 0;
            gameObject.SetActive(false);
        }
    }
}
