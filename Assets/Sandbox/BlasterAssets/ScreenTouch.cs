using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTouch : MonoBehaviour
{
    public ParticleSystem ps = null;
    public bool circles = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TouchPad(.5F,.5F);
    }

    private void TouchPad(float x = .5F, float y = .5F)
    {
        if (ps != null)
        {
            if(circles)
            {
                var rend = ps.GetComponent<ParticleSystemRenderer>();
                rend.material.SetFloat("TouchPosX", x);
                rend.material.SetFloat("TouchPosY", y);
            }
            else
            {
                var rend = ps.GetComponent<ParticleSystemRenderer>();
                rend.material.SetFloat("TouchPosY", x);
            }
        }
    }
}
