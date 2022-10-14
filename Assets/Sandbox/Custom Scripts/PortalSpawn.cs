using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    MeshRenderer mr;
    Transform body;
    Transform cockPit;
    Transform emitter;

    public GameObject portalExit;

    private void Start()
    {
        mr = gameObject.GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {

       if(other.gameObject.CompareTag("UFO"))
        {
            mr.enabled = true;

            body = other.gameObject.transform.GetChild(0);
            body.gameObject.SetActive(true);

            cockPit = other.gameObject.transform.GetChild(1);
            cockPit.gameObject.SetActive(true);

            emitter = other.gameObject.transform.GetChild(3);
            emitter.gameObject.SetActive(true);

            StartCoroutine("SpawnExit");
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
     
        if (other.gameObject.CompareTag("UFO"))
        {
            mr.enabled = false;
        }

    }

    public IEnumerator SpawnExit()
    {
        yield return new WaitForSeconds(30f);
        portalExit.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
