using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class qAbility : Attack
{
    public override int getDamage()
    {
        return 12;
    }
    public override attackType getAttackType()
    {
        return attackType.Sword;
    }

    /*public override void attack(InputAction.CallbackContext c)
    {
        Debug.Log("Basic Attack Performed");
    }*/

    public override void attack()
    {
        Debug.Log("Q ability activated");
    }
}