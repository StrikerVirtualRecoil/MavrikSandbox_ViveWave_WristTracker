using System.Collections;
using System.Collections.Generic;
using StrikerLink.Unity.Runtime.HapticEngine;
using StrikerLink.Unity.Runtime.Core;
using UnityEngine;

public class BallCollide : MonoBehaviour
{

    public bool smallBall;
    private StrikerDevice mavrik;
    private Blaster blaster;
    public GameObject sbp;
    public HapticEffectAsset smallBallCollide;
    public HapticEffectAsset bigBallCollide;


    void Awake()
    {
        sbp = GameObject.Find("StrikerBlasterPrefab");
        mavrik = sbp.GetComponent<StrikerDevice>();
        blaster = sbp.GetComponent<Blaster>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tennis")
        {
            //Debug.Log("it's a collision");

            if (!blaster.ballShots && !blaster.ballReload && !blaster.ballRotary)
            {
                Debug.Log("all clear for ball collide");
                if (smallBall)
                {
                    mavrik.FireHaptic(smallBallCollide);
                }
                else
                {
                    mavrik.FireHaptic(bigBallCollide);
                }
            }
        }
    }

   

 
}
