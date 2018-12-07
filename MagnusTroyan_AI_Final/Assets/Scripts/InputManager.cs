using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject Structure1;

    Node startNode;
    Node endNode;
    bool findPath = false;

    bool enterUnitPlacingMode = false;

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
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            print("click");
            hits = Physics.RaycastAll(ray);
            foreach(RaycastHit hit in hits)
            {
                if (hit.transform.parent.GetComponent<Node>() != null)
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
                            endNode = hitNode;
                            Dijkstras.FindPath(startNode, endNode);
                            findPath = false;
                        }
                        else
                        {
                            startNode = hitNode;
                            findPath = true;
                        }
                        break;
                    }
                }
            }
          // if (Physics.Raycast(ray, out hit))
          // {
          //     
          //
          // }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            enterUnitPlacingMode = !enterUnitPlacingMode;
        }
    }
}
