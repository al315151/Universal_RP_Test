using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CubeManager;

public class Cube : MonoBehaviour
{
    // This cube parameters.
    private int color;
    private int type;
    private new bool enabled;
    private Mesh outlineMesh, colliderMesh, cubeMesh;

    // Default cube settings.
    // Variables used to change the cube on the inspector.
    public string Area = "Location in Level";
    public CubeColor CubeColor = CubeColor.White;
    public CubeType CubeType = CubeType.A;
    public bool Enabled = true;

    // This GameObject components
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    


    // Update the type of cube on variable change.
    // This function is only executed in the editor.
    void OnValidate()
    {
        try
        {
            int newColor = (int)CubeColor;
            int newType = (int)CubeType;
            if (newColor != color)
            {
                if (newColor >= (int) CubeColor.Count)
                {
                    newColor = ((int)CubeColor.Count - 1);
                }
                color = newColor;
                GetComponent<MeshRenderer>().material = CubeManager.Materials[newColor];
            }

            if (newType != type || Enabled != enabled)
            {
                if (newType >= (int) CubeType.Count)
                {
                    newType = ((int)CubeType.Count - 1);
                }

                type = newType;
                cubeMesh = CubeManager.Cubes[(int)CubeType];
                colliderMesh = CubeManager.Colliders[(int)CubeType];
                outlineMesh = CubeManager.Outlines[(int)CubeType];
                enabled = Enabled;
                
                GetComponent<MeshCollider>().sharedMesh = colliderMesh;
                GetComponent<MeshFilter>().mesh = (enabled) ? cubeMesh : outlineMesh;
                GetComponent<MeshCollider>().isTrigger = !enabled;

            }

            if (Area.Length < 1)
            {
                Area = "Location in Level";
            }
            else if (Area.Length > 20)
            {
                Area = Area.Substring(0,20);
            }
            
            gameObject.name = Area +"\t"+ type + " " + ((enabled) ? "Enabled": "Disabled") + "\t" + color;
        }
        catch
        {
            
        }
        
    }

    void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        color = (int)CubeColor;
        type = (int)CubeType;
        enabled = Enabled;

        cubeMesh = CubeManager.Cubes[type];
        colliderMesh = CubeManager.Colliders[type];
        outlineMesh = CubeManager.Outlines[type];

        // Add this cube to the current level.
        GameManager.AddCube(this,color);
    }

    public void Discard()
    {
        enabled = !enabled;

        meshCollider.isTrigger = !enabled;
		meshFilter.mesh = (enabled) ? cubeMesh : outlineMesh;
    }

    // Since the cubes don't collide with objects in the same layer,
    // we can asume that the collider other is going to be the player (for now.)
    void OnTriggerEnter(Collider other)
    {
        GameManager.InputsEnabled[color] = true;
    }
    void OnTriggerStay(Collider other)
    {
        GameManager.InputsEnabled[color] = false;
    }
	void OnTriggerExit(Collider other)
	{
        GameManager.InputsEnabled[color] = true;
    }
}
