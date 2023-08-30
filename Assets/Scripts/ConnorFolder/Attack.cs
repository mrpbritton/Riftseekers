using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Attack : MonoBehaviour {
    public enum attackType {
        None, Basic, Secondary, QAbility, EAbility, RAbility, FAbility
    }


    public abstract attackType getAttackType();
    public abstract int getDamage();
    public abstract void attack();
}