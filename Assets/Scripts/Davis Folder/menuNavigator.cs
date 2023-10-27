using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class menuNavigator : MonoBehaviour
{
    public GameObject button1;


    public void startNavigation()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button1.gameObject);
    }

    private void OnEnable()
    {
        EnemyController.levelComplete += startNavigation;

    }

}
