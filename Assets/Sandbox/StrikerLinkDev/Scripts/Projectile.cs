using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject ExplosionPrefab = null;
    
    //private Rigidbody rigid = null;
    public bool dontDestroy;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigid != null)
       {
            rigid.AddRelativeForce(new Vector3(0, 0, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Blaster")
        {
            GameObject explosion = Instantiate(ExplosionPrefab, transform.position, new Quaternion(0, 0, 0, 0));

            if (!dontDestroy)
            {
                Destroy(gameObject);
            }
            Destroy(explosion);
            //Debug.Log("13242151");
        }
    }
}

