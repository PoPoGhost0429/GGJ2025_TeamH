using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInitData
{
}

public class PlayerBase
{
    private PlayerInitData initData;

    public PlayerBase(PlayerInitData data)
    {
        this.initData = data;
    }

    public void Move(InputData inputData){

    }

    public void GenerateUnit(){

    }
}
