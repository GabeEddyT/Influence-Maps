using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Dijkstras;

public class Influencer : MonoBehaviour
{
    public static void PropagateInfluence(Node node, float strength)
    {
        float Normalize(float x)
        { return (x / strength); }

        var hits = Physics.OverlapSphere(node.transform.position, strength);
        foreach (var hit in hits)
        {
            Node myNode = hit.GetComponentInParent<Node>();
            myNode.setWeight( Normalize (strength - Vector3.Distance(myNode.transform.position, node.transform.position)));
        }
    }
}
