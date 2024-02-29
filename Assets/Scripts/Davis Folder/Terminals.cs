using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Terminals : MonoBehaviour
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
        randVal = Random.Range(0, logs.Length);
        Debug.Log(randVal);
        screen.SetActive(true);
    } 

}
