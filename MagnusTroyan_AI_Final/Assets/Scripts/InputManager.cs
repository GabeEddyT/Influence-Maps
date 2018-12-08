using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Dijkstras;

public class InputManager : MonoBehaviour
{
    public GameObject Structure1;

    Node startNode;
    Node endNode;
    bool findPath = false;

    bool enterUnitPlacingMode = false;
    NodeList prevPath = new NodeList();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            print("click");
            if (Physics.SphereCast(ray, 1.0f, out RaycastHit hit) && hit.transform.parent.GetComponent<Node>() != null)
            {
                Node hitNode = hit.transform.parent.GetComponent<Node>();
                if (enterUnitPlacingMode == true)
                {
                    hitNode.setWeight(500);
                    print("changing weight of " + hitNode.name);
                    //spawn unit on this node
                    //propogate weights
                    //also remember red or blue faction
                }
                else
                {
                    print("hit node " + hit.transform.parent.name);
                    if (findPath)
                    {
                        ClearPath(prevPath);
                        endNode = hitNode;
                        prevPath = FindPath(startNode, endNode);
                        findPath = false;
                    }
                    else
                    {
                        startNode = hitNode;
                        startNode.GetComponentInChildren<MeshRenderer>().material.color = new Color(0, 1, 0);
                        findPath = true;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            enterUnitPlacingMode = !enterUnitPlacingMode;
        }
    }

    private void ClearPath(NodeList path)
    {
        foreach(NodeRecord record in path)
        {
            record.node.GetComponentInChildren<MeshRenderer>().material.color = new Color(1, 1, 1);
        }
    }
}
