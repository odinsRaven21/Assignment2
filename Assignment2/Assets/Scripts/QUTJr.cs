using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QUTJr : MonoBehaviour
{
    public GameObject child;
    public GameObject control;
    public Mesh mesh;

    public Vector3 jointLocation;
    public Vector3 jointOffset;

    public float angle;
    public float lastAngle;
    public Vector3[] limbVertexLocations;

    //set movement speed
    public float speed = 1f;

    //set positions to move between
    public Vector3 pos1;
    public Vector3 pos2;
    private Vector3 curPosition;

    public float xleft = -1;
    public float xright = 5;
    
    public Vector3 offset;
    public bool goRight = true;
    public float currentPos;

    void Awake()
    {
        DrawLimb();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (child != null)
        {
            child.GetComponent<QUTJr>().MoveByOffSet(jointOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        lastAngle = angle;
        if (control != null)
        {
            if (gameObject.tag == "upperarm") 
            {
                angle = control.GetComponent<controller>().value;
            }
            if (gameObject.tag == "base")
            {
                angle = control.GetComponent<Walking>().value;
            }
            
        }


        WalkBase();

        if (child != null)
        {
            child.GetComponent<QUTJr>().RotateAroundPoint(jointLocation, angle, lastAngle);

        }


        //recalculate the bounds of the mesh
        mesh.RecalculateBounds();
        
    }

    private void WalkBase()
    {
        Vector3 pos;
        if (goRight == true)
        {
            if (gameObject.tag == "base")
            {
                pos = new Vector3(0.01f, 0f, 1f);
                MoveByOffSet(pos);
            }
            currentPos += 0.01f;
        }
        if (goRight == false)
        {
            if (gameObject.tag == "base")
            {
                pos = new Vector3(-0.01f, 0f, 1f);
                MoveByOffSet(pos);
            }
            currentPos -= 0.01f;
        }
        if (currentPos >= 15)
        {
            goRight = false;
            //gameObject.GetComponent<Transforms>().Scale(-1);
        }

        if (currentPos <= 2)
        {
            goRight = true;
        }

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

    public void MoveByOffSet(Vector3 offset)
    {
        //Get the translation matrix
        Matrix3x3 T = gameObject.GetComponent<Transforms>().Translate(offset);

        //transform each point in the mesh to it's new position
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = T.MultiplyPoint(vertices[i]);
        }

        //set the vertices in the mesh to their new position
        mesh.vertices = vertices;

        //apply transformation to joint
        jointLocation = T.MultiplyPoint(jointLocation);

        //apply transformation to children
        if (child != null)
        {
            child.GetComponent<QUTJr>().MoveByOffSet(offset);
        }
    }

    /*public void Walking(Vector3 pos1, Vector3 pos2)
    {
        Matrix3x3 T1 = gameObject.GetComponent<Transforms>().Translate(-pos1);

        Matrix3x3 T2 = gameObject.GetComponent<Transforms>().Translate(pos2);

        Matrix3x3 M = T1 * T2;

        Vector3[] vertices = mesh.vertices;

        //Get the translation matrix
        //Matrix3x3 T = gameObject.GetComponent<Transforms>().Translate(offset);

        Vector3[] Vertices = mesh.vertices;

        //apply to all the vertices in mesh
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = M.MultiplyPoint(vertices[i]);
        }

        //set the vertices in the mesh to their new position
        mesh.vertices = vertices;


        pos1 = M.MultiplyPoint(pos1);

        if (child != null)
        {
            child.GetComponent<QUTJr>().Walking(pos1, pos2);
        }

        
    }*/

    public void RotateAroundPoint(Vector3 point, float angle, float lastAngle)
    {
        // Move the point to the origin
        Matrix3x3 T1 = GetComponent<Transforms>().Translate(-point);
        // Undo the last rotation
        Matrix3x3 R1 = GetComponent<Transforms>().Rotate(-lastAngle);
        // Move the point back to the oritinal position
        Matrix3x3 T2 = GetComponent<Transforms>().Translate(point);
        // Perform the new rotation
        Matrix3x3 R2 = GetComponent<Transforms>().Rotate(angle);
        // The final translation matrix
        Matrix3x3 M = T2 * R2 * R1 * T1;
        // Move the mesh
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = M.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
        // Apply the transformation to the joint
        jointLocation = M.MultiplyPoint(jointLocation);
        // Apply the transformation to the children
        if (child != null)
        {
            child.GetComponent<QUTJr>().RotateAroundPoint(point, angle, lastAngle);
        }
    }
}
