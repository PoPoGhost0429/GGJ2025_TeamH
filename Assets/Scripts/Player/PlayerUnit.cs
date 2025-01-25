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

    private List<bubbleController> bubbleList = new List<bubbleController>();

   private void Start(){
        rb = GetComponent<Rigidbody2D>();
   }

   public void SetPlayerBase(PlayerBase playerBase){
        this.playerBase = playerBase;
   }

   public void Move(Vector3 moveDirection, float moveSpeed){
        if(rb == null){
            rb = GetComponent<Rigidbody2D>();
        }
        
        // 計算目標位置
        targetPosition = (Vector2)transform.position + (Vector2)moveDirection * moveSpeed * Time.fixedDeltaTime;
        
        // 使用 MovePosition 進行平滑移動
        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, smoothSpeed * extraSpeed * Time.fixedDeltaTime));
   }

   private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("BubbleType1")){
                bubbleController bubble = other.gameObject.GetComponent<bubbleController>();
                // bubble.absorption();
                playerBase.AddExtraMoveSpeed(true);
                Invoke("CancelExtraMoveSpeed", 1);
                CancelInvoke("GetAir");
            }
        if(other.gameObject.CompareTag("Pearl")){
            playerBase.ReturnUnit(this);
            CancelInvoke("GetAir");
        }
   }

   private void GetAir(){
        playerBase.AddAir();
   }

   private void CancelExtraMoveSpeed(){
        playerBase.ClearExtraMoveSpeed();
   }

   private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("BubbleType2")){
            InvokeRepeating("GetAir", 0, 1);
        }
   }

   private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("BubbleType2")){
            CancelInvoke("GetAir");
        }
   }
}

