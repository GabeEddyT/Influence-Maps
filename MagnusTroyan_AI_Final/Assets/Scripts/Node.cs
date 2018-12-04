using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    float Weight = 0.0f;

    public float NodeSizeMultiplier;

    void Start()
    {
        GameObject gridImage = GameObject.CreatePrimitive(PrimitiveType.Plane);
        gridImage.transform.localScale /= 10;
        gridImage.transform.localScale *= NodeSizeMultiplier;
    }


    float getWeight() { return Weight; }
    void setWeight(float newWeight)
    {
        Weight = newWeight;
    }

}
