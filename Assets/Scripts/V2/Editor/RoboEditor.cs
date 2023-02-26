using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoboController))]
public class RoboEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoboController robo = target as RoboController;
        if(GUILayout.Button("Toggle Place"))
        {
            robo.ToggleLocker();
        }
    }
}
