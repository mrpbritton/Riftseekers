using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterFrame : MonoBehaviour
{
    private PInput pInput;
    [Header("Attacks and Abilities")]
    public Attack basicAttack;
    public Attack secondAttack;
    public Attack qAbility;
    public Attack eAbility;
    public Attack rAbility;
    public Attack fAbility;

    [Header("Stats")]
    public float movementSpeed;
    public float dashSpeed;

    Coroutine attacker = null;

    public static Action UpdateStats = delegate { };

    //more options to come in the future
    private void Start()
    {
        pInput = new PInput();
        pInput.Enable();

        pInput.Player.BasicAttack.started += ctx => performAttack(basicAttack);
        pInput.Player.SecondAttack.started += ctx => performAttack(secondAttack);
        pInput.Player.Ability1.started += ctx => performAttack(qAbility);
        pInput.Player.Ability2.started += ctx => performAttack(eAbility);
        pInput.Player.Ability3.started += ctx => performAttack(rAbility);
        pInput.Player.Ult.started += ctx => performAttack(fAbility);
    }

    //  waits for the attack cooldown to finish
    IEnumerator attackWaiter(float cooldownTime) {
        yield return new WaitForSeconds(cooldownTime);
        attacker = null;
    }

    void performAttack(Attack curAttack) {
        if(attacker != null) return;
        curAttack.attack();
        attacker = StartCoroutine(attackWaiter(curAttack.cooldownTime()));
    }

    public static void Restat()
    {
        UpdateStats();
    }
    
    private void OnDisable()
    {
        pInput.Disable();
    }
    //tree to execute each respective attack
}