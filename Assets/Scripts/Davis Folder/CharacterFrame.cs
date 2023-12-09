using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

/***********************************************************************
 * NOTE: if you add any stats in the enum, it should be added into the 
 * Character frame and vice versa. Additionally, you should update the
 * "UpdateStat_GA" and add a case to the switch with the stat you added.
 ***********************************************************************/

public enum CharStats
{
    maxHealth,
    health,
    moveSpeed,
    dashSpeed,
    dashDistance,
    dashCharges,
    attackDamage,
    attackSpeed,
    cooldownMod,
    chargeLimit
}

public class CharacterFrame : MonoBehaviour
{
    private PInput pInput;
    
    [Tooltip("If true, the player is using controller. False, K&M.")]
    private static bool isController;
    [Header("Attacks and Abilities")]
    public Attack basicAttack;
    public Attack secondAttack;
    public Attack qAbility;
    public Attack eAbility;
    public Attack rAbility;
    public Attack fAbility;

    [Header("Stats")]
    [Tooltip("Maximum health of the player")]
    public int maxHealth;
    [Tooltip("Current health of the player")]
    public float health;
    private Health health_s;

    [Tooltip("How fast the player moves")]
    public float movementSpeed;
    [Tooltip("Speed of the dash")]
    public float dashSpeed;
    [Tooltip("How far the dash goes")]
    public float dashDistance;
    [Tooltip("How many charges the dash has")]
    public int dashCharges;
    private PlayerMovement move_s;

    [Tooltip("Base attack damage; each attack derives this for a calculation")]
    public float attackDamage;
    [Tooltip("Base attack speed; each attack derives this for a calculation")]
    public float attackSpeed;

    [Tooltip("Base cooldown modifier; each ability uses this for a calculation")]
    public float cooldownMod;
    [Tooltip("Limit the ultimate ability will charge to")]
    public int chargeLimit;
    [Tooltip("Current charge of the ultimate")]
    public float charge;

    [Header("Sprites")]
    public Animator character;
    public SpriteRenderer characterSprite;
    private CardinalDirection cachedDir;

    [Header("Managers")]
    public LevelManager transfer;
    public Health trueHealth;
    [Tooltip("The scriptable object of the default stats")]
    public readonly DefaultStats_SO defaultStats;

    Coroutine attacker = null;
    bool bIsPressed;

    //more options to come in the future
    private void Start()
    {
        health_s = GetComponent<Health>();
        move_s = GetComponent<PlayerMovement>();

        SaveData.wipe();
    }

    private void OnEnable()
    {
        pInput = new PInput();
        pInput.Enable();

        #region Started Subscriptions
        pInput.Player.BasicAttack.started += ctx => performAttack(basicAttack);
        pInput.Player.SecondAttack.started += ctx => performAttack(secondAttack);
        pInput.Player.Ability1.started += ctx => performAttack(qAbility);
        pInput.Player.Ability2.started += ctx => performAttack(eAbility);
        pInput.Player.Ability3.started += ctx => performAttack(rAbility);
        pInput.Player.Ult.started += ctx => performAttack(fAbility);
        #endregion

        #region Canceled Subscriptions
        pInput.Player.BasicAttack.canceled += ctx => NotPressed();
        pInput.Player.SecondAttack.canceled += ctx => NotPressed();
        pInput.Player.Ability1.canceled += ctx => NotPressed();
        pInput.Player.Ability2.canceled += ctx => NotPressed();
        pInput.Player.Ability3.canceled += ctx => NotPressed();
        pInput.Player.Ult.canceled += ctx => NotPressed();
        #endregion

        //pInput.Player.AnyController.performed += ctxt => 

        //AddScript_GA.ChangeAttackType += ReplaceAttack;
    }

    #region Attacks
    //  waits for the attack cooldown to finish
    IEnumerator attackWaiter(Attack curAttack) 
    {
        do{
            curAttack.attack();
            curAttack.anim(character, false);
            yield return new WaitForSeconds(curAttack.getRealCooldownTime());
            curAttack.reset();
        } while (bIsPressed);
        attacker = null;
    }

    public void UpdateAttack()
    {
        List<ConItem> activeItems = new();

        for(int i = 0; i < 3; i++)
        {
            activeItems.Add(Inventory.getActiveItem(i, FindFirstObjectByType<ItemLibrary>()));
        }


        foreach(ConItem item in activeItems)
        {
            if(item != null)
            {
                if(item.overrideAbil != Attack.attackType.None) //if it isn't a passive item
                {
                    ReplaceAttack(item.overrideAbil, item.attackScript);
                }
                else //if it is a passive item
                {
                    ReplacePassive(item.passiveScript);
                }
            }
            else
            {
                FixAbility();
            }
        }
    }

    /// <summary>
    /// If any abilities are null, this will set them to the base ability
    /// </summary>
    private void FixAbility()
    {
        if(basicAttack == null)
        {
            gameObject.AddComponent<GSword>();
            basicAttack = GetComponent<GSword>();
        }
        if(secondAttack == null)
        {
            gameObject.AddComponent<Handgun>();
            secondAttack = GetComponent<Handgun>();
        }
        if(eAbility == null)
        {

        }
        if(fAbility == null)
        {

        }
        if (rAbility == null)
        {

        }
        if (qAbility == null)
        {

        }
    }

    public void RemoveAbility(ConItem item)
    {
        switch (item.overrideAbil)
        {
            case Attack.attackType.Basic:
                Destroy(basicAttack);
                basicAttack = null;
                break;
            case Attack.attackType.Secondary:
                Destroy(secondAttack);
                secondAttack = null;
                break;
            case Attack.attackType.EAbility:
                Destroy(eAbility);
                eAbility = null;
                break;
            case Attack.attackType.FAbility:
                Destroy(fAbility);
                fAbility = null;
                break;
            case Attack.attackType.RAbility:
                Destroy(rAbility);
                rAbility = null;
                break;
            case Attack.attackType.QAbility:
                Destroy(qAbility);
                qAbility = null;
                break;
            case Attack.attackType.None:
                RemovePassive(item.passiveScript);
                break;
            default: //this should not happen
                Debug.LogError("Something real bad happened when removing an item.");
                break;
        }
        FixAbility();
    }

    private void ReplacePassive(PassiveScript pScript)
    {
        switch (pScript)
        {
            case PassiveScript.combatBoots:
                var script = gameObject.AddComponent<CombatBoots>();
                script.Equip(this);
                break;
            default:
                Debug.LogError("Passive could not be added.");
                break;
        }
    }

    private void RemovePassive(PassiveScript pScript)
    {
        switch (pScript)
        {
            case PassiveScript.combatBoots:
                var script = gameObject.GetComponent<CombatBoots>();
                script.UnEquip(this);
                break;
            default:
                Debug.LogError("Passive could not be removed.");
                break;
        }
    }

    private void ReplaceAttack(Attack.attackType aType, AttackScript aScript)
    {
        switch(aType)
        {
            case Attack.attackType.Basic:
                Destroy(basicAttack);
                break;
            case Attack.attackType.Secondary:
                Destroy(secondAttack);
                break;
            case Attack.attackType.EAbility:
                Destroy(eAbility);
                break;
            case Attack.attackType.FAbility:
                Destroy(fAbility);
                break;
            case Attack.attackType.RAbility:
                Destroy(rAbility);
                break;
            case Attack.attackType.QAbility:
                Destroy(qAbility);
                break;
            default: //this will also be none, cause this shouldn't happen
                break;
        }

        switch(aScript)
        {
            case AttackScript.handgun:
                gameObject.AddComponent<Handgun>();
                secondAttack = GetComponent<Handgun>();
                break;
            case AttackScript.shotgun:
                gameObject.AddComponent<Shotgun>();
                secondAttack = GetComponent<Shotgun>();
                break;
            default:
                Debug.LogError("Attack could not be replaced.");
                break;
        }
    }

    void performAttack(Attack curAttack) 
    {
        if(attacker != null) return;
        IsPressed();
            attacker = StartCoroutine(attackWaiter(curAttack));
    }
    void IsPressed()
    {
        bIsPressed = true;
    }

    void NotPressed()
    {
        bIsPressed = false;
    }
    #endregion

    #region Sprites
    public void UpdateSprite(Vector3 direction)
    {
        #region Sprite Setting
        if (direction.x > 0)
        {
            
            if (direction.z < 0) // SOUTHEAST
            {
                //characterSprite.sprite = southEast;
                character.SetTrigger("WalkSE");
                cachedDir = CardinalDirection.southEast;
            }
            else if (direction.z == 0) // EAST
            {
                character.SetTrigger("WalkE");
                cachedDir = CardinalDirection.east;
            }
            else // direction.z == 1 *** NORTHEAST
            {
                character.SetTrigger("WalkNE");
                cachedDir = CardinalDirection.northEast;
            }
        }
        else if (direction.x < 0)
        {
            if (direction.z < 0) // SOUTHWEST
            {
                //characterSprite.sprite = southWest;
                character.SetTrigger("WalkSW");
                cachedDir = CardinalDirection.southWest;
            }
            else if (direction.z == 0) // WEST
            {
                character.SetTrigger("WalkW");
                cachedDir = CardinalDirection.west;
            }
            else // direction.z == 1 *** NORTHWEST
            {
                //characterSprite.sprite = northWest;
                character.SetTrigger("WalkNW");
                cachedDir = CardinalDirection.northWest;
            }
        }
        else //direction.x == 0
        {
            if (direction.z < 0) // SOUTH
            {
                character.SetTrigger("WalkS");
                cachedDir = CardinalDirection.south;
            }
            else if (direction.z == 0) // NO INPUT
            {
                switch(cachedDir.ToString())
                {
                    case "north":
                        character.SetTrigger("WalkNStop");
                        break;
                    case "northEast":
                        character.SetTrigger("WalkNEStop");
                        break;
                    case "northWest":
                        character.SetTrigger("WalkNWStop");
                        break;
                    case "south":
                        character.SetTrigger("WalkSStop");
                        break;
                    case "southEast":
                        character.SetTrigger("WalkSEStop");
                        break;
                    case "southWest":
                        character.SetTrigger("WalkSWStop");
                        break;
                    case "east":
                        character.SetTrigger("WalkEStop");
                        break;
                    case "west":
                        character.SetTrigger("WalkWStop");
                        break;
                    default:
                        break;
                }
            }
            else // direction.z == 1 *** NORTH
            {
                character.SetTrigger("WalkN");
                cachedDir = CardinalDirection.north;
            }
        }
        #endregion
    }

    /// <summary>
    /// Only use this for the default direction.
    /// </summary>
    /// <param name="cachedDir">Should be CardinalDirection.east</param>
    public void UpdateSprite(CardinalDirection cachedDir)
    {
        switch (cachedDir.ToString())
        {
            case "north":
                character.SetTrigger("WalkNStop");
                break;
            case "northEast":
                character.SetTrigger("WalkNEStop");
                break;
            case "northWest":
                character.SetTrigger("WalkNWStop");
                break;
            case "south":
                character.SetTrigger("WalkSStop");
                break;
            case "southEast":
                character.SetTrigger("WalkSEStop");
                break;
            case "southWest":
                character.SetTrigger("WalkSWStop");
                break;
            case "east":
                character.SetTrigger("WalkEStop");
                break;
            case "west":
                character.SetTrigger("WalkWStop");
                break;
            default:
                break;
        }
    }
    #endregion

    #region Controller Setting

    private void ChangeMediums()
    {

    }

    #endregion

    private void OnDisable()
    {
        pInput.Disable();
        StopAllCoroutines();

        #region Started Subscriptions
        pInput.Player.BasicAttack.started -= ctx => performAttack(basicAttack);
        pInput.Player.SecondAttack.started -= ctx => performAttack(secondAttack);
        pInput.Player.Ability1.started -= ctx => performAttack(qAbility);
        pInput.Player.Ability2.started -= ctx => performAttack(eAbility);
        pInput.Player.Ability3.started -= ctx => performAttack(rAbility);
        pInput.Player.Ult.started -= ctx => performAttack(fAbility);
        #endregion

        #region Canceled Subscriptions
        pInput.Player.BasicAttack.canceled -= ctx => NotPressed();
        pInput.Player.SecondAttack.canceled -= ctx => NotPressed();
        pInput.Player.Ability1.canceled -= ctx => NotPressed();
        pInput.Player.Ability2.canceled -= ctx => NotPressed();
        pInput.Player.Ability3.canceled -= ctx => NotPressed();
        pInput.Player.Ult.canceled -= ctx => NotPressed();
        #endregion

        //AddScript_GA.ChangeAttackType -= ReplaceAttack;
    }
    //tree to execute each respective attack


    public void UpdateStats()
    {
        move_s.UpdateStats();
        health_s.UpdateStats();
    }

    public void UpdateStats(Attack atk)
    {
        atk.updateStats(attackDamage, cooldownMod);
    }

    bool dumbo = true;
    public void Update()
    {       
        if(trueHealth.health <= 0)
        {
            if (dumbo)
            {
                AkSoundEngine.PostEvent("Player_Death", gameObject);
                transfer.playerDeath();
                dumbo = false;
            }
        }
    }
}
public enum CardinalDirection
{
    north, south, east, west, northEast, northWest, southEast, southWest
}