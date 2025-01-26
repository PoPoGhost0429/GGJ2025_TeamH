using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawEvent : MonoBehaviour
{
    [SerializeField] private GameObject straw;
    [SerializeField] private float startYPos = 76f;
    [SerializeField] private float endYPos = 36f;

    [SerializeField] private float OffsetXRange = 9f;

    [SerializeField] private float stirRotRange = 9f;
    [SerializeField] private float inOutAnimTime = 3f;

    [SerializeField] private float stirAnimTime = 2f;
    private bool isAnim = false;
   private int animState = 0;
   private float animTime = 0f;

   private int strawStirTimes = 0;
   
   private float nowXPos;
   private float nowRot;

   private float targetRot;

    private void Update(){
        StrawEventUpdate();
    }

   public void StrawEventStart(int stirTimes){
        animState = 0;  
        animTime = 0;
        strawStirTimes = stirTimes;
        nowXPos = Random.Range(-OffsetXRange, OffsetXRange);
        transform.position = new Vector3(nowXPos, startYPos, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        isAnim = true;
   }

    private void StrawEventUpdate(){
        if(!isAnim) return;

        animTime += Time.deltaTime;

        if(animState == 0){
            transform.position = new Vector3(nowXPos, Mathf.Lerp(startYPos, endYPos, Mathf.InverseLerp(0, inOutAnimTime, animTime)), transform.position.z);
            if(animTime >= inOutAnimTime){
                animState = 1;
                nowRot = 0;
                targetRot = Random.Range(-stirRotRange, stirRotRange);
                animTime = 0;
            }
        }
        else if(animState == 1){
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(nowRot, targetRot, Mathf.InverseLerp(0, inOutAnimTime, animTime)));
            if(animTime >= inOutAnimTime){
                if(strawStirTimes > 1){
                    nowRot = targetRot;
                    while(Mathf.Abs(nowRot - targetRot) < 2f){
                        targetRot = Random.Range(-stirRotRange, stirRotRange);
                    }
                    strawStirTimes--;
                }
                else if(strawStirTimes == 1){
                    nowRot = targetRot;
                    targetRot = 0;
                    strawStirTimes--;
                }
                else{
                    animState = 2;
                }
                animTime = 0;
            }
        }
        else if(animState == 2){
            transform.position = new Vector3(nowXPos, Mathf.Lerp(endYPos, startYPos, Mathf.InverseLerp(0, inOutAnimTime, animTime)), transform.position.z);
            if(animTime >= inOutAnimTime){
                animState = 0;
                animTime = 0;
                isAnim = false;
            }
        }
    }
}

