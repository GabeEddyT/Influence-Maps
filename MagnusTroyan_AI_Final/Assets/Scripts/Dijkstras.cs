using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstras : MonoBehaviour
{
    public struct Connection
    {
        public Connection(Node from, Node to) { this.from = from; this.to = to; }
        public Node from;
        public Node to;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void FindPath(Node from, Node to)
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
                float endNodeCost = currentNode.costSoFar + 1;

                NodeRecord endNodeRecord;
            }
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
            Node hitNode = hit.GetComponent<Node>();
            if (hitNode && hitNode != node)
            {
                Connection connection = new Connection(node, hitNode);
                connections.Add(connection);
            }
        }

        return connections;
    }

    static bool Contains(NodeList list, Node node)
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
}
