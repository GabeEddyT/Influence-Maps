using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Dijkstras;
using static Influencer;

public class InputManager : MonoBehaviour
{
    public GameObject Structure1;

    GenerateGrid gridGenerator;
    Node exampleNode;
    Node startNode;
    Node endNode;
    bool findPath = false;

    bool enterUnitPlacingMode = false;
    NodeList prevPath = new NodeList();

    // Start is called before the first frame update
    void Start()
    {
        gridGenerator = FindObjectOfType<GenerateGrid>();
        exampleNode = gridGenerator.NodeObjectPrefab.GetComponent<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void LateUpdate()
    {
        ColorPath(prevPath);
        if (startNode)
        {
            startNode.GetComponentInChildren<MeshRenderer>().material.color = new Color(0, 1, 0);
        }
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
                    if (findPath && startNode)
                    {
                        ClearPath(prevPath);
                        endNode = hitNode;

						bool foundPathWithMaxWeight = false;
						float maxWeight = 0f;

                       // prevPath = FindPath(startNode, endNode);

						while (foundPathWithMaxWeight == false)
						{
							prevPath = FindPathWithMaxWeight(startNode, endNode, maxWeight);
							int last = prevPath.Count - 1;

							foreach(NodeRecord record in prevPath)
							{
								if (record.node == endNode)
								{
									//prevPath = FindPath(startNode, endNode);
									foundPathWithMaxWeight = true;
									break;
								}
							}
							if (!foundPathWithMaxWeight)
								{
									prevPath.Clear();
									maxWeight += .1f;
								}
						}

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
        else if (Input.GetMouseButton(1))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.SphereCast(ray, 1.0f, out RaycastHit hit))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Node hitNode = hit.transform.parent.GetComponent<Node>();
                    
                    if (!hitNode)
                    {
                        var node = hit.transform.parent.gameObject.AddComponent<Node>();
                        node.setWeight(exampleNode.getWeight());
                        node.NodeSizeMultiplier = exampleNode.NodeSizeMultiplier;

                        // Destroy plane since the node will create a new one.
                        Destroy(hit.transform.gameObject);
                    }
                }
                else if (hit.transform.parent.GetComponent<Node>() != null)
                {
                    Node hitNode = hit.transform.parent.GetComponent<Node>();
                    hit.transform.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
                    Destroy(hitNode);
                }
            }           

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            enterUnitPlacingMode = !enterUnitPlacingMode;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.SphereCast(ray, 1.0f, out RaycastHit hit) && hit.transform.parent.GetComponent<Node>() != null)
            {
                PropagateInfluence(hit.transform.parent.GetComponent<Node>(), 5.0f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.SphereCast(ray, 1.0f, out RaycastHit hit) && hit.transform.parent.GetComponent<Node>() != null)
            {
                PropagateInfluence(hit.transform.parent.GetComponent<Node>(), 5.0f, -1);
            }

        }
    }

    private void ClearPath(NodeList path)
    {
        foreach(NodeRecord record in path)
        {
            if (record.node)
                record.node.GetComponentInChildren<MeshRenderer>().material.color = new Color(1, 1, 1);
        }
    }
}
