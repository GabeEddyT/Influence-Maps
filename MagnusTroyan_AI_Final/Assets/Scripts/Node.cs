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

    public void Refresh()
    {
        Dijkstras.ConnectionList connections = Dijkstras.GetConnections(this);
        foreach (var connection in connections)
        {
            if (Weight == 0 && !Mathf.Approximately(connection.to.getWeight(),0) || Mathf.Sign(connection.to.getWeight()) != Mathf.Sign(Weight) )
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.tag = "Border";
                DestroyImmediate(cube.GetComponent<Collider>());
                cube.GetComponent<MeshRenderer>().material.color = Color.magenta * .5f;
                cube.transform.position = Vector3.Lerp(transform.position, connection.to.transform.position, .5f);
                cube.transform.localScale = Vector3.one * .2f;
                cube.transform.LookAt(connection.to.transform);
                cube.transform.localScale += Vector3.right * .25f;
                //Destroy(cube, 1f);
            }
        }
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
