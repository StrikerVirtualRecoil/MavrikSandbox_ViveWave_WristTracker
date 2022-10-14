using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Animation animObject = null;
    private float LedDefaultStrength = 0;
    private bool LedToggle = true;

    public float PressDuration = .2F;
    public List<ParticleSystem> ps = new List<ParticleSystem>();
    public GameObject BulletPrefab = null;
    public GameObject BulletSpawnPoint = null;
    public GameObject MeshRef = null;

    public bool FrontBottom = false;
    public bool FrontTop = false;
    public bool LedHaloOn = false;
    public bool Left = false;
    public bool PowerButton = false;
    public bool PowerLamp = false;
    public bool Right = false;
    public bool TouchLeft = false;
    public bool TouchpadsTouch = false;
    public bool TouchRight = false;
    public bool Trigger = false;
    void Start()
    {
        animObject = this.GetComponent<Animation>();
        if (MeshRef != null)
        {
            Renderer renderer = MeshRef.GetComponent<Renderer>();
            Material mat = renderer.material;
            LedDefaultStrength = mat.GetFloat("LedStrength");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 14; i++)
        {
            if (FrontBottom)
            {
                OnFrontBottom();
                FrontBottom = false;
            }
            if (FrontTop)
            {
                OnFrontTop();
                FrontTop = false;
            }
            if (LedHaloOn)
            {
                OnLedHaloOn();
                LedHaloOn = false;
            }
            if (Left)
            {
                OnLeft();
                Left = false;
            }
            if (PowerButton)
            {
                OnPowerButton();
                PowerButton = false;
            }
            if (PowerLamp)
            {
                OnPowerLamp();
                PowerLamp = false;
            }
            if (Right)
            {
                OnRight();
                Right = false;
            }
            if (TouchLeft)
            {
                OnTouchLeft();
                TouchLeft = false;
            }
            if (TouchpadsTouch)
            {
                OnTouchpadsTouch();
                TouchpadsTouch = false;
            }
            if (TouchRight)
            {
                OnTouchRight();
                TouchRight = false;
            }
            if (Trigger)
            {
                OnTrigger();
                Trigger = false;
            }
        }
    }

    public void Idle()
    {
        animObject.clip = animObject.GetClip("Armature|Base");
        animObject.Play();
    }
    public void OnFrontBottom()
    {
        animObject.clip = animObject.GetClip("Armature|FrontBottom");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[0].Play(true);
    }
    public void OnFrontTop() 
    {
        animObject.clip = animObject.GetClip("Armature|FrontTop");
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
            if (LedToggle)
            {
                mat.SetFloat("LedStrength",0);
            }
            else
            {
                mat.SetFloat("LedStrength", LedDefaultStrength);
            }
            LedToggle = !LedToggle;
        }
        

    }
    public void OnLeft()
    {
        animObject.clip = animObject.GetClip("Armature|Left");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[2].Play(true);
    }
    public void OnPowerButton()
    {
        animObject.clip = animObject.GetClip("Armature|Power");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[3].Play(true);
    }
    public void OnPowerLamp()
    {

    }
    public void OnRight()
    {
        animObject.clip = animObject.GetClip("Armature|Right");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[4].Play(true);
    }
    public void OnTouchLeft()
    {
        animObject.clip = animObject.GetClip("Armature|TouchLeft");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[5].Play(true);
    }
    public void OnTouchpadsTouch()
    {
        ps[6].Play(true);
    }
    public void OnTouchRight()
    {
        animObject.clip = animObject.GetClip("Armature|TouchRight");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[7].Play(true);
    }
    public void OnTrigger()
    {
        animObject.clip = animObject.GetClip("Armature|Trigger");
        animObject.Play();
        Invoke("Idle", PressDuration);
        ps[8].Play(true);
        ps[9].Play(true);

        if(BulletPrefab != null && BulletSpawnPoint != null)
        {
            Instantiate(BulletPrefab, BulletSpawnPoint.transform.position, Quaternion.LookRotation(BulletSpawnPoint.transform.forward));//Quaternion.LookRotation(BulletSpawnPoint.transform.forward)
        }
    }
}
