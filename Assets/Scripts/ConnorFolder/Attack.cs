using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Attack : MonoBehaviour {
    public enum attackType {
        None, Basic, Secondary, QAbility, EAbility, RAbility, FAbility
    }

    int dmgMod;
    float cooldownMod;

    public void updateStats(int dMod, float cdMod) {
        dmgMod = dMod;
        cooldownMod = cdMod;
    }


    public abstract attackType getAttackType();
    public abstract void attack();
    protected abstract int getDamage();
    protected abstract float getCooldownTime();   //  NOTE: this does nothing atm

    public int getRealDamage() {
        return getDamage() * dmgMod;
    }
    public float getRealCooldownTime() {
        return getCooldownTime() * cooldownMod;
    }
}