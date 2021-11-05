using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QUTJr : MonoBehaviour
{
    public GameObject child;
    public GameObject control;
    public Mesh mesh;
    public Material material;

    public Vector3 jointLocation;
    public Vector3 jointOffset;

    public float angle;
    public float lastAngle;
    public Vector3[] limbVertexLocations;

    //set movement speed
    //public float speed = 1f;

    
    public Vector3 offset;
    private bool goRight = true;
    private bool goUp = true;
    private bool move = true;
    private bool jump = true;
    private bool jumpForward = false;
    private bool up = true;
    public bool nod = true;
    public Vector3 currentPos;
    public float distanceCovered = 0;
    public bool collapse = false;
    float currentAngle = 0;
    float previousAngle;

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
            if (nod == true)
            {
                if (gameObject.tag == "upperarm")
                {
                    angle = control.GetComponent<controller>().value;
                }
            }
            if (gameObject.tag == "base")
            {
                angle = control.GetComponent<Walking>().value;
            }
            
        }

        WalkBase();

        

        //key input controls
        if (Input.GetKeyDown("a"))
        {
            //go left
            move = true;
            goRight = false;
        }
        else if (Input.GetKeyDown("d"))
        {
            //go right
            move = true;
            goRight = true;
        }
        else if (Input.GetKeyDown("w"))
        {
            //jumps on spot
            move = false;
            jump = true;
        }
        else if (Input.GetKeyDown("s"))
        {
            //jumps forward
            jumpForward = true;
            move = false;
            jump = false;
            
        }
        else if (Input.GetKeyDown("z"))
        {
            //stop moving
            move = false;
            jump = false;
            nod = false;
            //collapse = true;
            bool forwardFall = true;

            if (forwardFall == true)
            {
                previousAngle = currentAngle + 0.01f;
                if (gameObject.tag == "lowerarm")
                {
                    RotateAroundPoint(currentPos, currentAngle, previousAngle);
                    if (child != null)
                    {
                        //child.GetComponent<QUTJr>().RotateAroundPoint(jointLocation, currentAngle - 0.3f, previousAngle + 0.3f);                       
                    }
                }
                if (gameObject.tag == "upperarm")
                {
                    RotateAroundPoint(jointLocation, currentAngle - 0.3f, previousAngle + 0.3f);
                    if (child != null)
                    {
                        child.GetComponent<QUTJr>().RotateAroundPoint(jointLocation, currentAngle - 0.3f, previousAngle + 0.3f);
                    }
                }
                currentAngle -= 0.01f;
            }
            

            if (collapse == true)
            {
                
                if (forwardFall == false)
                {
                    previousAngle = currentAngle;
                    if (gameObject.tag == "lowerarm")
                    {
                        RotateAroundPoint(currentPos, currentAngle, previousAngle);
                        currentAngle -= 0.01f;
                    }
                }
                if (currentAngle <= 0.5f)
                {
                    forwardFall = true;
                }
                if (currentAngle > 0.5f)
                {
                    forwardFall = false;
                    /*
                    if (currentAngle < 0)
                    {
                        collapse = false;
                        move = true;
                        jump = true;
                    }*/
                }
            }
        }

        if (Input.GetKeyUp("w") && jumpForward == false && collapse == false)
        {
            //returns QUT Jr continually walking left and right
            move = true;
        }

        //controls jumping forward
        if (jumpForward == true)
        {
            Jump();
        }

        //controls collapse
        if (collapse == true)
        {
           
        }

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
        //walking
        if (move == true)
        {
            //walks to the right
            if (goRight == true)
            {
                if (gameObject.tag == "base")
                {
                    pos = new Vector3(0.01f, 0f, 1f);
                    MoveByOffSet(pos);
                }
                currentPos.x += 0.01f;
            }
            //walks to the left
            if (goRight == false)
            {
                if (gameObject.tag == "base")
                {
                    pos = new Vector3(-0.01f, 0f, 1f);
                    MoveByOffSet(pos);
                }
                currentPos.x -= 0.01f;
            }
        }

        //jumps
        if (jump == true)
        {
            if (goUp == true)
            {
                if (gameObject.tag == "base")
                {
                    pos = new Vector3(0f, 0.01f, 1f);
                    MoveByOffSet(pos);
                }
                currentPos.y += 0.01f;
            }
            //falls
            if (goUp == false)
            {
                if (gameObject.tag == "base")
                {
                    pos = new Vector3(0f, -0.01f, 1f);
                    MoveByOffSet(pos);
                }
                currentPos.y -= 0.01f;
            }

            //controls jump of QUT Jr
            if (currentPos.y >= 0.5)
            {
                goUp = false;
            }
            if (currentPos.y <= 0)
            {
                goUp = true;
            }
        }

        //controls direction of QUT Jr walking
        if (currentPos.x >= 15)
        {
            goRight = false;
        }

        if (currentPos.x <= 2)
        {
            goRight = true;
        }

    }

    private void Jump()
    {
        Vector3 offsetPos;
        if (jumpForward == true)
        {
            if (goRight == true)
            {
                if (up == true)
                {
                    offsetPos = new Vector3(0.08f, 0.2f, 1f);
                    if (gameObject.tag == "base")
                    {
                        MoveByOffSet(offsetPos);
                    }
                    currentPos.y += 0.2f;
                    currentPos.x += 0.08f;
                    distanceCovered += 0.08f;
                }
                if (up == false)
                {
                    offsetPos = new Vector3(0.08f, -0.2f, 1f);
                    if (gameObject.tag == "base")
                    {
                        MoveByOffSet(offsetPos);
                    }
                    currentPos.y -= 0.2f;
                    currentPos.x += 0.08f;
                    distanceCovered += 0.08f;
                }
            }
            if (goRight == false)
            {
                if (up == true)
                {
                    offsetPos = new Vector3(-0.08f, 0.2f, 1f);
                    if (gameObject.tag == "base")
                    {
                        MoveByOffSet(offsetPos);
                    }
                    currentPos.y += 0.2f;
                    currentPos.x -= 0.08f;
                    distanceCovered -= 0.08f;
                }
                if (up == false)
                {
                    offsetPos = new Vector3(-0.08f, -0.2f, 1f);
                    if (gameObject.tag == "base")
                    {
                        MoveByOffSet(offsetPos);
                    }
                    currentPos.y -= 0.2f;
                    currentPos.x -= 0.08f;
                    distanceCovered -= 0.08f;
                }

            }
        }
            

        if (goRight == true)
        {
            if (currentPos.y <= 0)
            {
                up = true;
            }
            if (currentPos.y >= 4)
            {
                up = false;
            }
            if (distanceCovered >= 1.6)
            {
                jumpForward = false;
                move = true;
                jump = true;
            }
        }
        if (goRight == false)
        {
            if(currentPos.y <= 0)
                    {
                up = true;
            }
            if (currentPos.y >= 4)
            {
                up = false;
            }
            if (distanceCovered <= -1.6)
            {
                jumpForward = false;
                move = true;
                jump = true;
            }
        }
        
    }

    private void Collapse()
    {
        float currentAngle = 0;
        float previousAngle;
        
        bool forwardFall = true;
        
        if (collapse == true)
        {
            if (forwardFall == true)
            {
                previousAngle = currentAngle;
                if (gameObject.tag == "lowerarm")
                {
                    RotateAroundPoint(currentPos, currentAngle, previousAngle);
                    currentAngle += 0.01f;
                }
            }
            if (forwardFall == false)
            {
                previousAngle = currentAngle;
                if (gameObject.tag == "lowerarm")
                {
                    RotateAroundPoint(currentPos, currentAngle, previousAngle);
                    currentAngle -= 0.01f;
                }
            }
            if (currentAngle <= 0.5f)
            {
                forwardFall = true;
            }
            if (currentAngle > 0.5f)
            {
                forwardFall = false;
                if (currentAngle < 0)
                {
                    collapse = false;
                    move = true;
                    jump = true;
                }
            }
        }
        
        
    }

    private void DrawLimb()
    {
        //Add a mesh filter and mesh renderer to empty gameobject
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        //get the mesh from the mesh filter
        mesh = GetComponent<MeshFilter>().mesh;

        //set the material
        GetComponent<MeshRenderer>().material = material;

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
