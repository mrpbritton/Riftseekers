using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Attack : MonoBehaviour {
    public enum attackType {
        None, Basic, Secondary, QAbility, EAbility, RAbility, FAbility
    }

    float dmgMod = 1.0f;
    float cooldownMod = 1.0f;
    public AbilityLibrary.abilType abilType;
    private static int rayDistance = 100; //how far the ray will cast out

    public void updateStats(float dMod, float cdMod) {
        dmgMod = dMod;
        cooldownMod = cdMod;
    }

    public static Vector3 GetPoint()
    {
        //puts the cursor direction vector in the middle of the screen
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(cursorPos, Camera.main.transform.forward * rayDistance, Color.red, 10);
        Physics.Raycast(cursorPos, Camera.main.transform.forward, out hit, rayDistance);
        //Debug.Log($"hit: {hit.point} | cursorPos: {cursorPos}");
        return hit.point;
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