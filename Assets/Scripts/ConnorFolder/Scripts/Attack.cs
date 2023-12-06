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
    protected ExplosionManager explosionManager;

    public void updateStats(float dMod, float cdMod) {
        dmgMod = dMod;
        cooldownMod = cdMod;
    }

    protected virtual void Start()
    {
        pInput = new PInput();
        pInput.Enable();
        frame = FindAnyObjectByType<CharacterFrame>();
        explosionManager = FindObjectOfType<ExplosionManager>();
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
    /// <summary>
    /// This is called in an attack to execute the correct animation.
    /// </summary>
    /// <param name="anim">The animator of the character</param>
    /// <param name="reset">If true, the triggers will be set to go back to normal</param>
    public abstract void anim(Animator anim, bool reset);
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