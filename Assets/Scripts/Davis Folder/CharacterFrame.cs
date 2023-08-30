using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFrame : MonoBehaviour
{
    private PInput pInput;
    public Attack basicAttack;
    public Attack secondAttack;
    public Attack qAbility;
    public Attack eAbility;
    public Attack rAbility;
    public Attack fAbility;

    //more options to come in the future
    private void Start()
    {
        pInput = new PInput();
        pInput.Enable();

       // pInput.Player.BasicAttack.started += basicAttack.attack();
    }

    private void OnDisable()
    {
        pInput.Disable();
    }
    //tree to execute each respective attack
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("Does this work?");
            qAbility.attack();
        }
    }
}