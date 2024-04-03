using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class outOfGameMenus : MonoBehaviour
{
    public GameObject startingButton;


    private void OnEnable()
    {
        startMenu();
        Cursor.visible = true;
    }

    public void startMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startingButton.gameObject);
    }

    public void restartWipeData() {
        SaveData.wipe();
    }
}
