using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Terminals : Singleton<Terminals>
{
    public GameObject screen;
    public TextMeshProUGUI text;
    int randVal;

    public string[] logs;
    private void Start()
    {
        screen.SetActive(false);
    }

    public void powerOn()
    {
        //randVal = Random.Range(0, Inventory.seenLore().Length);
        Debug.Log(randVal);
        text.text = logs[randVal];
        screen.SetActive(true);
    } 

    public void powerOff()
    {
        screen.SetActive(false);
    }

    public void viewLog(int index)
    {
        text.text = logs[index];
    }
}
