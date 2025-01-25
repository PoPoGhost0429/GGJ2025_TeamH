using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBubble : MonoBehaviour
{
    public SpawnBubbleSettingSO[] spawnBubbleSetting;
    private RectTransform spawnArea;
    private int parts;

    private void Awake()
    {
        parts = spawnBubbleSetting.Length;
        spawnArea = GetComponent<RectTransform>();
    }
   
    public Vector2 CalculateSpawnPosition(int index)
    {
        float width = spawnArea.rect.width;
        float height = spawnArea.rect.height;

        float xPos = index * (width / parts) - (width / 2);
        xPos = Random.Range(xPos, xPos + (width / parts));
        float yPos = Random.Range(-height / 2, height / 2);

        return new Vector2(xPos, yPos) + (Vector2)spawnArea.position;
    }
}
