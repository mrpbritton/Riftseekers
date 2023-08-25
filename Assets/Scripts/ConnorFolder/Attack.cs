using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {
    public enum attackType {
        None, Sword, Shoot
    }

    public abstract attackType getAttackType();
    public abstract int getDamage();
    public abstract void attack();
}