using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LiquidController : MonoBehaviour
{
   public int index = 0;
   public float height = 0f;

   private SpriteShapeController spriteShapeController;

   private Spline spline;

   public List<Vector2> splinePoints = new List<Vector2>();

   public float speed = 30f;

   public float radDis = 100f;

   public float  currAngle = 0f;
   public float Str = 0f;

   public void Awake(){
        spriteShapeController = GetComponent<SpriteShapeController>();
        spline = spriteShapeController.spline;

        for(int i = 0; i<spline.GetPointCount(); i++){
            splinePoints.Add(spline.GetPosition(i));
        }
   }

   private void Update(){
        // for(int i = 0; i<spline.GetPointCount(); i++){
        //     splinePoints[i] = spline.GetPosition(i);
        // }

        currAngle += speed * Time.deltaTime;
        if(currAngle > 360f){
            currAngle = 0f;
        }

        float dH1 = Mathf.Sin(currAngle * Mathf.Deg2Rad) * Str;
        float dH2 = Mathf.Sin(currAngle * Mathf.Deg2Rad + radDis * Mathf.Deg2Rad) * Str;

        spline.SetPosition(2, splinePoints[2] + new Vector2(0f, dH1));
        spline.SetPosition(3, splinePoints[3] + new Vector2(0f, dH2));
   }
}
