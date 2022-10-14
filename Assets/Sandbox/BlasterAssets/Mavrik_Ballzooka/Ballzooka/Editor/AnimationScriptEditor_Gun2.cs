using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunController))]

public class AnimationScriptEditor_Gun2 : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        var obj = target as GunController;
        if(GUILayout.Button("FrontBottom"))
            obj.OnFrontBottom();
        if(GUILayout.Button("FrontTop"))
            obj.OnFrontTop();
        if(GUILayout.Button("LedHaloOn"))
            obj.OnLedHaloOn();
        if(GUILayout.Button("LedStripesOn"))
            obj.OnLedStripesOn();
        if(GUILayout.Button("LedPowerOn"))
            obj.OnLedPowerOn();
        if(GUILayout.Button("Left"))
            obj.OnLeft();
        if(GUILayout.Button("PowerButton"))
            obj.OnPowerButton();
        if(GUILayout.Button("Right"))
            obj.OnRight();
        if(GUILayout.Button("TouchLeft"))
            obj.OnTouchLeft();
        if(GUILayout.Button("TouchpadsTouch"))
            obj.OnTouchpadsTouch();
        if(GUILayout.Button("SwapWeapon"))
            if (obj.newLayerWeight_SwapWeapon == 1)
                obj.newLayerWeight_SwapWeapon = 0;
            else 
                obj.newLayerWeight_SwapWeapon = 1;
        

        DrawDefaultInspector();
    }
}