using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public float followPlayerSpeed = 1f, getGas = 0;
    // Update is called once per frame
    void Update()
    {
        
    }

    public float getGasValue(){
        float Value = getGas;
        getGas = 0;
        return Value;
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            Transform parent = transform.parent;
            parent.GetComponent<Rigidbody2D>().AddForce((other.gameObject.transform.position - parent.transform.position) * followPlayerSpeed);
            parent.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            getGas += parent.GetComponent<bubbleController>().absorption();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            Debug.Log("Leave");
            Transform parent = transform.parent;
            parent.GetComponent<Rigidbody2D>().gravityScale = -0.3f;
        }
    }
}
