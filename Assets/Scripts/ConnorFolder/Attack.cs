using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Attack : MonoBehaviour {
    public enum attackType {
        None, Basic, Secondary, QAbility, EAbility, RAbility, FAbility
    }

    float dmgMod;
    float cooldownMod;

    public void updateStats(float dMod, float cdMod) {
        dmgMod = dMod;
        cooldownMod = cdMod;
    }

    public abstract attackType getAttackType();
    public abstract void attack();
    protected abstract float getDamage();
    protected abstract float getCooldownTime();   //  NOTE: this does nothing atm

    public float getRealDamage() {
        return getDamage() * dmgMod;
    }
    public float getRealCooldownTime() {
        return getCooldownTime() * cooldownMod;
    }
}