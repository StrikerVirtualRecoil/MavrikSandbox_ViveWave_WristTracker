using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rings : MonoBehaviour
{
    public Material mat;
    private MeshRenderer mesh;
    public AudioSource goal;


    // Start is called before the first frame update
    void Start()
    {
        mat = Resources.Load<Material>("GreenGlass");
        mesh = gameObject.GetComponent<MeshRenderer>();
        Material blueMat = mesh.material; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "Pong" && other.gameObject.GetComponent<TennisPong>().bounced == true)
        {
            mesh.material = mat;
            goal.Play();
        }
        else if(gameObject.tag == "Ring")
        {
            mesh.material = mat;
            goal.Play();
        }


        
    }
}
