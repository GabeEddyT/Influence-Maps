using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Dijkstras;

public class Influencer : MonoBehaviour
{
    readonly public static Gradient InfluenceGradient = new Gradient
    {
        colorKeys = new GradientColorKey[] { new GradientColorKey(Color.red, 0), new GradientColorKey(Color.white, .5f), new GradientColorKey(Color.blue, 1) },
        alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
    };

    class NodeArray : List<Node> { }

    public static void PropagateInfluence(Node node, float strength, float team = 1)
    {
        
        float Normalize(float x)
        { return (x / strength); }
        
        //var hits = Physics.OverlapSphere(node.transform.position, strength);
        NodeArray open = new NodeArray();
        NodeArray closed = new NodeArray();
        Dictionary <Node, float> weightList = new Dictionary<Node, float>();

        open.Add(node);
        float initWeight = node.getWeight();
        node.setWeight(team);

        Dictionary<Node, int> depthMap = new Dictionary<Node, int>();

        depthMap.Add(node, 0);

        while (open.Count > 0)
        {
            var myNode = open[0];
            open.Remove(myNode);
            if(!closed.Contains(myNode)) closed.Add(myNode);

            var connections = GetConnections(myNode);
            
            foreach(Connection connection in connections)
            {
                if(!closed.Contains(connection.to) && Vector3.Distance(connection.to.transform.position, node.transform.position) < strength)
                {
                    int depth = depthMap[myNode] + 1;
                    depthMap[connection.to] = depth;
                    closed.Add(connection.to);
                    weightList.Add(connection.to, connection.to.getWeight());
                    if(!open.Contains(connection.to)) open.Add(connection.to);
                    connection.to.setWeight(myNode.getWeight() * Mathf.Pow(Mathf.Abs(Normalize(strength - (depth * myNode.NodeSizeMultiplier))),.5f));                                
                }
            }
        }

        node.setWeight(initWeight + node.getWeight());

        foreach(var myPair in weightList)
        {
            var myNode = myPair.Key;
            var myWeight = myPair.Value;
            myNode.setWeight(myWeight + myNode.getWeight());
        }

        foreach(var myNode in closed)
        {
            if (!myNode) continue;
            myNode.GetComponentInChildren<MeshRenderer>().material.color = Eval(myNode.getWeight());
        }

        //foreach (var hit in hits)
        //{
        //    Node myNode = hit.GetComponentInParent<Node>();
        //    if (!myNode) continue;
        //    myNode.setWeight( Normalize (strength - Mathf.Clamp(Vector3.Distance(myNode.transform.position, node.transform.position),0,strength)) * team + myNode.getWeight());
        //    hit.GetComponent<MeshRenderer>().material.color = Eval(myNode.getWeight());
        //}
    }

    public static Color Eval(float val)
    {
        return InfluenceGradient.Evaluate(Mathf.Sign(val) * Mathf.Pow(Mathf.Abs(val),.5f) / 2 + .5f);
    }
}
