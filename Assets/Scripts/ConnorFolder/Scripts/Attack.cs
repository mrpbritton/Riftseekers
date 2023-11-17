using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {
    public enum attackType
    {
        None, Basic, Secondary, QAbility, EAbility, RAbility, FAbility
    }
    float dmgMod = 1.0f;
    float cooldownMod = 1.0f;
    public AbilityLibrary.abilType abilType;
    protected static CharacterFrame frame;
    private static int rayDistance = 100; //how far the ray will cast out
    protected PInput pInput;
    public PlayerUICanvas cooldownBar;

    public void updateStats(float dMod, float cdMod) {
        dmgMod = dMod;
        cooldownMod = cdMod;
    }

    protected virtual void Start()
    {
        pInput = new PInput();
        pInput.Enable();
        frame = FindAnyObjectByType<CharacterFrame>();
    }

    protected void OnDisable()
    {
        pInput.Disable();
    }

    protected Vector3 GetPoint()
    {
        //puts the cursor direction vector in the middle of the screen
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit hit;
        //Debug.DrawRay(cursorPos, Camera.main.transform.forward * rayDistance, Color.red, 10);
        Physics.Raycast(cursorPos, Camera.main.transform.forward, out hit, rayDistance, LayerMask.GetMask("AllButCam"));
        return hit.point;
    }    

    public abstract attackType getAttackType();
    public abstract void attack();
    public abstract void reset();
    protected abstract float getDamage();
    protected abstract float getCooldownTime();   //  NOTE: this does nothing atm

    public float getRealDamage() {
        return getDamage() * dmgMod;
    }
    public float getRealCooldownTime() {
        return getCooldownTime() * cooldownMod;
    }
}