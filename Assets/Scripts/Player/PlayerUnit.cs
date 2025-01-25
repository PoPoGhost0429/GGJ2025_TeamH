using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    private PlayerBase playerBase;
    private Rigidbody2D rb;

   public PlayerUnit(PlayerBase playerBase)
   {
        this.playerBase = playerBase;
   }

   private void Start(){
        rb = GetComponent<Rigidbody2D>();
   }

   public void Move(Vector3 moveDirection, float moveSpeed){
        if(rb == null){
            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = moveDirection * moveSpeed;
   }    
}
