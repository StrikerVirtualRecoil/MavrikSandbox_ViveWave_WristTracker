using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSaucer : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform target;

    public int rotation;
    public int speed;
    public int wait;
    public int torque;

    public float repairTime;

    public MeshRenderer cockpit;

    public Material greenGlass;
    public Material redGlass;

    public ParticleSystem explosion;
    public ParticleSystem smoke;
    public ParticleSystem sparks;
    public ParticleSystem antiGravity;

    public AudioSource ufo;
    public AudioSource ufoHit;
    public AudioSource ufoSparks;

    private bool hasTarget;
    private int count;
    private float timer;
    private bool broken;

    void Start()
    {
        this.transform.position = this.startPoint.position;
        this.cockpit.material = greenGlass;
        this.speed = 20;
        this.broken = false;
        this.smoke.Stop();
        this.sparks.Stop();
        this.antiGravity.Stop();
        this.count = 0;
        this.wait = Random.Range(1500, 3000);
    }

    // Update is called once per frame
    void Update()
    {

        if (this.broken && (this.timer < repairTime)) // if this saucer is broken and timer is less than repairTime, increment timer with the current time value
        {
            this.timer += Time.deltaTime;
        }
        else if (this.broken && (this.timer > repairTime)) // if this saucer is broken and timer is over repairTime, repair the saucer.
        {
            IsFixed();
        }

       

        // When a UFO is spawned, it begins moving toward the end point
        if (Vector3.Distance(endPoint.position, this.transform.position) > 0.001)
        {
            Patrol();
        }
        else
        {
            if (!hasTarget)
            {
                // Once the end point is reached, it begins emitting the beam and grabs the cow
                if (!antiGravity.isPlaying)
                {
                    antiGravity.Play();
                }
                hasTarget = true;
                count = 0;
                target.gameObject.GetComponent<Rigidbody>().useGravity = false;
                target.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0, 2), Random.Range(0, 2), 1) * torque);
            }
            else // Once the beam is fully emitted and cow is grabbed, it begins moving to the moon
            {
                if (count == wait)
                {
                    target.SetParent(this.transform);
                    ReturnHome();
                    count++;
                }
                else if (count < wait)
                {
                    target.transform.position = Vector3.MoveTowards(target.transform.position, this.transform.position - new Vector3(0, 8f, 0), Time.deltaTime);
                    //Rotate the UFO
                    target.transform.Rotate(0.0f, rotation, 0.0f, Space.Self);
                    count++;
                }
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Tennis") || collision.gameObject.CompareTag("Plasma") || collision.gameObject.CompareTag("Plunger"))
        {
            IsHit();
        }
    }

    private void IsHit()
    {
        this.explosion.Play();
        this.cockpit.material = redGlass;
        this.broken = true;
        this.timer = 0.0f;
        this.smoke.Play();
        this.sparks.Play();
        this.ufo.Stop();
        this.ufoHit.Play();
        this.ufoSparks.Play();
        this.antiGravity.Stop();
        target.SetParent(null);
        target.gameObject.GetComponent<Rigidbody>().useGravity = true;
        ReturnHome();
        //Debug.Log($"{this.name} was hit by a blaser bolt");
    }

    private void IsFixed()
    {
        this.cockpit.material = greenGlass;
        this.broken = false;
        this.explosion.Stop();
        this.smoke.Stop();
        this.sparks.Stop();
        this.ufo.Play();
        this.ufoHit.Stop();
        this.ufoSparks.Stop();
        ReturnHome();
        //Debug.Log($"{this.name} has repaired itself");
    }

    private void Patrol()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.endPoint.position, speed * Time.deltaTime);
    }

    private void ReturnHome()
    {
        Transform temp = this.startPoint;
        this.startPoint = this.endPoint;
        this.endPoint = temp;
    }
}

