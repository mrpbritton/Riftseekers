using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Terminals : Singleton<Terminals>
{
    public GameObject screen;
    public TextMeshProUGUI text;
    int randVal;

    public List<LorePiece_SO> logs;
    public List<Button> logButtons;
    private void Start()
    {
        screen.SetActive(false);
    }

    public void powerOn()
    {
        //randVal = Random.Range(0, Inventory.seenLore().Length);
        randVal = Inventory.getRandSeenLoreIndex();
        List<int> list = new();
        if (Inventory.getSeenLore().Count > 0)
        {
            list = Inventory.getSeenLore(); 
            foreach (int i in list)
            {
                logButtons[i].interactable = true;
            }
        }
        Debug.Log(randVal);
        if (randVal > 0)
            text.text = logs[randVal].lore;
        else
            text.text = "ERROR 331: NO LOG FOUND. CHECK SYSTEM INTEGRITY AND STORAGE.";
        screen.SetActive(true);
    }

    public void powerOff()
    {
        screen.SetActive(false);
    }

    public void viewLog(int index)
    {
        text.text = logs[index].lore;
    }
}