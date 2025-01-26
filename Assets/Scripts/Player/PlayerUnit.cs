using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    private PlayerBase playerBase;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private float smoothSpeed = 5f;
    
    public float extraSpeed = 1f;

    private List<BubbleTrigger> bubbleList = new List<BubbleTrigger>();

    private bool isActivate = false;

    private float maxHeight = 17f;

   private void Start(){
        rb = GetComponent<Rigidbody2D>();

        
        InvokeRepeating("GetAir", 0, 0.5f);
        isActivate = true;
   }

   public void SetPlayerBase(PlayerBase playerBase){
        this.playerBase = playerBase;
   }

   public void SetMaxHeight(float maxHeight){
        this.maxHeight = maxHeight;
   }

   public void Move(Vector3 moveDirection, float moveSpeed){
        if(!isActivate){
            return;
        }
        if(rb == null){
            rb = GetComponent<Rigidbody2D>();
        }
        
        // 計算目標位置
        targetPosition = (Vector2)transform.position + (Vector2)moveDirection * moveSpeed * Time.fixedDeltaTime;
        
        // 限制高度上限
        if(targetPosition.y > maxHeight) {
            targetPosition.y = maxHeight;
        }
        
        // 使用 MovePosition 進行平滑移動
        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, smoothSpeed * extraSpeed * Time.fixedDeltaTime));
   }

   private void OnCollisionEnter2D(Collision2D other) {
        if(!isActivate){
            return;
        }
        // Debug.Log(other.gameObject.tag);
        if(other.gameObject.CompareTag("BubbleType2")){
                Debug.Log(other.gameObject.name);
                bubbleController bubble = other.gameObject.GetComponent<bubbleController>();
                BubbleTrigger bubbleTrigger = other.transform.GetChild(0).GetComponent<BubbleTrigger>();
                playerBase.AddAir((int)bubbleTrigger.getGasValue());
                // bubbleList.Remove(bubbleTrigger);
                // bubble.absorption();
                
                if(bubble.getBubbleType() == "Rainbow"){
                    playerBase.AddExtraMoveSpeed(true);
                    Invoke("CancelExtraMoveSpeed", 5);
                    AudioController.Instance.PlaySound(1);
                }
                else{
                    AudioController.Instance.PlaySound(Random.Range(5, 11));
                }
            }
        if(other.gameObject.CompareTag("Pearl")){
               Animator animator = GetComponent<Animator>();
               animator.SetTrigger("Explode");
            isActivate = false;
            Invoke("ReturnUnit", 0.4f);
        }
   }

   private void ReturnUnit(){
        playerBase.ReturnUnit(this);
   }

   private void GetAir(){
        if(!isActivate){
            return;
        }
        if(bubbleList.Count > 0){
            foreach(var bubble in bubbleList){
                playerBase.AddAir((int)bubble.getGasValue());
            }
        }
   }

   private void CancelExtraMoveSpeed(){
        playerBase.ClearExtraMoveSpeed();
   }

   private void OnTriggerEnter2D(Collider2D other) {
        if(!isActivate){
            return;
        }
        if(other.gameObject.CompareTag("BubbleType2")){
            BubbleTrigger bubbleTrigger = other.gameObject.GetComponent<BubbleTrigger>();
            bubbleList.Add(bubbleTrigger);
        }
   }

   private void OnTriggerExit2D(Collider2D other) {
        if(!isActivate){
            return;
        }
        if(other.gameObject.CompareTag("BubbleType2")){
            BubbleTrigger bubbleTrigger = other.gameObject.GetComponent<BubbleTrigger>();
            bubbleList.Remove(bubbleTrigger);
        }
   }
}

