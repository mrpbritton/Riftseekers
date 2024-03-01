using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
#if UNITY_EDITOR
using UnityEditor;


public class DebuggerWindow : EditorWindow {

    [MenuItem("Window/Debugger")]
    public static void showWindow() {
        GetWindow<DebuggerWindow>("Debugger");
    }


    private void OnGUI() {
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Wipe"))
            SaveData.wipe();
        GUILayout.EndHorizontal();
    }
}

#endif