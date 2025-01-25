using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleBase : MonoBehaviour{
    // Start is called before the first frame update
    public float ratio = 0.2f, scaleInit = 0.01f, maxHeight = 50.0f, maxScale = 1.0f;
    [SerializeField] bool isMax = false;
    void Start(){
        transform.position = new Vector3(UnityEngine.Random.Range(-7.0f, 8.0f), -3.7f, 0);
        System.Random random = new System.Random();
        int randomInt = random.Next(0, 100);
        if (randomInt < 40){
            gameObject.tag = "Pearl";
            transform.localScale = new Vector3(1.0f, 1.0f, 0);
            transform.position = new Vector3(UnityEngine.Random.Range(-7.0f, 8.0f), 5f, 0);
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1.0f);
        } else if (randomInt < 70){
            gameObject.tag = "BubbleType1";
            gameObject.GetComponent<SpriteRenderer>().color = new Color(141f / 255f, 218f / 255f, 80f / 255f, 0.3f);
        }
        else{
            gameObject.tag = "BubbleType2";
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        }
    }

    // Update is called once per frame
    protected virtual void Update(){
        if (transform.localScale.x < maxScale && !isMax){
            transform.localScale += new Vector3(scaleInit*ratio, scaleInit*ratio, 0);
        }
        else{
            isMax = true;
        }

        if (transform.position.y > maxHeight && gameObject.tag != "Pearl"){
            Destroy(gameObject);
        }
    }
}
