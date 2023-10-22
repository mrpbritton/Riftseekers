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
    protected static CharacterFrame frame;
    private static int rayDistance = 100; //how far the ray will cast out
    public bool isController;
    public PInput pInput;

    public void updateStats(float dMod, float cdMod) {
        dmgMod = dMod;
        cooldownMod = cdMod;
    }

    protected virtual void Start()
    {
        pInput = new PInput();
        pInput.Enable();
        frame = FindAnyObjectByType<CharacterFrame>();

        pInput.Player.AnyController.started += ctx => IsController();
        pInput.Player.AnyKey.started += ctx => IsKeyboard();
    }

    protected void OnDisable()
    {
        pInput.Player.AnyController.started -= ctx => IsController();
        pInput.Player.AnyKey.started -= ctx => IsKeyboard();
        pInput.Disable();
    }

    private void IsController()
    {
        if (isController) return; //dont set it if it is set

        isController = true;
    }

    private void IsKeyboard()
    {
        if (!isController) return; //dont set it if it is set
        Debug.Log("IsKeyboard");
        isController = false;
    }

    public static Vector3 GetPoint()
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
    protected abstract float getDamage();
    protected abstract float getCooldownTime();   //  NOTE: this does nothing atm

    public float getRealDamage() {
        return getDamage() * dmgMod;
    }
    public float getRealCooldownTime() {
        return getCooldownTime() * cooldownMod;
    }
}