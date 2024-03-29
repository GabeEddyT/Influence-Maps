﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Dijkstras;
using static Influencer;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    GameObject textTransform;
    public GameObject helpText;
    static float multiplier = 5.0f;
    public InputManager Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gridGenerator = FindObjectOfType<GenerateGrid>();
        exampleNode = gridGenerator.NodeObjectPrefab.GetComponent<Node>();
        textTransform = Camera.main.GetComponentInChildren<Text>().gameObject;
        textTransform.GetComponent<Text>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        textTransform.GetComponent<RectTransform>().position = Input.mousePosition + Vector3.left * textTransform.GetComponent<Text>().fontSize;
    }

    private void LateUpdate()
    {
        ColorPath(prevPath);
        if (startNode)
        {
            startNode.GetComponentInChildren<MeshRenderer>().material.color = new Color(0, 1, 0);
        }
    }

    public void CallColorize(NodeArray list)
    {
        StartCoroutine(Colorize(list));
    }

    public IEnumerator Colorize(NodeArray list)
    {
        yield return null;
        float waitTime = 0;
        while (list.Count > 0)
        {
            if (waitTime > 2)
            {
                var node = list[0];
                node.GetComponentInChildren<MeshRenderer>().material.color = Eval(node.getWeight());
                list.Remove(node);
                
                yield return new WaitForSecondsRealtime(2);
                waitTime = 0;
            }
            waitTime += Time.deltaTime;
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
                            if (maxWeight >= .3f)
                            {
                                foundPathWithMaxWeight = true;
                                break;
                            }
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
                               // foundPathWithMaxWeight = true;
                             //   break;
                       		}
                       }
                        if (maxWeight >= .3f)
                        {
                            prevPath = FindPath(startNode, endNode);
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
                    hitNode.StopAllCoroutines();
                    Destroy(hitNode);
                    hit.transform.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0,1);
                }
            }           

        }

        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                multiplier += multiplier < 25 ? 1 : 0;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                multiplier -= multiplier > 0 ? 1 : 0;
            }
            StopAllCoroutines();
            StartCoroutine(PreviewRadius());
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
                if (hit.transform.parent.GetComponent<Node>().isOccupiedByUnit == true)
                {
                    if (hit.transform.parent.GetComponent<Node>().team == 1)
                    { 
                    }
                    else
                    {
                        hit.transform.parent.GetComponent<Node>().isOccupiedByUnit = true;
                        hit.transform.parent.GetComponent<Node>().SetTeam(1);//blue
                        PropagateInfluence(hit.transform.parent.GetComponent<Node>(), multiplier);
                    }
                }
                else
                {
                    hit.transform.parent.GetComponent<Node>().isOccupiedByUnit = true;
                    hit.transform.parent.GetComponent<Node>().SetTeam(1);//blue
                    PropagateInfluence(hit.transform.parent.GetComponent<Node>(), multiplier);
                }
              //  PropagateInfluence(hit.transform.parent.GetComponent<Node>(), multiplier);
              //  hit.transform.parent.GetComponent<Node>().isOccupiedByUnit = true;
              //  hit.transform.parent.GetComponent<Node>().team = 1;//blue
            }
            RefreshBorder();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.SphereCast(ray, 1.0f, out RaycastHit hit) && hit.transform.parent.GetComponent<Node>() != null)
            {

                if (hit.transform.parent.GetComponent<Node>().isOccupiedByUnit == true)
                {
                    if (hit.transform.parent.GetComponent<Node>().team == -1)
                    {
                    }
                    else
                    {
                        hit.transform.parent.GetComponent<Node>().isOccupiedByUnit = true;
                        hit.transform.parent.GetComponent<Node>().SetTeam(-1);//red
                        PropagateInfluence(hit.transform.parent.GetComponent<Node>(), multiplier, -1);
                    }
                }
                else
                {
                    hit.transform.parent.GetComponent<Node>().isOccupiedByUnit = true;
                    hit.transform.parent.GetComponent<Node>().SetTeam(-1);//red
                    PropagateInfluence(hit.transform.parent.GetComponent<Node>(), multiplier, -1);
                }
                //  PropagateInfluence(hit.transform.parent.GetComponent<Node>(), multiplier);
                //  hit.transform.parent.GetComponent<Node>().isOccupiedByUnit = true;
                //  hit.transform.parent.GetComponent<Node>().team = 1;//blue
            }
            RefreshBorder();
        }
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(0);
		}

        helpText.transform.GetChild(0).gameObject.SetActive(Input.GetKey(KeyCode.Tab));
    }

    IEnumerator PreviewRadius()
    {
        textTransform.GetComponent<Text>().text = multiplier + "";
        textTransform.GetComponent<Text>().enabled = true;        
        yield return new WaitForSeconds(1);
        textTransform.GetComponent<Text>().enabled = false;
    }

    void RefreshBorder()
    {
        foreach(var border in GameObject.FindGameObjectsWithTag("Border"))
        {
            Destroy(border);
        }
        foreach (var node in FindObjectsOfType<Node>())
        {
            node.Refresh();
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
