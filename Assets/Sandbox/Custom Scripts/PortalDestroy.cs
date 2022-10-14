using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDestroy : MonoBehaviour
{
    MeshRenderer mr;

    private void Start()
    {
        mr = gameObject.GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {

       if(other.gameObject.CompareTag("UFO"))
        {
            mr.enabled = true; 
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("UFO"))
        {
            mr.enabled = false;
            other.gameObject.SetActive(false);
        }  
    }
}
