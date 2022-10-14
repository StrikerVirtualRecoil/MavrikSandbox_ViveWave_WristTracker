using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to a GameObject to rotate around the target position.
public class Orbit : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject target;
    public float orbrate;

    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;

    void Update()
    {
        if (rotateX)
        {
            // Spin the object around the target on the Y axis
            transform.RotateAround(target.transform.position, Vector3.right, orbrate * Time.deltaTime);
        }
        if (rotateY) 
        {
            // Spin the object around the target on the Y axis
            transform.RotateAround(target.transform.position, Vector3.up, orbrate * Time.deltaTime);
        }
        if (rotateZ)
        {
            // Spin the object around the target on the Y axis
            transform.RotateAround(target.transform.position, Vector3.forward, orbrate * Time.deltaTime);
        }
    }
}