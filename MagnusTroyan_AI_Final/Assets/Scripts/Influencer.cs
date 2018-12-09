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

    public static void PropagateInfluence(Node node, float strength, float team = 1)
    {
        float Normalize(float x)
        { return (x / strength); }

        var hits = Physics.OverlapSphere(node.transform.position, strength);
        foreach (var hit in hits)
        {
            Node myNode = hit.GetComponentInParent<Node>();
            if (!myNode) continue;
            myNode.setWeight( Normalize (strength - Vector3.Distance(myNode.transform.position, node.transform.position)) * team + myNode.getWeight());
            hit.GetComponent<MeshRenderer>().material.color = Eval(myNode.getWeight());
        }
    }

    public static Color Eval(float val)
    {
        return InfluenceGradient.Evaluate(val / 2 + .5f);
    }
}
