using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

namespace ExtensionMethods
{
    using static Dijkstras;
    public static class MyExtensions
    {
        
        public static bool Contains(this NodeList list, Node node)
        {
            foreach (NodeRecord record in list)
            {
                if (record.node == node)
                {
                    return true;
                }
            }
            return false;
        }

        public static NodeRecord FindNode(this NodeList list, Node node)
        {
            foreach(NodeRecord record in list)
            {
                if (record.node == node)
                {
                    return record;
                }
            }
            return default;
        }
    }
}

public class Dijkstras : MonoBehaviour
{   
    public struct Connection
    {
        public Connection(Node from, Node to, float cost = 1) { this.from = from; this.to = to; this.cost = cost; }
        public Node from;
        public Node to;
        public float cost;
    }

    public struct NodeRecord
    {
        public Node node;
        public Connection connection;
        public float costSoFar;
    }

    // Alias ?
    public class NodeList : List<NodeRecord> { }
    public class ConnectionList : List<Connection> { }

    public static void FindPath(Node from, Node to)
    {
        NodeList openList = new NodeList();
        NodeList closedList = new NodeList();
        ConnectionList connections = new ConnectionList();

        NodeRecord startRecord = new NodeRecord
        {
            node = from,
            costSoFar = 0
        };

        openList.Add(startRecord);

        NodeRecord currentNode = new NodeRecord();

        while (openList.Count > 0)
        {
            currentNode = FindSmallestNode(openList);
            if (currentNode.node == to)
            {
                break;
            }

            connections = GetConnections(currentNode.node);

            foreach (Connection con in connections)
            {
                Node endNode = con.to;
                float endNodeCost = currentNode.costSoFar + con.cost;

                NodeRecord endNodeRecord;

                if (closedList.Contains(endNode))
                {
                    continue;
                }
                else if(openList.Contains(endNode))
                {
                    endNodeRecord = openList.FindNode(endNode);

                    if (endNodeRecord.costSoFar <= endNodeCost)
                    {
                        continue;
                    }
                }
                else
                {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endNode;
                }

                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = con;

                openList.Add(endNodeRecord);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
        }

        NodeList nodesToAdd = new NodeList();

        while (currentNode.node != from)
        {
            nodesToAdd.Add(currentNode);
            currentNode = closedList.FindNode(currentNode.connection.from);
        }


        for (int i = 0;  i < nodesToAdd.Count; ++i)
        {
            NodeRecord record = nodesToAdd[i];
            record.node.GetComponentInChildren<MeshRenderer>().material.color = new Color(i * 1.0f / nodesToAdd.Count, 0, 0);
        }
    }

    static NodeRecord FindSmallestNode(NodeList nodeRecords)
    {
        NodeRecord smallest = nodeRecords[0];

        foreach (NodeRecord temp in nodeRecords)
        {
            smallest = temp.costSoFar < smallest.costSoFar ? temp : smallest;
        }

        return smallest;
    }

    static ConnectionList GetConnections(Node node)
    {
        ConnectionList connections = new ConnectionList();
        var hits = Physics.OverlapBox(node.transform.position, Vector3.one);
        foreach(var hit in hits)
        {
            Node hitNode = hit.GetComponentInParent<Node>();
            if (hitNode && hitNode != node)
            {
                Connection connection = new Connection(node, hitNode, Vector3.Distance(node.transform.position, hitNode.transform.position));
                connections.Add(connection);
            }
        }

        return connections;
    }
}
