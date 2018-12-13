using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    float Weight = 0f;

    public float NodeSizeMultiplier;
    GameObject gridImage;
    public bool isOccupiedByUnit = false;
    public int team = 0; //1 is blue, -1 is red

    public Material frownMat;
    public Material smileMat;

    public Material wallMat;


void Start()
    {
        gridImage = GameObject.CreatePrimitive(PrimitiveType.Plane);
        gridImage.transform.parent = this.gameObject.transform;
        gridImage.transform.localScale /= 10.5f;
        gridImage.transform.localScale *= NodeSizeMultiplier;
        // Invoke("ResetPlaneLocation", .4f);
        ResetPlaneLocation();
    }
    bool inside = false;
    private void Update()
    {
        if (!inside)
        {
            GetComponentInChildren<MeshRenderer>().material.color = Influencer.Eval(Weight);
        }
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
                Material standardShaderMaterial = cube.GetComponent<MeshRenderer>().material = wallMat;
                cube.transform.position = Vector3.Lerp(transform.position, connection.to.transform.position, .5f);
                cube.transform.localScale = Vector3.one * .2f;
                cube.transform.LookAt(connection.to.transform);
                cube.transform.localScale += Vector3.right * .25f;
                //Destroy(cube, 1f);
            }
        }
    }

    public void Colorize(Color color)
    {
        StopAllCoroutines();
        StartCoroutine(ColorizeRoutine(color));
    }

    public IEnumerator ColorizeRoutine(Color color)
    {
        inside = true;
        float time = 0;
        var rend = GetComponentInChildren<MeshRenderer>();
        while (time < 5)
        {
            rend.material.color = Color.Lerp(rend.material.color, color, time / 5);
            time += Time.deltaTime;
            yield return null;
        }
        inside = false;
    }

    public void ResetPlaneLocation()
    {
        gridImage.transform.localPosition = Vector3.zero;   
    }

    public void SetTeam(int newTeam)
    {
        if (newTeam != -1 && newTeam != 1 && newTeam != 0)
        {
            Debug.LogError("Invalid Team Change");
            return;
        }

        if (newTeam == 1)
        {
            team = newTeam;
            GetComponentInChildren<Renderer>().material = frownMat;
        }
        else if (newTeam == -1)
        {
            team = newTeam;
            GetComponentInChildren<Renderer>().material = smileMat;
        }
        else
        {
            GetComponentInChildren<Renderer>().material = default;
        }

    }
    public float getWeight() { return Weight; }
    public void setWeight(float newWeight)
    {
        Weight = newWeight;
    }
        
}
