using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : Attack
{
    protected override float SetDamage => 0f;
    protected override float SetCooldownTime => 3f;/*
    [SerializeField]
    private int remainingCharges; //how many charges the dash has left
    [SerializeField, Tooltip("Time it takes for a whole dash to recharge in seconds")]
    private float dashChargeCooldown;
    private bool canRecharge; //if the dash can recharge*/
    public bool cantDash; //whether or not the player can dash

    private CharacterController player;
    private Vector3 dashDirection;
    public static Transform playerTrans;
    //private bool bMove = true;
    public override AttackScript AScript => AttackScript.Dash; 
    public override AttackType AType => AttackType.Movement;

    private void OnEnable()
    {
        //DashUI.UpdateDashUI(remainingCharges);
        player = gameObject.GetComponent<CharacterController>();
        //canRecharge = true;
    }

    private void Update()
    {
/*        if (remainingCharges < PlayerStats.DashCharges && canRecharge && bMove)
        {
            StartCoroutine(RechargeDash());
        }*/
    }

    public override void DoAttack() //action
    {
        if (cantDash == true) return;
        StartCoroutine(DoDash());
    }

    IEnumerator DoDash()
    {
        Debug.Log($"{PlayerStats.DashDistance}, {PlayerStats.DashSpeed}");
        AkSoundEngine.PostEvent("Dash", gameObject);

        cantDash = true;
        float dTimeRemaining = 0;
        dashDirection = PlayerMovement.GetDirection();
        if (SpriteManager.I.UpdateSpriteToDash(PlayerMovement.GetDirection())) //UpdateSpriteToDash returns true if it is the default dash
        {
            dashDirection = Vector3.right;
        }
        else
        {
            SpriteManager.I.UpdateSpriteToDash(dashDirection);
        }

        /* The loop below ends up being a pseudo-update function. This is able to 
         * happen because of the yield return null; at the end of this while loop.
         * Every iteration makes the dTimeRemaining decrease until it is zero (or 
         * until the dash is completed), while the position of the player continues 
         * to move towards the direction */

        while (dTimeRemaining > 0)
        {
            dTimeRemaining -= Time.deltaTime;
            player.Move(PlayerStats.DashDistance * PlayerStats.DashSpeed * Time.deltaTime * dashDirection.normalized);
            yield return null;
        }

        //remainingCharges--; //since dash was performed, subtract a dash
        cantDash = false;

        SpriteManager.I.UpdateSpriteToWalk(dashDirection);
    }

    //cbt otherwise known as cock and ball torture

/*    public IEnumerator RechargeDash()
    {
        canRecharge = false;
        yield return new WaitForSeconds(dashChargeCooldown);
        remainingCharges = PlayerStats.DashCharges;
        DashUI.UpdateDashUI(remainingCharges);
        canRecharge = true;
    }*/


    public override void Anim(Animator anim, bool reset)
    {
        throw new System.NotImplementedException();
    }

    public override void ResetAttack()
    {

    }
    
}
