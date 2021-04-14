using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(MinimapConfig))]
public class MinimapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("add"))
        {
            MinimapConfig minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<MinimapConfig>();
            minimap.AddElement();

        }
        if (GUILayout.Button("remove"))
        {
            MinimapConfig minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<MinimapConfig>();
            minimap.RemoveElement();

        }
    }
}
#endif
