using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerInitData
{
    public int unitAmount;

    public float unitGenerageRange;
}

public class PlayerBase
{
    private PlayerInitData initData;
    private List<PlayerUnit> unitList = new List<PlayerUnit>();
    private PlayerSystem playerSystem;

    private float moveSpeed = 1.0f;
    private float rotationRadius = 2f;    // 當前旋轉半徑
    private float targetRadius = 2f;      // 目標旋轉半徑
    private float minRadius = 1f;         // 最小半徑
    private float maxRadius = 3f;         // 最大半徑
    private float radiusChangeSpeed = 2f; // 半徑變化速度
    private float rotationSpeed = 2f;     // 旋轉速度
    private float currentAngle = 0f;      // 當前角度
    private Vector3 groupPosition;        // 群體位置

    public PlayerBase(PlayerInitData data)
    {
        playerSystem = PlayerSystem.Instance;
        this.initData = data;
        for (int i = 0; i < initData.unitAmount; i++)
        {
            GenerateUnit();
        }
        
        groupPosition = GetUnitCenterPosition();
    }

    public void Move(InputData inputData){
        // 更新群體位置
        groupPosition += (Vector3)inputData.moveDirection * moveSpeed * Time.deltaTime;
        Debug.Log(groupPosition);
        // foreach (var unit in unitList)
        // {
        //     unit.Move(inputData.moveDirection, moveSpeed);
        // }
    }

    public void RotateUnits()
    {
        rotationRadius = Mathf.Lerp(rotationRadius, targetRadius, radiusChangeSpeed * Time.deltaTime);
        currentAngle += rotationSpeed * Time.deltaTime;
        
        // 使用群體位置作為旋轉中心
        Vector3 centerPos = groupPosition;
        
        for (int i = 0; i < unitList.Count; i++)
        {
            float angleOffset = (360f / unitList.Count) * i;
            float finalAngle = currentAngle + angleOffset;
            
            Vector3 offset = new Vector3(
                Mathf.Cos(finalAngle * Mathf.Deg2Rad) * rotationRadius,
                Mathf.Sin(finalAngle * Mathf.Deg2Rad) * rotationRadius,
                0
            );
            
            Vector3 targetPosition = centerPos + offset;
            Vector2 moveDirection = ((Vector2)(targetPosition - unitList[i].transform.position)).normalized;
            unitList[i].Move(moveDirection, moveSpeed);
        }
    }

    public void GenerateUnit(){
        Vector3 randomOffset = new Vector3(Random.Range(-initData.unitGenerageRange, initData.unitGenerageRange), Random.Range(-initData.unitGenerageRange, initData.unitGenerageRange));
        GameObject unit = playerSystem.GenerateUnit(GetUnitCenterPosition() + randomOffset); 
        PlayerUnit playerUnit = unit.GetComponent<PlayerUnit>();
        unitList.Add(playerUnit);
    }

    // 取得所有單位中心點位置（相對於群體位置的偏移）
    public Vector3 GetUnitCenterPosition(){
        Vector3 centerPosition = Vector3.zero;
        foreach (var unit in unitList)
        {
            centerPosition += unit.transform.position;
        }
        if (unitList.Count > 0)
        {
            return (centerPosition / unitList.Count) - groupPosition;
        }
        return Vector3.zero;
    }

    // 設置目標半徑
    public void SetTargetRadius(float radius)
    {
        targetRadius = Mathf.Clamp(radius, minRadius, maxRadius);
    }

    // 擴大半徑
    public void ExpandRadius(float amount = 1f)
    {
        SetTargetRadius(targetRadius + amount);
    }

    // 縮小半徑
    public void ShrinkRadius(float amount = 1f)
    {
        SetTargetRadius(targetRadius - amount);
    }

    // public void ReturnUnit(GameObject unit){
    //     unitList.Remove(unit);
    //     playerSystem.ReturnUnit(unit);
    // }
}
