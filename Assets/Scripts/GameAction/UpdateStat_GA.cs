using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateStat_GA : GameAction
{
    [SerializeField, Tooltip("Player's Frame")]
    private CharacterFrame frame;
    [SerializeField, Tooltip("Stat to be changed")]
    private CharStats stat;
    [SerializeField, Tooltip("Change in the stat")]
    private float modifier;
    public override void Action()
    {
        if (frame == null)
        {
            //finds the player if the frame isn't set, finds the player, then gets it's frame
            frame = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterFrame>();
        }

        /*    maxHealth,
    health,
    moveSpeed,
    dashSpeed,
    dashDistance,
    attackDamage,
    attackSpeed,
    cooldownMod,
    chargeLimit*/

        switch (stat)
        {
            case CharStats.maxHealth:
                frame.maxHealth += Mathf.FloorToInt(modifier);
                CharacterFrame.Restat();
                break;

            case CharStats.health:
                frame.health += modifier;
                CharacterFrame.Restat();
                break;

            case CharStats.moveSpeed:
                frame.movementSpeed += modifier;
                CharacterFrame.Restat();
                break;
            
            case CharStats.dashSpeed:
                frame.dashSpeed += modifier;
                CharacterFrame.Restat();
                break;

            case CharStats.dashDistance:
                frame.dashDistance += modifier;
                CharacterFrame.Restat();
                break;

            case CharStats.attackDamage:
                frame.attackDamage += modifier;
                CharacterFrame.Restat();
                break;

            case CharStats.attackSpeed:
                frame.attackSpeed += modifier;
                CharacterFrame.Restat();
                break;
            
            case CharStats.cooldownMod:
                frame.cooldownMod += modifier;
                CharacterFrame.Restat();
                break;

            case CharStats.chargeLimit:
                frame.chargeLimit += Mathf.FloorToInt(modifier);
                CharacterFrame.Restat();
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
