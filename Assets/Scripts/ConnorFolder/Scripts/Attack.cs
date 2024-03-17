using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {
    public enum AttackType
    {
        Melee, Ranged, Special, Movement, None
    }
    public abstract AttackType AType { get; }
    public abstract AttackScript AScript { get; }
    protected abstract float SetDamage { get; }
    protected abstract float SetCooldownTime { get; }   //  NOTE: this does nothing atm
    private readonly int rayDistance = 100; //how far the ray will cast out
    protected PInput pInput;
    public PlayerUICanvas cooldownBar;
    protected ExplosionManager explosionManager;

    protected virtual void Start()
    {
        pInput = new PInput();
        pInput.Enable();
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
        hit.point = new Vector3(hit.point.x, Vector3.forward.y, hit.point.z);
        return hit.point;
    }    

    public float GetDamage()
    {
        return SetDamage * PlayerStats.AttackDamage;
    }

    public float GetCooldownTime()
    {
        return SetCooldownTime * PlayerStats.CooldownMod;
    }

    public abstract void DoAttack();
    /// <summary>
    /// This is called in an attack to execute the correct animation.
    /// </summary>
    /// <param name="anim">The animator of the character</param>
    /// <param name="reset">If true, the triggers will be set to go back to normal</param>
    public abstract void Anim(Animator anim, bool reset);
    public abstract void ResetAttack();
}