using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerInitData
{
    public int unitAmount;
    public float unitMoveSpeed;

    public float unitScale;

    public float unitGenerageRange;

    public int GenerateUnitAirAmount;
}

public class PlayerBase
{
    private PlayerInitData initData;
    private List<PlayerUnit> unitList = new List<PlayerUnit>();
    private PlayerSystem playerSystem;

    private float rotationRadius = 2f;    // 當前旋轉半徑
    private float targetRadius = 0.5f;      // 目標旋轉半徑
    private float minRadius = 0.5f;         // 最小半徑
    private float maxRadius = 2f;         // 最大半徑

    private float extraMoveSpeed = 0f;

    private float extraRadius = 0f;

    private float extraRadiusSpeed = 0f;

    private float dispersionRadius = 3f;

    private float radiusChangeSpeed = 1f; // 降低半徑變化速度
    private float rotationSpeed = 3f;   // 降低旋轉速度
    private float currentAngle = 0f;      // 當前角度
    private Vector3 groupPosition;        // 群體位置

    private int airAmount;

    public PlayerBase(int playerID, PlayerInitData data)
    {
        playerSystem = PlayerSystem.Instance;
        this.initData = data;
        for (int i = 0; i < initData.unitAmount; i++)
        {
            GenerateUnit();
        }
        
        groupPosition = GetUnitCenterPosition();

        InputSystem.Instance.PlayerControllers[playerID].OnInputEvent += Move;
    }

    public bool CheckIsAlive(){
        return unitList.Count > 0;
    }

    public int GetUnitAmount(){
        return unitList.Count;
    }

    public void AddExtraMoveSpeed(bool spedUp){
        extraMoveSpeed = spedUp ? 2f : -2f;
    }

    public void ClearExtraMoveSpeed(){
        extraMoveSpeed = 0f;
    }

    public void Move(InputData inputData){
        // 更新群體位置
        groupPosition += (Vector3)inputData.moveDirection * initData.unitMoveSpeed * Time.deltaTime;
        // groupPosition += (Vector3)inputData.moveDirection * initData.unitMoveSpeed * Time.deltaTime;
    }

    public void RotateUnits()
    {
        float finalRadius = targetRadius + extraRadius;
        rotationRadius = Mathf.Lerp(rotationRadius, finalRadius, (radiusChangeSpeed + extraRadiusSpeed) * Time.deltaTime);
        currentAngle += rotationSpeed * Time.deltaTime;
        
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
            Vector2 moveDirection = ((Vector2)(targetPosition - unitList[i].transform.position));
            // 不需要 normalized，讓距離影響移動速度
            unitList[i].Move(moveDirection, initData.unitMoveSpeed + extraMoveSpeed);
        }
    }

    public void GenerateUnit(){
        Vector3 randomOffset = new Vector3(Random.Range(-initData.unitGenerageRange, initData.unitGenerageRange), Random.Range(-initData.unitGenerageRange, initData.unitGenerageRange));
        GameObject unit = playerSystem.GenerateUnit(GetUnitCenterPosition() + randomOffset); 
        unit.transform.localScale = Vector3.one * initData.unitScale;
        PlayerUnit playerUnit = unit.GetComponent<PlayerUnit>();
        playerUnit.SetPlayerBase(this);
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

    public void AddAir(){
        airAmount++;
        if(airAmount >= initData.GenerateUnitAirAmount){
            GenerateUnit();
            airAmount = 0;
        }
    }

    public void Dispersion()
    {
        extraRadius = dispersionRadius;
        extraRadiusSpeed = 3f;
        foreach(var unit in unitList){
            unit.extraSpeed = 3f;
        }
    }

    public void Polymerization()
    {
        extraRadius -= 0.5f;
        extraRadiusSpeed -= 0.5f;
        foreach(var unit in unitList){
            unit.extraSpeed -= 0.4f;
            unit.extraSpeed = Mathf.Clamp(unit.extraSpeed, 1f, 3f);
        }
        if(extraRadius < 0f){
            extraRadius = 0f;
        }
        if(extraRadiusSpeed < 0f){
            extraRadiusSpeed = 0f;
        }
    }

    public void ReturnUnit(PlayerUnit unit){
        unitList.Remove(unit);
        playerSystem.ReturnUnit(unit.gameObject);
    }
}
