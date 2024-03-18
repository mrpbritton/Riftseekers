using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : Attack
{
    /**Attack Inheritance Properties**/
    public override AttackScript AScript => AttackScript.Dash;
    public override AttackType AType => AttackType.Movement;
    protected override float SetDamage => 0f;
    protected override float SetCooldownTime => 3f;
    /*********************************/
    
    [HideInInspector]
    public bool cantDash; //whether or not the player can dash
    private bool isDefDash;
    private Vector3 dashDirection;
    public static Transform playerTrans;
    private CharacterController player;

    private void OnEnable()
    {
        player = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
    }

    public override void DoAttack() //action
    {
        if (cantDash == true) return;
        StartCoroutine(DoDash());
    }

    IEnumerator DoDash()
    {
        //Debug.Log($"{PlayerStats.DashDistance}, {PlayerStats.DashSpeed}");
        AkSoundEngine.PostEvent("Dash", gameObject);

        cantDash = true;

        dashDirection = PlayerMovement.GetDirection();
        if (SpriteManager.I.UpdateSpriteToDash(dashDirection)) //UpdateSpriteToDash returns true if it is the default dash
        {
            dashDirection = Vector3.right;
            isDefDash = true;
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

        //this finds how much time it would take given the current DashDistance and DashSpeed to go that far.
        float dTimeRemaining = Mathf.Sqrt(2*PlayerStats.DashDistance/PlayerStats.DashSpeed);
        
        while (dTimeRemaining > 0)
        {
            dTimeRemaining -= Time.deltaTime;
            player.Move(PlayerStats.DashSpeed * Time.deltaTime * dashDirection.normalized);
            yield return null;
        }

        cantDash = false;
        
        SpriteManager.I.UpdateSpriteToWalk(dashDirection);    
        if(isDefDash)
        {
            SpriteManager.I.UpdateSpriteToIdle(dashDirection);
            isDefDash = false;
        }
    }

    //cbt otherwise known as cock and ball torture
    public override void Anim(Animator anim, bool reset)
    {
        if(reset)
        {

        }
    }

    public override void ResetAttack()
    {

    }
    
}
