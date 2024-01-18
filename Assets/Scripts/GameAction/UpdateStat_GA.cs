using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UpdateStat_GA : GameAction
{
    [Tooltip("Stat to be changed")]
    public CharStats stat;
    [Tooltip("Change in the stat")]
    public float modifier;

    public AttackManager attackManager;
    public PlayerMovement pMovement;
    public Health pHealth;

    public override void Action()
    {

            /*  maxHealth,
                health,
                moveSpeed,
                dashSpeed,
                dashDistance,
                dashCharges,
                attackDamage,
                attackSpeed,
                cooldownMod,
                chargeLimit */

        switch (stat)
        {
            case CharStats.maxHealth:
                PlayerStats.UpdateMaxHealth += Mathf.FloorToInt(modifier);
                break;

            case CharStats.health:
                PlayerStats.UpdateHealth += modifier;
                break;

            case CharStats.moveSpeed:
                PlayerStats.UpdateMovementSpeed += modifier;
                break;
            
            case CharStats.dashSpeed:
                PlayerStats.UpdateDashSpeed += modifier;
                break;

            case CharStats.dashDistance:
                PlayerStats.UpdateDashDistance += modifier;
                break;

            case CharStats.dashCharges:
                PlayerStats.UpdateDashCharges += Mathf.FloorToInt(modifier);
                break;

            case CharStats.attackDamage:
                PlayerStats.UpdateAttackDamage += modifier;
                break;

            case CharStats.attackSpeed:
                PlayerStats.UpdateAttackSpeed += modifier;
                break;
            
            case CharStats.cooldownMod:
                PlayerStats.UpdateCooldownMod += modifier;
                break;

            case CharStats.chargeLimit:
                PlayerStats.UpdateChargeLimit += Mathf.FloorToInt(modifier);
                break;

            default:
                Debug.LogError("Stat could not be changed.");
                break;
        }

    }
    
    /// <summary>
    /// This should not be used.
    /// </summary>
    public override void DeAction()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// This should not be used.
    /// </summary>
    public override void ResetToDefault()
    {
        throw new System.NotImplementedException();
    }
}
