using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = Vector3.Lerp(gameObject.GetComponent<QUTJr>().pos1, gameObject.GetComponent<QUTJr>().pos2, 1);
    }
}
