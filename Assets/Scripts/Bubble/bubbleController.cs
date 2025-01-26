using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class bubbleController : BubbleBase{
    // Update is called once per frame
    public float absorbSpeed = 0.3f, absorbRatio = 0.8f, maxGas=100f, Nowgas = 0;
    private string bubbleType = "Normal";
    public bool canAsborb = false;
    [SerializeField] private List<GameObject> bubbleTypeList = new List<GameObject>();
    protected override void Start(){
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
        if(bubbleType == "Normal"){
            gameObject.GetComponent<SpriteRenderer>().sprite = bubbleTypeList[0].GetComponent<SpriteRenderer>().sprite;
        }
        else if(bubbleType == "Rainbow"){
            gameObject.GetComponent<SpriteRenderer>().sprite = bubbleTypeList[1].GetComponent<SpriteRenderer>().sprite;
        }

    }

    public void setBubbleType(string type){
        if(type == "Normal" || type == "Rainbow"){
            bubbleType = type;
        }
        else{
            Debug.Log("Invalid Bubble Type");
        }  
    }

    public string getBubbleType(string type){
        return bubbleType;
    }

    public float absorption(){
        float resGas = maxGas*0.01f;
        Nowgas -= resGas;
        transform.localScale -= new Vector3(absorbSpeed*absorbRatio, absorbSpeed*absorbRatio, 0);
        if(Nowgas < 0f){
            gameObject.SetActive(false);
        }
        return resGas;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            Transform child = transform.GetChild(0);
            child.GetComponent<BubbleTrigger>().getGas += Nowgas;
            Nowgas = 0;
            gameObject.SetActive(false);
        }
    }
}
