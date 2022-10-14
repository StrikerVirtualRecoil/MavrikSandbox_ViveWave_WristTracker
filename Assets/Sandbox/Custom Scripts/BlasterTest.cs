using System.Collections;
using System.Collections.Generic;
using StrikerLink.Unity;
using StrikerLink.Shared.Devices.DeviceFeatures;
using StrikerLink.Unity.Runtime.HapticEngine;
using StrikerLink.Unity.Runtime.Core;
using UnityEngine;


public class BlasterTest : MonoBehaviour
{
    [Header("Shot Settings")]

    public StrikerDevice mavrik;

    public HapticEffectAsset laser_shot;
  
    public AudioSource laser;
 
    private float rate = 10f;

    private IEnumerator fireCo;
    
    public DeviceButton sideRight;
    public DeviceButton sideLeft;
    public DeviceSensor gripReload;

    public int fireMode;
    public int fireModeMax;


    // Start is called before the first frame update
    void Start()
    {
        sideRight = DeviceButton.SideRight;
        sideLeft = DeviceButton.SideLeft;

        fireMode = 0;
        fireModeMax = 1;

        fireCo = RapidFire();

        InvokeRepeating("FiringHandler", 2.0f, 2.0f);
        InvokeRepeating("FiringKiller", 4.0f, 2.0f);
        InvokeRepeating("FiringKiller", 4.0f, 2.1f);
    }

    // Update is called once per frame
    void Update()
    {
        FireMode();
        //FiringHandler();
        if (fireMode == 0)
        {
            StopAllCoroutines();
        }
    }

    public void Shoot()
    {
        if(fireMode == 1)
        {
            mavrik.FireHaptic(laser_shot);
        }

    }

    IEnumerator RapidFire()
    {
       // yield return new WaitForSeconds(2);

        while (true)
        {
            Shoot();
            //shotCount++;
            yield return new WaitForSeconds(1 / rate);
        }
    }

   

    void FiringHandler()
    {
        
        if (fireMode == 1 )//&& shotCount < 30)
        {

            StartCoroutine(fireCo);
            if (fireCo == null)
            {
                fireCo = RapidFire();
            }
        }
        
    }

    void FiringKiller()
    {
        StopAllCoroutines();
        /*StopCoroutine(fireCo);
        
        */
        fireCo = RapidFire();
    }



    void FireMode()
    {
        if (mavrik.GetButtonDown(sideRight))
        {
            if (fireMode < fireModeMax)
            {
                fireMode++;
            }
        }

        if (mavrik.GetButtonDown(sideLeft))
        {
            if (fireMode > 0)
            {
                fireMode--;
            }
        }
    }


 
    
}
