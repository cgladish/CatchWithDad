using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GlobalSettings))]
public class GlobalSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script lets you update global variables while debugging.", MessageType.Info);
        GlobalSettings globalSettings = (GlobalSettings)target;
        if (GlobalSettings.Instance.IsDevModeEnabled && GUILayout.Button("Disable Dev Mode")) {
            GlobalSettings.Instance.IsDevModeEnabled = false;
        }
        if (!GlobalSettings.Instance.IsDevModeEnabled && GUILayout.Button("Enable Dev Mode")) {
            GlobalSettings.Instance.IsDevModeEnabled = true;
        }
    }
}
