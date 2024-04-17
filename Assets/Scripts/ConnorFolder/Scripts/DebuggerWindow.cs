using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


public class DebuggerWindow : EditorWindow {
    [MenuItem("Window/Debugger")]
    public static void showWindow() {
        GetWindow<DebuggerWindow>("Debugger");
    }


    private void OnGUI() {
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Save")) {
            Inventory.saveInventory();
            FindObjectOfType<PlayerStats>().save();
            Debug.Log("Saved");
        }
        if(GUILayout.Button("Clear")) {
            SaveData.wipe();
            Debug.Log("Cleared");
        }
        if(GUILayout.Button("Add Lore")) {
            int ind = 0;
            do {
                if(AugmentLibrary.I == null)
                    Debug.Log("Press Play b4 pressing this");
                ind = Random.Range(0, AugmentLibrary.I.getLoreCount());
            } while(Inventory.seenLore(ind));
            Debug.Log("Added: " + ind);
            Inventory.removeLoreIndex(ind);
        }
        GUILayout.EndHorizontal();
    }
}

#endif