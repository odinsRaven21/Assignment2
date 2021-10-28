using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QUTJr : MonoBehaviour
{
    public GameObject child;
    public Mesh mesh;

    public Vector3 jointLocation;
    public Vector3 jointOffset;

    public float angle;
    public float lastAngle;
    public Vector3[] limbVertexLocations;

    //set movement speed
    public float speed = 1f;

    //set positions to move between
    private Vector3 pos1 = new Vector3(-9,0,0);
    private Vector3 pos2;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        DrawLimb();

        //MoveByOffSet(pos1);
        //set the positions
        //pos1 = new Vector3(-9, 0, 0);
        //pos2 = new Vector3(9, 0, 0);


        //move the child to the joint location
        /*
        if (child != null)
        {
            child.GetComponent<QUTJr>().MoveByOffSet(jointOffset);
        }*/

        //MoveByOffSet(jointOffset);

        
    }

    // Update is called once per frame
    void Update()
    {
        //recalculate the bounds of the mesh
        mesh.RecalculateBounds();

        //Walking(pos1);
    }

    private void DrawLimb()
    {
        //Add a mesh filter and mesh renderer to empty gameobject
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        //get the mesh from the mesh filter
        mesh = GetComponent<MeshFilter>().mesh;

        //clear all vertex and index data from the mesh
        mesh.Clear();

        //create rectangle 
        mesh.vertices = new Vector3[]
        {
            limbVertexLocations[0],
            limbVertexLocations[1],
            limbVertexLocations[2],
            limbVertexLocations[3]
        };

        mesh.colors = new Color[]
        {
            new Color(0.8f, 0.3f, 0.3f, 1.0f),
            new Color(0.8f, 0.3f, 0.3f, 1.0f),
            new Color(0.8f, 0.3f, 0.3f, 1.0f),
            new Color(0.8f, 0.3f, 0.3f, 1.0f)
        };

        //set vertex indices
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    }
    /*
    public void MoveByOffSet(Vector3 offset)
    {
        //get the vertices from the matrix
        Vector3[] vertices = mesh.vertices;

        //Get the translation matrix
        Matrix3x3 T = gameObject.GetComponent<Transform>().Translate(offset);

        //transform each point in the mesh to it's new position
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = T.MultiplyPoint(vertices[i]);
        }

        //set the vertices in the mesh to their new position
        mesh.vertices = vertices;

        jointLocation = T.MultiplyPoint(jointLocation);

        if (child != null)
        {
            child.GetComponent<QUTJr>().MoveByOffSet(offset);
        }
    }*/
    /*
    public void Walking(Vector3 pos)
    {
        Matrix3x3 T1 = gameObject.GetComponent<Transform>().Translate(-pos);

        Matrix3x3 T2 = gameObject.GetComponent<Transform>().Translate(pos);

        Matrix3x3 M = T1 * T2 * Time.deltaTime;

        Vector3[] vertices = mesh.vertices;

        //Get the translation matrix
        //Matrix3x3 T = gameObject.GetComponent<Transform>().Translate(offset);

        //apply to all the vertices in mesh
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = M.MultiplyPoint(vertices[i]);
        }

        //set the vertices in the mesh to their new position
        mesh.vertices = vertices;

    }*/
}
