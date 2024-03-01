using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    [Header("Attacks and Abilities")]
    public Attack meleeAttack;
    public Attack rangedAttack;
    public Attack specialAttack;

    [Header("Player UI Canvases (DO NOT CHANGE)")]
    public List<PlayerUICanvas> puiCanvases;

    [Tooltip("Base attack damage; each attack derives this for a calculation")]
    public float attackDamage => PlayerStats.AttackDamage;
    [Tooltip("Base attack speed; each attack derives this for a calculation")]
    public float attackSpeed => PlayerStats.AttackSpeed;

    [Tooltip("Base cooldown modifier; each ability uses this for a calculation")]
    public float cooldownMod => PlayerStats.CooldownMod;
    [Tooltip("Limit the ultimate ability will charge to")]
    public int chargeLimit => PlayerStats.ChargeLimit;
    [Tooltip("Current charge of the ultimate")]
    public float charge => PlayerStats.Charge;

    private PInput pInput;
    private bool bIsMainPressed;
    private bool bIsAbilPressed;
    private bool bIsMainAttacking; //is true when a main attack is happening
    private bool bIsAbilAttacking; //is true when an ability is happening

    public bool canAttack = true;

    //this dictionary is used to store which attackType each AttackScript is. This is so I don't have to make
    //  a massive switch statement for every single attack script in a function down below.
    private readonly Dictionary<AttackScript, Attack.attackType> attackTypeDict = new()
    {
        { AttackScript.Handgun, Attack.attackType.Ranged },
        { AttackScript.Shotgun, Attack.attackType.Ranged },
        { AttackScript.Sword, Attack.attackType.Melee },
        { AttackScript.Rocket, Attack.attackType.Special },
        { AttackScript.None, Attack.attackType.None}

    };
    
    public void OnEnable()
    {
        //SaveData.wipe(); //this is for debugging purposes, DO NOT HAVE THIS ON AT ALL TIMES
        pInput = new PInput();
        pInput.Enable();

        /*
         - gun and sword can't swing together
         - rocket could go at anytime
        */

        #region Attack Subscriptions

        if (FindObjectOfType<TutorialChecker>() == null)
        {
            #region Started Subscriptions
            pInput.Player.BasicAttack.started += ctx => PerformAttack(meleeAttack, true);
            pInput.Player.SecondAttack.started += ctx => PerformAttack(rangedAttack, true);
            pInput.Player.Ability1.started += ctx => PerformAttack(specialAttack, false);
            #endregion

            #region Canceled Subscriptions
            pInput.Player.BasicAttack.canceled += ctx => SetMainPressed(false);
            pInput.Player.SecondAttack.canceled += ctx => SetMainPressed(false);
            pInput.Player.Ability1.canceled += ctx => SetAbilPressed(false);
            pInput.Player.Ability2.canceled += ctx => SetAbilPressed(false);
            pInput.Player.Ability3.canceled += ctx => SetAbilPressed(false);
            pInput.Player.Ult.canceled += ctx => SetAbilPressed(false);
            #endregion
        }
        //  subs that the tutorial doesn't handle so just subscribe on start
        else
        {
            #region Started Subscriptions
            #endregion

            #region Canceled Subscriptions
            #endregion
        }
        #endregion
    }

    #region Performing Attacks

    /// <summary>
    /// Used whenever an attack is needed to be performed.
    /// </summary>
    /// <param name="attack">The attack from the AttackManager going to be performed</param>
    /// <param name="isMainAttack">If True, attack is either basicAttack or secondAttack</param>
    private void PerformAttack(Attack attack, bool isMainAttack)
    {
        if(!canAttack) return;
        if(isMainAttack && !bIsMainAttacking)     //is main attack
        {
            SetMainPressed(true);
            bIsMainAttacking = true;
            StartCoroutine( MainAttackWaiter(attack) );
        }
        else if (!isMainAttack && !bIsAbilAttacking)               //is an ability attack
        {
            SetAbilPressed(true);
            bIsAbilAttacking = true;
            StartCoroutine( SpecialAttackWaiter(attack) );
        }
    }

    private void ActivatePUI(Attack attack)
    {
        switch(attack.getAttackType())
        {
            case Attack.attackType.Melee: //sword
                puiCanvases[0].updateSlider(attack.getRealCooldownTime());
                break;
            case Attack.attackType.Ranged: //gun
                puiCanvases[1].updateSlider(attack.getRealCooldownTime());
                break;
            case Attack.attackType.Special: //rocket
                puiCanvases[2].updateSlider(attack.getRealCooldownTime());
                break;
            default:
                Debug.Log($"aType of attack is not a sword, pistol, or rocket.");
                break;
        }
    }

    IEnumerator MainAttackWaiter(Attack curAttack)
    {
        do
        {
            ActivatePUI(curAttack);
            curAttack.attack();
            //curAttack.anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.getRealCooldownTime());
            curAttack.reset();
        } while (bIsMainPressed);
        bIsMainAttacking = false;
    }

    IEnumerator SpecialAttackWaiter(Attack curAttack)
    {
        do
        {
            ActivatePUI(curAttack);
            curAttack.attack();
            //curAttack.anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.getRealCooldownTime());
            curAttack.reset();
        } while (bIsAbilPressed);
        bIsAbilAttacking = false;
    }

    /// <summary>
    /// Sets bIsPressed to be either true or false depending on the parameters.
    /// </summary>
    /// <param name="newValue">value that bIsPressed will be set to</param>
    private void SetMainPressed(bool newValue)
    {
        bIsMainPressed = newValue;
    }

    private void SetAbilPressed(bool newValue)
    {
        bIsAbilPressed = newValue;
    }

    #endregion

    #region Updating Attacks
    public void UpdateAttack()
    {
        List<ConItem> activeItems = new();

        for (int i = 0; i < 3; i++)
        {
            activeItems.Add(Inventory.getActiveItem(i, FindFirstObjectByType<AugmentLibrary>()));
        }

        foreach (ConItem item in activeItems)
        {
            if (item != null)
            {
                if (item.overrideAbil != Attack.attackType.None) //if it isn't a passive item
                {
                    ReplaceAttack(item.attackScript);
                }
                else //if it is an augment
                {
                    //ReplaceAugment(item.passiveScript);
                }
            }
            else
            {
                FixAbility();
            }
        }
    }

    public Attack.attackType GetAttackType(AttackScript aScript)
    {
        return attackTypeDict[aScript];
    }

    public AttackScript GetAttackScript(Attack.attackType aType)
    {
        switch(aType)
        {
            case Attack.attackType.Melee:
                return meleeAttack.AScript;

            case Attack.attackType.Ranged:
                return rangedAttack.AScript;

            case Attack.attackType.Special:
                return specialAttack.AScript;

            case Attack.attackType.Movement:
                Debug.LogError("Tried to query an ability slot that doesn't exist (Movement)");
                return AttackScript.None;

            default: //Attack.attackType.None
                Debug.LogError("Tried to query an ability slot that doesn't exist (None)");
                return AttackScript.None;
        }
    }

    public void UpdateAttack(ConItem item)
    {
        if (item != null)
        {
            if (item.overrideAbil != Attack.attackType.None) //if it isn't an augment
            {
                ReplaceAttack(item.attackScript);
            }
            else //if it is an augment
            {
                //ReplaceAugment(item.passiveScript);
            }
        }
        else
        {
            FixAbility();
        }
    }


    /// <summary>
    /// If any abilities are null, this will set them to the base ability
    /// </summary>
    private void FixAbility()
    {
        if (meleeAttack == null)
        {
            gameObject.AddComponent<GSword>();
            meleeAttack = GetComponent<GSword>();
        }
        if (rangedAttack == null)
        {
            gameObject.AddComponent<Handgun>();
            rangedAttack = GetComponent<Handgun>();
        }
        if (specialAttack == null)
        {
            gameObject.AddComponent<Handgun>();
            rangedAttack = GetComponent<Handgun>();
        }
    }

/*    private void ReplaceAugment(PassiveScript pScript)
    {
        switch (pScript)
        {
            case PassiveScript.combatBoots:
                var script = gameObject.AddComponent<CombatBoots>();
                script.Equip();
                break;
            default:
                Debug.LogError("Passive could not be added.");
                break;
        }
    }
*/


    public void ReplaceAttack(AttackScript aScript)
    {
        switch (aScript)
        {
            case AttackScript.Handgun:
                gameObject.AddComponent<Handgun>();
                rangedAttack = GetComponent<Handgun>();
                break;
            case AttackScript.Shotgun:
                gameObject.AddComponent<Shotgun>();
                rangedAttack = GetComponent<Shotgun>();
                break;
            default:
                Debug.LogError("Attack could not be replaced.");
                break;
        }
    }
    #endregion

    #region Tutorial-Specific Subscriptions
    //  function for Connor's tutorial
    public void activateSubscription(TutorialChecker.teachTypes type)
    {
        switch (type)
        {
            case TutorialChecker.teachTypes.BasicAtt:
                pInput.Player.BasicAttack.started += ctx => PerformAttack(meleeAttack, true);
                pInput.Player.BasicAttack.canceled += ctx => SetMainPressed(false);
                break;
            case TutorialChecker.teachTypes.SecondAtt:
                pInput.Player.SecondAttack.started += ctx => PerformAttack(rangedAttack, true);
                pInput.Player.SecondAttack.canceled += ctx => SetMainPressed(false);
                break;
            case TutorialChecker.teachTypes.RocketAtt:
                pInput.Player.Ability1.started += ctx => PerformAttack(specialAttack, false);
                pInput.Player.Ability1.canceled += ctx => SetAbilPressed(false);
                break;
        }
    }
    #endregion

    public void OnDisable()
    {
        pInput.Disable();
    }
}
