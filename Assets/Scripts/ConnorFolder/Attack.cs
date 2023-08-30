using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {
    public enum attackType {
        None, Sword, Shoot
    }

    PInput input;

    //  NOTE: cannot use Awake function in any of the children classes
    private void Awake() {
        input = new PInput();
        input.Enable();
        if(isBasicAttack())
            input.Player.BasicAttack.performed += ctx => attack();
        else
            input.Player.SecondAttack.performed += ctx => attack();
    }

    //  NOTE: cannot use OnDisable function in any of the children classes
    private void OnDisable() {
        input.Disable();
    }

    public abstract attackType getAttackType();
    public abstract int getDamage();
    public abstract void attack();
    public abstract bool isBasicAttack();   //  returns false if its a secondary attack
}