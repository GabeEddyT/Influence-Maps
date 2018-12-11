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

public static class Dijkstras
{
    public static readonly Gradient PathGradient = new Gradient
    {
        colorKeys = new GradientColorKey[] { new GradientColorKey(Color.yellow, 0), new GradientColorKey(Color.green, 1) },
        alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
    };

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

    public static NodeList FindPath(Node from, Node to)
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
                float endNodeCost = currentNode.costSoFar + con.cost + endNode.getWeight(); //added get weight which should be set later with the influence map
               
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

        nodesToAdd.Add(startRecord);        

        for (int i = 0;  i < nodesToAdd.Count; ++i)
        {
            NodeRecord record = nodesToAdd[i];
            record.node.GetComponentInChildren<MeshRenderer>().material.color = PathGradient.Evaluate(i * 1.0f / (nodesToAdd.Count));
            //store path 
        }
        return nodesToAdd;
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

    public static ConnectionList GetConnections(Node node)
    {
        ConnectionList connections = new ConnectionList();
        var hits = Physics.OverlapBox(node.transform.position, Vector3.one * node.NodeSizeMultiplier);
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

    public static void ColorPath(NodeList path)
    {
        int i = 0;
        foreach(NodeRecord record in path)
        {
            if (record.node)
            {
                record.node.GetComponentInChildren<MeshRenderer>().material.color = PathGradient.Evaluate(i * 1.0f / (path.Count));
            }
            i++;
        }
    }
}
