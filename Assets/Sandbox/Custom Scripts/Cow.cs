using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour
{
    public bool taken;
    public Transform point;
    public SphereCollider abductor;

    // Start is called before the first frame update
    void Start()
    {
        taken = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(taken)
        {
            // if the cow has been grabbed by a UFO
        }
    }

}
