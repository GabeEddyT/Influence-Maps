using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    float Weight = 0.0f;

    void Start()
    {

    }


    float getWeight() { return Weight; }
    void setWeight(float newWeight)
    {
        Weight = newWeight;
    }

}
