using System.Collections;
using System.Collections.Generic;
using StrikerLink.Unity.Runtime.HapticEngine;
using StrikerLink.Unity.Runtime.Core;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public StrikerDevice mavrik;

    public Blaster blaster;

    public Transform firePoint;

    Rigidbody rb;

    public RaycastHit grapplePoint;

    bool isGrappling = false;

    float distance;

    public float grappleSpeed = 5f;

    public ParticleSystem particles;
    public GameObject particleParent;

    public HapticEffectAsset gravityPull, gravityPush;

    public int particleSpeed = 10;
    public bool flip;

    void Awake()
    {

        particles.Stop();
        flip = false;
    }

    public void DoGrappling()
    {
        if (isGrappling)
        {
            //Get Vector between player and grappling point
            Vector3 grappleDirection = (grapplePoint.point - transform.position);

            // With this you can determine if you are overshooting your target
            if (rb != null && distance < grappleDirection.magnitude)
            {
                rb.gameObject.transform.SetParent(mavrik.transform);
            }
            else
                //Calculate distance between player and grappling point
                distance = grappleDirection.magnitude;
        }
    }

    public void GrapplingTriggerDown()
    {

        // Check if a button is pressed and if the Raycast hits something
        if (mavrik.GetTriggerDown() && Physics.Raycast(firePoint.position, firePoint.forward, out grapplePoint))
        {
            isGrappling = true;

            particles.Play();


            Vector3 grappleDirection = (transform.position - grapplePoint.point);


            rb = grapplePoint.rigidbody;

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = grappleDirection.normalized * grappleSpeed;
                mavrik.FireHaptic(gravityPull);
                InvokeRepeating("GravityPull", 1f, 1f);
            }

        }
    }

    public void GrapplingTriggerUp()
    {
        //turn grappling mode off when the button is released
        if (rb != null && mavrik.GetTriggerUp())
        {
            Debug.Log("this is my parent: " + rb.gameObject.transform.parent);

            isGrappling = false;

            particles.Stop();
            CancelInvoke("GravityPull");

            rb.isKinematic = false;
            rb.gameObject.transform.SetParent(null);
            mavrik.FireHaptic(gravityPush);
            rb.gameObject.GetComponent<Rigidbody>().AddRelativeForce(firePoint.forward * 5000);

        }
        else if(rb == null)
        {
            CancelInvoke("GravityPull");
        }

    }

    public void GravityPull()
    { 
        mavrik.FireHaptic(gravityPull);
    }
}
