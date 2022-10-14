using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    public Sprite first, fullAuto, ballzooka, gravity, crossbeaux, plasma;
    public Blaster blaster;
    private Image image;
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        first = Resources.Load<Sprite>("UI_Sprites/StrikerVR_Sandbox_UI_First");
        fullAuto = Resources.Load<Sprite>("UI_Sprites/StrikerVR_Sandbox_UI_FullAuto");
        ballzooka = Resources.Load<Sprite>("UI_Sprites/StrikerVR_Sandbox_UI_Ballzooka");
        gravity = Resources.Load<Sprite>("UI_Sprites/StrikerVR_Sandbox_UI_Gravity");
        crossbeaux = Resources.Load<Sprite>("UI_Sprites/StrikerVR_Sandbox_UI_Crossbeaux");
        plasma = Resources.Load<Sprite>("UI_Sprites/StrikerVR_Sandbox_UI_Plasma");

        image = gameObject.GetComponent<Image>();

        
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = sprite; 

        if (blaster.fireMode == 0)
        {
            sprite = first;
        }
        else if (blaster.fireMode == 1)
        {
            sprite = fullAuto;
        }
        else if (blaster.fireMode == 2)
        {
            sprite = ballzooka;
        }
        else if (blaster.fireMode == 3)
        {
            sprite = gravity;
        }
        else if (blaster.fireMode == 4)
        {
            sprite = crossbeaux;
        }
        else if (blaster.fireMode == 5)
        {
            sprite = plasma;
        }
    }
}
