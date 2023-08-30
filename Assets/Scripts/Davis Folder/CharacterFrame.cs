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

        pInput.Player.BasicAttack.started += ctx => basicAttack.attack();
        pInput.Player.SecondAttack.started += ctx => secondAttack.attack();
        pInput.Player.Ability1.started += ctx => qAbility.attack();
        pInput.Player.Ability2.started += ctx => eAbility.attack();
        pInput.Player.Ability3.started += ctx => rAbility.attack();
        pInput.Player.Ult.started += ctx => fAbility.attack();
    }

    private void OnDisable()
    {
        pInput.Disable();
    }
    //tree to execute each respective attack
}