using System.Collections;
using System.Collections.Generic;
using StrikerLink.Unity.Runtime.HapticEngine;
using StrikerLink.Unity.Runtime.Core;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationCurve curveAnim;
    private Animation animObject = null;
    private Animator animator = null;
    private float LedDefaultHaloStrength = 0;
    private float LedDefaultStripesStrength = 0;
    private float LedDefaultPowerStrength = 0;
    private bool LedHaloToggle = true;
    private bool LedStripesToggle = true;
    private bool LedPowerToggle = true;

    public float PressDuration = .2F;
    public List<ParticleSystem> ps = new List<ParticleSystem>();
    public GameObject BulletPrefab = null;
    public GameObject BulletPrefabSmallBall = null;
    public GameObject BulletPrefabBigBall = null;
    public GameObject BulletSpawnPoint = null;
    public GameObject MeshRef = null;

    public HapticEffectAsset ballzookaFireSmall, ballzookaFireBig, ballzookaReload, ballzookaRotary;
    public AudioSource miniMode, tennisMode, spinUp, spinDown, spinLoop, muzzleSparks, reloadSparks;

    public StrikerDevice mavrik;
    public Blaster blaster;

    public List<GameObject> TennisBigBall = new List<GameObject>();
    private List<GameObject> AllBigTennisBalls = new List<GameObject>();
    public Transform BigBallSpawnPoint = null;

    public List<GameObject> TennisSmallBall = new List<GameObject>();
    private List<GameObject> AllSmallTennisBalls = new List<GameObject>();
    public Transform SmallBallSpawnPoint = null;

    public Transform FireSpawnPoint = null;
    private float LastShootTime;
    public float ShootDelayBigBall = 0.5f;
    public float ShootDelaySmallBall = 0.1f;

    private float layerWeight_SwapWeapon = 0;
    [HideInInspector] public float newLayerWeight_SwapWeapon = 0;
    private float speedSwitchWeapon = 2;

    private float layerWeight_Trigger = 0;
    private float newLayerWeight_Trigger = 0;
    public float speedTrigger = 2;

    private float layerWeight_LeftTouch = 0;
    private float newlayerWeight_LeftTouch = 0;
    public float speedLeftTouch = 2;


    private float layerWeight_LeftButton = 0;
    private float newLayerWeight_LeftButton = 0;
    public float speedLeftButton = 2;
    private bool LeftButtonBool = false;


    private bool CanFire = true;
    private bool GetCanFire() { return CanFire; }
    private void SetCanFire(bool IsFireAvaliable) { CanFire = IsFireAvaliable; }
    private bool loaded;


    private int deactivatedBalls = 0;
    private bool isFrictionShotEnabled = false;
    private int rifleShot = 6;
    private GameObject[] cloneBullet;
    private GameObject cloneSingleBullet;
    private int rifleShotsCalled = 0;
    private bool bigBallsActivated = false;

    private bool sparks;


    void Start()
    {
        animObject = this.GetComponentInChildren<Animation>();
        animator = this.GetComponentInChildren<Animator>();

        if (MeshRef != null)
        {
            Renderer renderer = MeshRef.GetComponent<Renderer>();
            Material mat = renderer.material;
            LedDefaultHaloStrength = mat.GetFloat("LedHaloStrength");
            LedDefaultStripesStrength = mat.GetFloat("LedStripesStrength");
            LedDefaultPowerStrength = mat.GetFloat("LedPowerStrength");

        }
        AllBigTennisBalls = TennisBigBall;
        AllSmallTennisBalls = TennisSmallBall;
        CanFire = false;
        loaded = false;
        animator.SetBool("Fire", false);

    }

    private void FixedUpdate()
    {

        if (LeftButtonBool)
        {
            StartCoroutine(LeftTouch());
        }
        
    }

    private void Update()
    {
        OnTrigger();
        SpawnBall();
        SwapWeapon();
        OnLeft();
        OnTouchLeft();
        HapticPriority();
        

        Debug.Log("ballshots = " + blaster.ballShots);
        Debug.Log("ballreload = " + blaster.ballReload);
        Debug.Log("ballrotary = " + blaster.ballRotary);
    }
    public void Idle()
    {
        animObject.clip = animObject.GetClip("Base");
        animObject.Play();
        animator.SetBool("Fire", false);
    }
    public void OnFrontBottom()
    {
        animObject.clip = animObject.GetClip("FrontBottom");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[0].Play(true);
    }
    public void OnFrontTop()
    {
        animObject.clip = animObject.GetClip("FrontTop");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[1].Play(true);
    }
    public void OnLedHaloOn()
    {
        if (MeshRef != null)
        {
            Renderer renderer = MeshRef.GetComponent<Renderer>();
            Material mat = renderer.material;
            if (LedHaloToggle)
            {
                mat.SetFloat("LedHaloStrength", 0);
            }
            else
            {
                mat.SetFloat("LedHaloStrength", LedDefaultHaloStrength);
            }
            LedHaloToggle = !LedHaloToggle;
        }


    }

    public void OnLedPowerOn()
    {
        if (MeshRef != null)
        {
            Renderer renderer = MeshRef.GetComponent<Renderer>();
            Material mat = renderer.material;
            if (LedPowerToggle)
            {
                mat.SetFloat("LedPowerStrength", 0);
            }
            else
            {
                mat.SetFloat("LedPowerStrength", LedDefaultPowerStrength);
            }
            LedPowerToggle = !LedPowerToggle;
        }


    }

    public void OnLedStripesOn()
    {
        if (MeshRef != null)
        {
            Renderer renderer = MeshRef.GetComponent<Renderer>();
            Material mat = renderer.material;
            if (LedStripesToggle)
            {
                mat.SetFloat("LedStripesStrength", 0);
            }
            else
            {
                mat.SetFloat("LedStripesStrength", LedDefaultStripesStrength);
            }
            LedStripesToggle = !LedStripesToggle;
        }


    }
    public void OnLeft()
    {
        layerWeight_LeftButton = Mathf.MoveTowards(layerWeight_LeftButton, newLayerWeight_LeftButton, curveAnim.Evaluate(Time.deltaTime * speedLeftButton));
        animator.SetLayerWeight(4, layerWeight_LeftButton);
        if (Input.GetKeyDown("3") && layerWeight_LeftButton == 1)
        {
            ps[0].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Debug.Log("Off scope");
            newLayerWeight_LeftButton = 0;

        }
        else if (Input.GetKeyDown("3") && layerWeight_LeftButton == 0)
        {
            newLayerWeight_LeftButton = 1;
            Debug.Log("On scope");
            ps[0].Play();
        }
    }
    public void OnPowerButton()
    {
        animObject.clip = animObject.GetClip("Power");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[3].Play(true);
    }
    public void OnRight()
    {
        animObject.clip = animObject.GetClip("Right");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[4].Play(true);
    }
    public void OnTouchLeft()
    {
        if (Input.GetKeyDown("v") || mavrik.GetButtonDown(blaster.leftTouch))
        {
            LeftButtonBool = true;
        }
    }
    public void OnTouchpadsTouch()
    {
        ps[6].Play(true);
    }

    public void OnTrigger()
    {
        layerWeight_Trigger = Mathf.MoveTowards(layerWeight_Trigger, newLayerWeight_Trigger, curveAnim.Evaluate(Time.deltaTime * speedTrigger));
        //animator.SetLayerWeight(3, layerWeight_Trigger); //rotation animation
        animator.SetLayerWeight(2, layerWeight_Trigger); //trigger pull animation

        if (mavrik.GetTriggerDown() || Input.GetKeyDown("1"))
        {
            if (loaded) 
            { 
                SetCanFire(true);
            }

            animator.SetBool("Fire", true);

            if (CanFire)
            {
                blaster.ballShots = true;
            }
            else
            {
                blaster.ballShots = false;
            }

            newLayerWeight_Trigger = 1;

        }
        else if (mavrik.GetTriggerUp())
        {
            animator.SetBool("Fire", false);
            SetCanFire(false);
            muzzleSparks.Stop();
            ps[1].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            newLayerWeight_Trigger = 0;
        }
    }

    public void FrickinFire()
    {
        Fire(CanFire, isFrictionShotEnabled);
        spinLoop.Play();
        

        if (!blaster.ballShots && blaster.ballRotary)
        {
            mavrik.FireHaptic(ballzookaRotary);
        }
    }

    private void HapticPriority()
    {
        if (CanFire == true)
        {
            blaster.ballRotary = false;
        }
        else if (CanFire == false)
        {
            blaster.ballRotary = true;
        }
    }

    public void Fire(bool isFireAvaliable, bool isTripleShotEnabled)
    {
        float currentDelay = 0f;
        if (bigBallsActivated == true)
        {
            currentDelay = ShootDelayBigBall;
        }
        else if (bigBallsActivated == false)
        {
            currentDelay = ShootDelaySmallBall;
        }
        if ((LastShootTime + currentDelay) < Time.time && isFireAvaliable)
        {
            ps[4].Play(true);
            

            if (isTripleShotEnabled && !bigBallsActivated)
            {
                Debug.Log("rifle shot size:" + rifleShot);
                for (int i = 0; i < rifleShot; i++)
                {
                    BulletInstantiate(i);
                }
            }
            else if (isTripleShotEnabled && bigBallsActivated)
            {
                Debug.Log("rifle shot size:" + rifleShot);
                for (int i = 0; i < rifleShot; i++)
                {
                    BulletInstantiate(i);
                }
            }
            else
            {
                BulletInstantiate(0);
            }

            if (bigBallsActivated)
            {
                DecreaseBalls(TennisBigBall);

            }
            else
            {
                DecreaseBalls(TennisSmallBall);

            }

            LastShootTime = Time.time;
        }
    }

    private void BulletInstantiate(int index)
    {
        if (bigBallsActivated)
        {
            BulletPrefab = BulletPrefabBigBall;
            blaster.tennis.pitch = 1f;
        }
        else
        {
            BulletPrefab = BulletPrefabSmallBall;
            blaster.tennis.pitch = 1.4f;
        }

        cloneSingleBullet = Instantiate(BulletPrefab, FireSpawnPoint.transform.position, FireSpawnPoint.transform.rotation);

        if (blaster.ballShots && bigBallsActivated)
        {
            mavrik.FireHaptic(ballzookaFireBig);
            
        }
        else if (blaster.ballShots && !bigBallsActivated)
        {
            mavrik.FireHaptic(ballzookaFireSmall);
        }

        blaster.tennis.Play(0);
        cloneSingleBullet.SetActive(true);
        cloneSingleBullet.GetComponent<Rigidbody>().AddForce(-transform.forward * 5f);
        cloneSingleBullet.GetComponent<Rigidbody>().useGravity = false;

        Debug.Log("Big Balls activated: " + bigBallsActivated);
        Debug.Log("Ballshots?: " + blaster.ballShots);

    }

    public void SwapWeapon()
    {
        layerWeight_SwapWeapon = Mathf.MoveTowards(layerWeight_SwapWeapon, newLayerWeight_SwapWeapon, curveAnim.Evaluate(Time.deltaTime * speedSwitchWeapon));
        animator.SetLayerWeight(1, layerWeight_SwapWeapon);

        if (Input.GetKeyDown("2"))
        {
            if (newLayerWeight_SwapWeapon == 0)
            {
                newLayerWeight_SwapWeapon = 1;
                isFrictionShotEnabled = true;
            }
            else
            {
                newLayerWeight_SwapWeapon = 0;
                isFrictionShotEnabled = false;
            }
            //Debug.Log("Is rifle shot enabled: " + isFrictionShotEnabled);
        }
    }
    private void DecreaseBalls(List<GameObject> ballsList)
    {
        
        var modulo = (ballsList.Count - deactivatedBalls) % rifleShot;
        var moduloLeft = ((ballsList.Count - deactivatedBalls) / rifleShot) - modulo;
        
        //Debug.Log("Amount of deactivated balls BEFORE this shot: " + deactivatedBalls);
        if (!isFrictionShotEnabled && ballsList.Count > 0)
        {
            foreach (var ball in ballsList)
            {
                if (ball.gameObject.activeSelf)
                {
                    ps[1].Play();
                    sparks = true;
                    ball.gameObject.SetActive(false);
                    deactivatedBalls++;

                    if (deactivatedBalls == ballsList.Count)
                    {
                        ps[2].Play();
                        sparks = false;
                        loaded = false;
                        muzzleSparks.Stop();
                        SetCanFire(false);
                        blaster.ballRotary = true;
                        blaster.ballShots = false;
                        ps[1].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    

                    break;
                }
            }
        }
        //((ballsList.Count-deactivatedBalls) % rifleShot == 0)
        else if (isFrictionShotEnabled && moduloLeft != 0)
        {
            rifleShotsCalled++;
            for (int j = (rifleShotsCalled - 1) * rifleShot; j < rifleShot + rifleShot * (rifleShotsCalled - 1); j++)
            {
                if (ballsList[j].gameObject.activeSelf)
                {
                    ps[1].Play();
                    sparks = true;
                    ballsList[j].gameObject.SetActive(false);
                    deactivatedBalls++;
                }
                if (deactivatedBalls == ballsList.Count)
                {
                    ps[2].Play();
                    sparks = false; 
                    loaded = false;
                    muzzleSparks.Stop();
                    SetCanFire(false);
                    blaster.ballRotary = true;
                    //blaster.ballShots = false;
                    ps[1].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    newLayerWeight_SwapWeapon = 0;
                    isFrictionShotEnabled = false;
                    break;
                }
                newLayerWeight_SwapWeapon = 0;
                isFrictionShotEnabled = false;
            }
        }
        else if (isFrictionShotEnabled && modulo != 0 && moduloLeft == 0)
        {
            rifleShotsCalled++;
            int activeBallsLeft = ballsList.Count - deactivatedBalls;
            rifleShot = activeBallsLeft;
            //Debug.Log("active balls left: " + activeBallsLeft);
            for (int i = ballsList.Count - 1; i > ballsList.Count - 1 - activeBallsLeft; i--)
            {
                if (ballsList[i].gameObject.activeSelf)
                {
                    ps[1].Play();
                    sparks = true;
                    ballsList[i].gameObject.SetActive(false);
                    deactivatedBalls++;
                }
                if (deactivatedBalls == ballsList.Count)
                {
                    ps[2].Play();
                    sparks = false;
                    loaded = false;
                    muzzleSparks.Stop();
                    SetCanFire(false);
                    blaster.ballRotary = true;
                    //blaster.ballShots = false;
                    //ps[1].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    newLayerWeight_SwapWeapon = 0;
                    isFrictionShotEnabled = false;
                    break;
                }
            }
        }

        //Debug.Log("Amount of deactivated balls AFTER this shot: " + deactivatedBalls);
        //Debug.Log("left after modulo rifle shot: " + ((ballsList.Count - deactivatedBalls) % rifleShot));
    }
    public void SpawnBall()
    {
        if (!bigBallsActivated && mavrik.GetButtonDown(blaster.leftTouch))
        {

            bigBallsActivated = true;
            tennisMode.Play();

        }
        else if (bigBallsActivated && mavrik.GetButtonDown(blaster.leftTouch))
        {

            bigBallsActivated = false;
            miniMode.Play();
        }


        if ((mavrik.GetSensorDown(blaster.ballzookaReload) && bigBallsActivated) || Input.GetKeyDown("z")) {
            ClearAllBall();
            BulletPrefab = TennisBigBall[0];
            CanFire = true;
            blaster.ballReload = true;
            loaded = true;
            mavrik.FireHaptic(ballzookaReload);
            deactivatedBalls = 0;
            rifleShotsCalled = 0;
            //bigBallsActivated = true;
            rifleShot = 3;
            for (int i = 0; i < TennisBigBall.Count; i++)
            {
                TennisBigBall[i].transform.position = BigBallSpawnPoint.position;
                TennisBigBall[i].SetActive(true);
            }

        }
        else if ((mavrik.GetSensorDown(blaster.ballzookaReload) && !bigBallsActivated) || Input.GetKeyDown("x"))
        {
            ClearAllBall();
            BulletPrefab = TennisSmallBall[0];
            CanFire = true;
            blaster.ballReload = true;
            loaded = true;
            mavrik.FireHaptic(ballzookaReload);
            deactivatedBalls = 0;
            rifleShotsCalled = 0;
            //bigBallsActivated = false;
            rifleShot = 6;
            for (int i = 0; i < TennisSmallBall.Count; i++)
            {
                TennisSmallBall[i].transform.position = SmallBallSpawnPoint.position;
                TennisSmallBall[i].SetActive(true);
            }
        }
        else
        {
            blaster.ballReload = false;
        }
    }
    public void ClearAllBall()
    {

        StartCoroutine(GlassEffect());
        ps[3].Play(true);
        reloadSparks.Play();
        ps[2].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        for (int i = 0; i < TennisBigBall.Count; i++)
        {
            TennisBigBall[i].SetActive(false);
        }
        for (int i = 0; i < TennisSmallBall.Count; i++)
        {

            TennisSmallBall[i].SetActive(false);
        }
    }

    public IEnumerator GlassEffect()
    {
        Shader.SetGlobalFloat("_EnableGlassEmission", 1);
        yield return new WaitForSeconds(0.5f);
        Shader.SetGlobalFloat("_EnableGlassEmission", 0);
    }

    public IEnumerator LeftTouch()
    {
        Debug.Log("c");
        newlayerWeight_LeftTouch = 1;
        layerWeight_LeftTouch = Mathf.MoveTowards(layerWeight_LeftTouch, newlayerWeight_LeftTouch, curveAnim.Evaluate(Time.deltaTime * speedLeftTouch));
        animator.SetLayerWeight(5, layerWeight_LeftTouch);
        yield return new WaitForSeconds(0.7f);
        newlayerWeight_LeftTouch = 0;
        layerWeight_LeftTouch = Mathf.MoveTowards(layerWeight_LeftTouch, newlayerWeight_LeftTouch, curveAnim.Evaluate(Time.deltaTime * speedLeftTouch));
        animator.SetLayerWeight(5, layerWeight_LeftTouch);
        LeftButtonBool = false;
        StopCoroutine(LeftTouch());
    }

    public void PlaySpinDown()
    {
        spinDown.Play();
        spinLoop.Stop();

    }

    public void PlaySpinUp()
    {
        spinUp.Play();
        spinDown.Stop();
        spinLoop.Stop();
    }

    private void PlaySparks()
    {
        if (sparks)
        {
            muzzleSparks.Play();
        }  
    }
}