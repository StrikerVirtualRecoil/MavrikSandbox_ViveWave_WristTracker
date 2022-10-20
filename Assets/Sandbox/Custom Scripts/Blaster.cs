using System.Collections;
using System.Collections.Generic;
using StrikerLink.Unity;
using StrikerLink.Shared.Devices.DeviceFeatures;
using StrikerLink.Unity.Runtime.Core;
using StrikerLink.Unity.Runtime.HapticEngine;
using TMPro;
using UnityEngine;
using Script;

public class Blaster : MonoBehaviour
{
    [Header("Shot Settings")]
    public Transform projectileLaser;
    //public Transform projectileTennis;
    public Transform projectilePlasma1;
    public Transform projectilePlasma2;
    public Transform projectilePlasma3;
    public Transform projectilePlasma4;
    public Transform projectilePlasma5;
    public Transform plasmaField;

    public Crossbow crossbow;
    public GameObject crossbowParent;
    public GameObject mavrikClassicParent;

    public GunController ballzooka;
    public GameObject ballzookaParent;

    public ParticleSystem flash;
    private ParticleSystem mavrikBullet;
    private ParticleSystemRenderer psr;
    private ParticleSystem.MainModule mainLaser;

    private ParticleSystem mavrikPlasma;
    //private ParticleSystem.MainModule mainPlasma;
    public GameObject plasmaParent;
    private float oscVal;

    public Transform firePoint;

    public float range = 100f;

    public RaycastHit hit;

    public Grappling grappling;

    public StrikerDevice mavrik;

    public Haptics Haptics;
    public HapticEffectAsset mavrikClassicShot;

    public SoundEffects sfx;

    public AudioSource laser;
    public AudioSource tennis;
    public AudioSource crossbow_rope_sfx;
   
    
    private float rate = 10f;

    private IEnumerator fireCo;

    private DeviceButton sideRight, sideLeft;
    public DeviceButton leftTouch;

    [HideInInspector]
    public DeviceSensor gripReload;

    private DeviceSensor slideReload;

    [HideInInspector]
    public DeviceSensor ballzookaReload;

    private DeviceRawSensor forwardBar1, forwardBar2, forwardBar3, forwardBar4, forwardBar5, forwardBar6, forwardBar7, forwardBar8, forwardBar9;
    
    [HideInInspector]
    public DeviceRawSensor slideBar1, slideBar2, slideBar3, slideBar4, slideBar5, slideBar6, slideBar7, slideBar8, slideBar9, slideBar10, slideBar11, slideBar12;

    public int fireMode;
    public int fireModeMax;
    public int shotMax;
    public int shotCount;

    private int plasmaSize;

    public bool ballShots, ballReload, ballRotary;

    public TextMeshProUGUI tmp;

    public bool noRope;
    // Start is called before the first frame update
    void Start()
    {
        sideRight = DeviceButton.SideRight;
        sideLeft = DeviceButton.SideLeft;
        leftTouch = DeviceButton.TouchpadLeft;

        gripReload = DeviceSensor.ReloadTouched;
        slideReload = DeviceSensor.SlideTouched;
        ballzookaReload = DeviceSensor.UnderTouchpadGripTouched;

        forwardBar1 = DeviceRawSensor.ForwardBarGrip00;
        forwardBar2 = DeviceRawSensor.ForwardBarGrip01;
        forwardBar3 = DeviceRawSensor.ForwardBarGrip02;
        forwardBar4 = DeviceRawSensor.ForwardBarGrip03;
        forwardBar5 = DeviceRawSensor.ForwardBarGrip04;
        forwardBar6 = DeviceRawSensor.ForwardBarGrip05;
        forwardBar7 = DeviceRawSensor.ForwardBarGrip06;
        forwardBar8 = DeviceRawSensor.ForwardBarGrip07;
        forwardBar9 = DeviceRawSensor.ForwardBarGrip08;

        slideBar1 = DeviceRawSensor.Slide00;
        slideBar2 = DeviceRawSensor.Slide01;
        slideBar3 = DeviceRawSensor.Slide02;
        slideBar4 = DeviceRawSensor.Slide03;
        slideBar5 = DeviceRawSensor.Slide04;
        slideBar6 = DeviceRawSensor.Slide05;
        slideBar7 = DeviceRawSensor.Slide06;
        slideBar8 = DeviceRawSensor.Slide07;
        slideBar9 = DeviceRawSensor.Slide08;
        slideBar10 = DeviceRawSensor.Slide09;
        slideBar11 = DeviceRawSensor.Slide10;
        slideBar12 = DeviceRawSensor.Slide11;

        fireMode = 0;
        fireModeMax = 5;

        shotMax = 30;
        shotCount = 0;

        //fireCo = RapidFire();

        mavrikBullet = projectileLaser.GetChild(0).GetComponent<ParticleSystem>();
        psr = projectileLaser.GetChild(0).GetComponent<ParticleSystemRenderer>();
        mainLaser = mavrikBullet.main;

        mavrikPlasma = plasmaField.GetChild(0).GetComponent<ParticleSystem>();

        StartCoroutine(FadeTMP());

        ballShots = false;

    }

    // Update is called once per frame
    void Update()
    {
        FireMode();
        FiringHandler();
        Reload();
        ForwardBarSensorsTouched();
        SlideSensorsTouched();  
    }

    public void Shoot()
    {

        if (fireMode == 0 || fireMode == 1)
        {
            if(fireMode == 0)
            {
                psr.material = Resources.Load<Material>("M_Led");
            }
            else if(fireMode == 1)
            {
                psr.material = Resources.Load<Material>("M_Led 1");
            }

            Instantiate(projectileLaser, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 1000f));
            Instantiate(flash, firePoint);
            laser.Play(0);
            mavrik.FireHaptic(mavrikClassicShot);
        }
        else if (fireMode == 3)
        {
            Instantiate(flash, firePoint);
        }
        else if (fireMode == 5)
        {
            switch (plasmaSize)
            {
                case 1:
                    Instantiate(projectilePlasma1, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 167f));
                    mavrik.FireHaptic(Haptics.charge_shot1);
                    sfx.plasmaBlast1.Play(0);
                    sfx.plasmaCharge1.Stop();
                    break;

                case 2:
                    Instantiate(projectilePlasma2, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 625f));
                    mavrik.FireHaptic(Haptics.charge_shot2);
                    sfx.plasmaBlast2.Play(0);
                    sfx.plasmaCharge2.Stop();
                    break;

                case 3:
                    Instantiate(projectilePlasma3, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 1250f));
                    mavrik.FireHaptic(Haptics.charge_shot3);
                    sfx.plasmaBlast3.Play(0);
                    sfx.plasmaCharge3.Stop();
                    break;

                case 4:
                    Instantiate(projectilePlasma4, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2500f));
                    mavrik.FireHaptic(Haptics.charge_shot4);
                    sfx.plasmaBlast4.Play(0);
                    sfx.plasmaCharge4.Stop();
                    break;

                case 5:
                    Instantiate(projectilePlasma5, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 5000f));
                    mavrik.FireHaptic(Haptics.charge_shot5);
                    sfx.plasmaBlast5.Play(0);
                    sfx.plasmaCharge5.Stop();
                    break;
            }
        }
    }

    /*IEnumerator RapidFire()
    {
        while (fireMode == 1)
        {
            Shoot();
            yield return new WaitForSeconds(1 / rate);
        }
    }*/

    void FiringHandler()
    {

        if (mavrik.GetTriggerDown())
        {

            if ((fireMode == 0) && shotCount < 30)
            {
                Shoot();
            }
            else if (fireMode == 1 && shotCount < 30)
            {
                InvokeRepeating("Shoot", 0f, 1 / rate);
                /*StartCoroutine(fireCo);
                if (fireCo == null)
                {
                    fireCo = RapidFire();
                }*/
            }
            else if (fireMode == 2)
            {
                //ballShots = true;
            }
            else if (fireMode == 3)
            {
                grappling.GrapplingTriggerDown();
            }
            else if (fireMode == 4)
            {
                crossbow.Shoot();   
            }
            else if (fireMode == 5)
            {
                var plasmaVelocity = mavrikPlasma.velocityOverLifetime;
                plasmaVelocity.speedModifierMultiplier = 0.1f;

                var plasmaRadius = mavrikPlasma.shape;
                plasmaRadius.radius = .05f;
                Charging();
            }
        }


        else if (mavrik.GetTriggerUp())
        {
            if (fireMode == 1)
            {
                CancelInvoke("Shoot");
               /* StopCoroutine(fireCo);
                fireCo = RapidFire(); */
            }

            if (fireMode == 2)
            {
                ballShots = false;
                ballRotary = false;
            }

            //Debug.Log("Trigger Up");

            if (fireMode == 3)
            {
                grappling.GrapplingTriggerUp();
            }

            if (fireMode == 5)
            {
                Debug.Log("plasma blast size = " + plasmaSize);
                Shoot();

                var plasmaVelocity = mavrikPlasma.velocityOverLifetime;
                plasmaVelocity.speedModifierMultiplier = 0.1f;

                var plasmaRadius = mavrikPlasma.shape;
                plasmaRadius.radius = .05f;

                CancelInvoke("Charge6");
                StopAllCoroutines();
                plasmaSize = 1;
            }
        }

        if (shotCount >= shotMax)
        {
            StopAllCoroutines();
            //Debug.Log("Out of Ammo");
        }

        grappling.DoGrappling();
    }



    void FireMode()
    {
        if (mavrik.GetButtonDown(sideRight) || Input.GetKeyDown("right"))
        {
            if (fireMode < fireModeMax)
            {
                fireMode++;

                mavrik.FireHaptic(Haptics.blaster_swap);
                StartCoroutine(FadeTMP());
            } 
        }

        if (mavrik.GetButtonDown(sideLeft) || Input.GetKeyDown("left"))
        { 
            if (fireMode > 0)
            {
                fireMode--;

                mavrik.FireHaptic(Haptics.blaster_swap);
                StartCoroutine(FadeTMP());
            }
        }

        if(fireMode == 0)
        {
            //sfx.mavrikBlaster.Play(0);
            mavrikClassicParent.SetActive(true);
            tmp.text = "Mavrik Blaster";
        }

        if (fireMode == 1)
        {
            //sfx.fullAutoBlaster.Play(0);
            mavrikClassicParent.SetActive(true);
            tmp.text = "Full Auto Blaster";
        }


        if (fireMode == 2)
        {
            //sfx.ballzookaBlaster.Play(0);
            ballzookaParent.SetActive(true);
            mavrikClassicParent.SetActive(false);
            tmp.text = "Ballzooka";
        }
        else
        {
            ballzookaParent.SetActive(false);
        }

        if (fireMode == 3)
        {
            //sfx.gravityBlaster.Play(0);
            grappling.particleParent.SetActive(true);
            mavrikClassicParent.SetActive(true);
            tmp.text = "Gravity Blaster";
        }
        else
        {
            grappling.particleParent.SetActive(false);
        }

        if(fireMode == 4)
        {
            crossbowParent.SetActive(true);
            mavrikClassicParent.SetActive(false);
            //sfx.grapplingCrossbow.Play(0);
            /*crossbow._currentArrowInstance = 0;
            tmp.text = "Grappling Crossbow";*/

            crossbow._currentArrowInstance = 1;
            tmp.text = "Explosive Crossbow";
        }
        else
        {
            crossbowParent.SetActive(false);
        }

        if(fireMode == 5)
        {
            //sfx.plasmaBlaster.Play(0);
            plasmaParent.SetActive(true);
            mavrikClassicParent.SetActive(true);
            tmp.text = "Plasma Blaster";
        }
        else
        {
            plasmaParent.SetActive(false);
        }
              
    }


    void Reload()
    {
        if (mavrik.GetSensorDown(gripReload))
        {
            shotCount = 0;
            //mavrik.FireHaptic(laser_reload);
        }

        if (fireMode == 4)
        {
            if (mavrik.GetSensorDown(slideReload)){
                crossbow.StartLoading();
            }
        }
    }

    void Charging()
    {
        
        StartCoroutine(Charge1());
        StartCoroutine(Charge2());
        StartCoroutine(Charge3());
        StartCoroutine(Charge4());
        StartCoroutine(Charge5());
       
            InvokeRepeating("Charge6", 5.0f, 1.0f);
    }

    private IEnumerator FadeTMP()
    {
        tmp.alpha = 255f;
        yield return new WaitForSeconds(3.0f);
        tmp.alpha = 0;
    }

    public IEnumerator Charge1()
    {
        yield return new WaitForSeconds(0.0f);
        var plasmaVelocity = mavrikPlasma.velocityOverLifetime;
        plasmaVelocity.speedModifierMultiplier = Mathf.MoveTowards(plasmaVelocity.speedModifierMultiplier, 1.0f, 1.0f);

        var plasmaShape = mavrikPlasma.shape;
        
        plasmaShape.radius = .05f;

        mavrik.FireHaptic(Haptics.charge1);
        sfx.plasmaCharge1.Play(0);
        plasmaSize = 1;
    }

    public IEnumerator Charge2()
    {
        yield return new WaitForSeconds(1.0f);
        var plasmaVelocity = mavrikPlasma.velocityOverLifetime;
        plasmaVelocity.speedModifierMultiplier = Mathf.MoveTowards(plasmaVelocity.speedModifierMultiplier, 3.0f, 3.0f);

        var plasmaShape = mavrikPlasma.shape;
        
        plasmaShape.radius = .1f;

        mavrik.FireHaptic(Haptics.charge2);
        sfx.plasmaCharge2.Play(0);
        plasmaSize = 2;
    }

    public IEnumerator Charge3()
    {
        yield return new WaitForSeconds(2f);
        var plasmaVelocity = mavrikPlasma.velocityOverLifetime;
        plasmaVelocity.speedModifierMultiplier = Mathf.MoveTowards(plasmaVelocity.speedModifierMultiplier, 9.0f, 9.0f);

        var plasmaShape = mavrikPlasma.shape;
        
        plasmaShape.radius = .15f;

        mavrik.FireHaptic(Haptics.charge3);
        sfx.plasmaCharge3.Play(0);
        plasmaSize = 3;
    }

    public IEnumerator Charge4()
    {
        yield return new WaitForSeconds(3f);
        var plasmaVelocity = mavrikPlasma.velocityOverLifetime;
        plasmaVelocity.speedModifierMultiplier = Mathf.MoveTowards(plasmaVelocity.speedModifierMultiplier, 30.0f, 30.0f);

        var plasmaShape = mavrikPlasma.shape;
        
        plasmaShape.radius = .2f;

        mavrik.FireHaptic(Haptics.charge4);
        sfx.plasmaCharge4.Play(0);
        plasmaSize = 4;
    }

    public IEnumerator Charge5()
    {
        yield return new WaitForSeconds(4f);
        var plasmaVelocity = mavrikPlasma.velocityOverLifetime;
        plasmaVelocity.speedModifierMultiplier = Mathf.MoveTowards(plasmaVelocity.speedModifierMultiplier, 70.0f, 30.0f);

        var plasmaShape = mavrikPlasma.shape;
        plasmaShape.radius = .25f;

        mavrik.FireHaptic(Haptics.charge5);
        sfx.plasmaCharge5.Play(0);
        plasmaSize = 5;
    }

    void Charge6()
    {
        mavrik.FireHaptic(Haptics.charge5);
        sfx.plasmaCharge5.Play(0);
    }

    void ForwardBarSensorsTouched()
    {
        if (mavrik.GetRawSensorDown(forwardBar1))
        {
            rate = 7f;
        }
        else if (mavrik.GetRawSensorDown(forwardBar2))
        {
            rate = 8f;
        }
        else if (mavrik.GetRawSensorDown(forwardBar3))
        {
            rate = 9f;
        } 
        else if (mavrik.GetRawSensorDown(forwardBar4))
        {
            rate = 10f;
        }
        else if (mavrik.GetRawSensorDown(forwardBar5))
        {
            rate = 11f;
        }
        else if (mavrik.GetRawSensorDown(forwardBar6))
        {
            rate = 12f;
        }
        else if (mavrik.GetRawSensorDown(forwardBar7))
        {
            rate = 13f;
        }
        else if (mavrik.GetRawSensorDown(forwardBar8))
        {
            rate = 12f;
        }
        else if (mavrik.GetRawSensorDown(forwardBar9))
        {
            rate = 15f;
        }
    }

    void SlideSensorsTouched() 
    {

        if (mavrik.GetRawSensorDown(slideBar11) || mavrik.GetRawSensorDown(slideBar12))
        {
            mavrikClassicShot = Haptics.laser_shot_6;
            laser.pitch = 1;
        }
        else if (mavrik.GetRawSensorDown(slideBar9) || mavrik.GetRawSensorDown(slideBar10))
        {
            mavrikClassicShot = Haptics.laser_shot_5;
            laser.pitch = 1.1f;
        }
        else if (mavrik.GetRawSensorDown(slideBar7) || mavrik.GetRawSensorDown(slideBar8))
        {
            mavrikClassicShot = Haptics.laser_shot_4;
            laser.pitch = 1.2f;
        }
        else if (mavrik.GetRawSensorDown(slideBar5) || mavrik.GetRawSensorDown(slideBar6))
        {
            mavrikClassicShot = Haptics.laser_shot_3;
            laser.pitch = 1.3f;

        }
        else if (mavrik.GetRawSensorDown(slideBar3) || mavrik.GetRawSensorDown(slideBar4))
        {
            mavrikClassicShot = Haptics.laser_shot_2;
            laser.pitch = 1.4f;
        }
        else if (mavrik.GetRawSensorDown(slideBar1) || mavrik.GetRawSensorDown(slideBar2))
        {
            mavrikClassicShot = Haptics.laser_shot_1;
            laser.pitch = 1.5f;
        }
    }

    public void CrossBowShootHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_shot);
        sfx.crossbow_shot_sfx.Play(0);
    }

    public void CrossBowSlideHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_sliding);
    }

    public void CrossBowExplodeHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_explode);
    }

    public void ArrowSpawnHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_reload);
        sfx.crossbow_reload_sfx.Play(0);
    }

    public void CrossBowPullOneHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_Load_1);
        sfx.crossbow_Load_1_sfx.Play(0);
    }
    public void CrossBowPullTwoHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_Load_2);
        sfx.crossbow_Load_2_sfx.Play(0);
    }
    public void CrossBowPullThreeHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_Load_3);
        sfx.crossbow_Load_3_sfx.Play(0);
    }
    public void CrossBowPullFourHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_Load_4);
        sfx.crossbow_Load_4_sfx.Play(0);
    }
    public void CrossBowPullFiveHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_Load_5);
        sfx.crossbow_Load_5_sfx.Play(0);
    }

    public void CrossBowPullSnapHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_Load_Locked);
        sfx.crossbow_Load_Locked_sfx.Play(0);
    }

    public void RopeHaptic()
    {
        mavrik.FireHaptic(Haptics.crossbow_rope);
        crossbow_rope_sfx.Play(0);
    }

    /*public void MinigunRotateHaptic()
    {
        mavrik.FireHaptic(Haptics.tennis_ball_rotate, intensityModifier(Evaluate(t)), frequencyModifier() );
    }*/

}

/// <summary>
/// haptics as a separate class so that they are neat in the editor
/// </summary>
[System.Serializable]
public class Haptics{

    public HapticEffectAsset laser_shot_1, laser_shot_2, laser_shot_3, laser_shot_4, laser_shot_5, laser_shot_6, laser_reload,
                             blaster_swap, 
                             crossbow_shot, crossbow_sliding, crossbow_rope, crossbow_reload, crossbow_explode,
                             crossbow_Load_1, crossbow_Load_2, crossbow_Load_3, crossbow_Load_4, crossbow_Load_5, crossbow_Load_Locked,
                             charge1, charge2, charge3, charge4, charge5, charge_shot1, charge_shot2, charge_shot3, charge_shot4, charge_shot5;
}

/// <summary>
/// sound effects as a separate class so that they are neat in the editor
/// </summary>
[System.Serializable]
public class SoundEffects
{
    public AudioSource crossbow_shot_sfx, crossbow_reload_sfx,
                             crossbow_Load_1_sfx, crossbow_Load_2_sfx, crossbow_Load_3_sfx, crossbow_Load_4_sfx, crossbow_Load_5_sfx, crossbow_Load_Locked_sfx,
                            mavrikBlaster, fullAutoBlaster, ballzookaBlaster, gravityBlaster, grapplingCrossbow, explosiveCrossbow, plasmaBlaster, plasmaCharge1,
                            plasmaCharge2, plasmaCharge3, plasmaCharge4, plasmaCharge5, plasmaBlast1, plasmaBlast2, plasmaBlast3, plasmaBlast4, plasmaBlast5;
}
