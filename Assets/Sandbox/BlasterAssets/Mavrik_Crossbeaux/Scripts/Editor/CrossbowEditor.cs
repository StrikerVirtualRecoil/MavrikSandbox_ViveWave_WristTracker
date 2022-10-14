using Script;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  [CustomEditor(typeof(Crossbow))]
  public class CrossbowEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      if (Application.isPlaying == false)
        return;
      
      Crossbow crossbow = target as Crossbow;
      
      if (GUILayout.Button("Reload")) 
        crossbow.StartLoading();
      
      if (GUILayout.Button("Shot")) 
        crossbow.Shoot();
      
      if (GUILayout.Button("Change Ammo Type")) 
        crossbow.SwapArrow();
    }
  }
}