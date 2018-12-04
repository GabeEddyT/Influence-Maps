using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public Vector2 GridSize;
    public GameObject NodeObjectPrefab;
    
    void Start()
    {
        Vector2 nodeSpawnLocation = Vector2.zero;
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                GameObject newNode = Instantiate(NodeObjectPrefab, Vector3.zero, Quaternion.identity);
                //create node
            }
        }
    }



}
