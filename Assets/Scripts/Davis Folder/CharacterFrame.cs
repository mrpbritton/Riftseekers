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
    private bool canAttack;

    //more options to come in the future
    private void Start()
    {
        pInput = new PInput();
        pInput.Enable();

        /*        pInput.Player.BasicAttack.started += ctx => basicAttack.attack();
                pInput.Player.SecondAttack.started += ctx => secondAttack.attack();
                pInput.Player.Ability1.started += ctx => qAbility.attack();
                pInput.Player.Ability2.started += ctx => eAbility.attack();
                pInput.Player.Ability3.started += ctx => rAbility.attack();
                pInput.Player.Ult.started += ctx => fAbility.attack();*/

        pInput.Player.BasicAttack.started += ctx => bAttack();
        pInput.Player.SecondAttack.started += ctx => sAttack();
        pInput.Player.Ability1.started += ctx => qAbil();
        pInput.Player.Ability2.started += ctx => eAbil();
        pInput.Player.Ability3.started += ctx => rAbil();
        pInput.Player.Ult.started += ctx => fAbil();
    }

    private void bAttack()
    {
        if (!canAttack) return;
        canAttack = false; 
        basicAttack.attack();
        canAttack = true;
    }

    private void sAttack()
    {
        if (!canAttack) return;
        canAttack = false; 
        secondAttack.attack();
        canAttack = true;
    }
    private void qAbil()
    {
        if (!canAttack) return;
        canAttack = false;
        qAbility.attack();
        canAttack = true;
    }
    private void eAbil()
    {
        if (!canAttack) return;
        canAttack = false; 
        eAbility.attack();
        canAttack = true;
    }
    private void rAbil()
    {
        if (!canAttack) return;
        canAttack = false; 
        rAbility.attack();
        canAttack = true;
    }
    private void fAbil()
    {
        if (!canAttack) return;
        canAttack = false; 
        fAbility.attack();
        canAttack = true;
    }

    private void OnDisable()
    {
        pInput.Disable();
    }
    //tree to execute each respective attack
}