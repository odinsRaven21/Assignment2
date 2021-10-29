using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public float value;
    //public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        value = Mathf.PingPong(Time.time, 1);

        //position = Vector3.Lerp(gameObject.GetComponent<QUTJr>().pos1, gameObject.GetComponent<QUTJr>().pos2, 1);
    }
}
