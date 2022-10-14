using Script;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  
  [CustomEditor(typeof(Morph))]
  public class MorphEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      if (Application.isPlaying == false)
        return;
      
      if (GUILayout.Button("Respawn"))
      {
        Morph morph = (Morph) target;
        morph.Spawn();
      }
    }
  }
}