using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    float Weight = 0f;

    public float NodeSizeMultiplier;
    GameObject gridImage;
    void Start()
    {
        gridImage = GameObject.CreatePrimitive(PrimitiveType.Plane);
        gridImage.transform.parent = this.gameObject.transform;
        gridImage.transform.localScale /= 10.5f;
        gridImage.transform.localScale *= NodeSizeMultiplier;
        // Invoke("ResetPlaneLocation", .4f);
        ResetPlaneLocation();
    }

    private void Update()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Influencer.Eval(Weight);
    }

    public void ResetPlaneLocation()
    {
        gridImage.transform.localPosition = Vector3.zero;   
    }


    public float getWeight() { return Weight; }
    public void setWeight(float newWeight)
    {
        Weight = newWeight;
    }
        
}
