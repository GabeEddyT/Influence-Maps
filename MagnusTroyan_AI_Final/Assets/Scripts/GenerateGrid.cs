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
                Vector3 NewNodePos = new Vector3(x * NodeObjectPrefab.GetComponent<Node>().NodeSizeMultiplier,
                                                0, y * NodeObjectPrefab.GetComponent<Node>().NodeSizeMultiplier);
                newNode.transform.position = NewNodePos;

                //create node
            }
        }

        StartCoroutine(Delayed());
    }

    IEnumerator Delayed()
    {
        yield return new WaitForEndOfFrame();
        var nodes = FindObjectsOfType<Node>();
        Dijkstras.FindPath(nodes[0], nodes[206]);
    }

}
