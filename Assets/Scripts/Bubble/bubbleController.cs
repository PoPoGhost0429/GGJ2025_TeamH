using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class bubbleController : BubbleBase{
    // Update is called once per frame
    public float followPlayerSpeed = 1f, absorbSpeed = 0.3f, absorbRatio = 0.8f, absorbMaxDis = 3f, gas = 0;
    public bool canAsborb = false;
    Vector3 test = new Vector3(0, 5, 0);

    protected override void Update(){
        base.Update();
        gas = transform.localScale.x;

    }

    public void follow(Vector3 playPos){
        GetComponent<Rigidbody2D>().AddForce((playPos - transform.position) * followPlayerSpeed);
    }

    public void absorption(){
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        transform.localScale -= new Vector3(absorbSpeed*absorbRatio, absorbSpeed*absorbRatio, 0);
        if(transform.localScale.x < 0.01f){
            gas = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player" && gameObject.tag != "Pearl"){
            GetComponent<Rigidbody2D>().AddForce((other.transform.position - transform.position) * followPlayerSpeed);
            canAsborb = true;
        }
        if(other.gameObject.tag == "BubbleType1" || other.gameObject.tag == "BubbleType2"){
            Collider2D bubble = other.gameObject.GetComponent<CircleCollider2D>();
            Collider2D pearl = gameObject.GetComponent<CircleCollider2D>();
            Physics2D.IgnoreCollision(bubble, pearl);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Player" && gameObject.tag != "Pearl"){
            canAsborb = false;
        }
    }
}
