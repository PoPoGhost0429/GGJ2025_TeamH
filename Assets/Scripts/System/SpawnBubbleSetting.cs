using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnBubbleSetting", menuName = "ScriptableObjects/SpawnBubbleSetting", order = 1)]
public class SpawnBubbleSettingSO : ScriptableObject
{
    public int index;
    public float spawnDuration;
    public float bubbleSize;
}