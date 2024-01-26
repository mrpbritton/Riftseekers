using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("Attacks and Abilities")]
    public Attack basicAttack;
    public Attack secondAttack;
    public Attack qAbility;
    public Attack eAbility;
    public Attack rAbility;
    public Attack fAbility;

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
    private bool bIsPressed;
    private bool bIsMainAttacking; //is true when a main attack is happening
    private bool bIsAbilAttacking; //is true when an ability is happening

    public void OnEnable()
    {
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
            pInput.Player.BasicAttack.started += ctx => PerformAttack(basicAttack, true);
            pInput.Player.SecondAttack.started += ctx => PerformAttack(secondAttack, true);
            pInput.Player.Ability1.started += ctx => PerformAttack(qAbility, false);
            pInput.Player.Ability2.started += ctx => PerformAttack(eAbility, false);
            pInput.Player.Ability3.started += ctx => PerformAttack(rAbility, false);
            pInput.Player.Ult.started += ctx => PerformAttack(fAbility, false);
            #endregion

            #region Canceled Subscriptions
            pInput.Player.BasicAttack.canceled += ctx => SetPressed(false);
            pInput.Player.SecondAttack.canceled += ctx => SetPressed(false);
            pInput.Player.Ability1.canceled += ctx => SetPressed(false);
            pInput.Player.Ability2.canceled += ctx => SetPressed(false);
            pInput.Player.Ability3.canceled += ctx => SetPressed(false);
            pInput.Player.Ult.canceled += ctx => SetPressed(false);
            #endregion
        }
        //  subs that the tutorial doesn't handle so just subscribe on start
        else
        {
            #region Started Subscriptions
            pInput.Player.Ability2.started += ctx => PerformAttack(eAbility, false);
            pInput.Player.Ability3.started += ctx => PerformAttack(rAbility, false);
            pInput.Player.Ult.started += ctx => PerformAttack(fAbility, false);
            #endregion

            #region Canceled Subscriptions
            pInput.Player.Ability2.canceled += ctx => SetPressed(false);
            pInput.Player.Ability3.canceled += ctx => SetPressed(false);
            pInput.Player.Ult.canceled += ctx => SetPressed(false);
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
        if(isMainAttack && !bIsMainAttacking)     //is main attack
        {
            SetPressed(true);
            bIsMainAttacking = true;
            StartCoroutine( MainAttackWaiter(attack) );
        }
        else if (!isMainAttack && !bIsAbilAttacking)               //is an ability attack
        {
            SetPressed(true);
            bIsMainAttacking = true;
            StartCoroutine( SpecialAttackWaiter(attack) );
        }
    }

    IEnumerator MainAttackWaiter(Attack curAttack)
    {
        do
        {
            curAttack.attack();
            //curAttack.anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.getRealCooldownTime());
            curAttack.reset();
        } while (bIsPressed);
        bIsMainAttacking = false;
    }

    IEnumerator SpecialAttackWaiter(Attack curAttack)
    {
        do
        {
            curAttack.attack();
            //curAttack.anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.getRealCooldownTime());
            curAttack.reset();
        } while (bIsPressed);
        bIsAbilAttacking = false;
    }

    /// <summary>
    /// Sets bIsPressed to be either true or false depending on the parameters.
    /// </summary>
    /// <param name="newValue">value that bIsPressed will be set to</param>
    private void SetPressed(bool newValue)
    {
        bIsPressed = newValue;
    }

    #endregion

    #region Updating Attacks
    public void UpdateAttack()
    {
        List<ConItem> activeItems = new();

        for (int i = 0; i < 3; i++)
        {
            activeItems.Add(Inventory.getActiveItem(i, FindFirstObjectByType<ItemLibrary>()));
        }


        foreach (ConItem item in activeItems)
        {
            if (item != null)
            {
                if (item.overrideAbil != Attack.attackType.None) //if it isn't a passive item
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

    public void UpdateAttack(ConItem item)
    {
        if (item != null)
        {
            if (item.overrideAbil != Attack.attackType.None) //if it isn't a passive item
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

    public void ResetAttack()
    {
        List<ConItem> activeItems = new();
        for (int i = 0; i < 3; i++)
        {
            activeItems.Add(Inventory.getActiveItem(i, FindFirstObjectByType<ItemLibrary>()));
        }
        foreach (ConItem item in activeItems)
        {
            if (item != null)
            {
                RemoveAbility(item);
            }
        }
    }


    /// <summary>
    /// If any abilities are null, this will set them to the base ability
    /// </summary>
    private void FixAbility()
    {
        if (basicAttack == null)
        {
            gameObject.AddComponent<GSword>();
            basicAttack = GetComponent<GSword>();
        }
        if (secondAttack == null)
        {
            gameObject.AddComponent<Handgun>();
            secondAttack = GetComponent<Handgun>();
        }
        if (eAbility == null)
        {

        }
        if (fAbility == null)
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
                script.Equip();
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
                script.UnEquip();
                Destroy(this);
                break;
            default:
                Debug.LogError("Passive could not be removed.");
                break;
        }
    }

    private void ReplaceAttack(Attack.attackType aType, AttackScript aScript)
    {
        switch (aType)
        {
            case Attack.attackType.Basic:
                if (basicAttack != null)
                    Destroy(basicAttack);
                break;
            case Attack.attackType.Secondary:
                if (secondAttack != null)
                    Destroy(secondAttack);
                break;
            case Attack.attackType.EAbility:
                if (eAbility != null)
                    Destroy(eAbility);
                break;
            case Attack.attackType.FAbility:
                if (fAbility != null)
                    Destroy(fAbility);
                break;
            case Attack.attackType.RAbility:
                if (rAbility != null)
                    Destroy(rAbility);
                break;
            case Attack.attackType.QAbility:
                if (qAbility != null)
                    Destroy(qAbility);
                break;
            default: //this will also be none, cause this shouldn't happen
                break;
        }

        switch (aScript)
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
    #endregion

    #region Tutorial-Specific Subscriptions
    //  function for Connor's tutorial
    public void activateSubscription(TutorialChecker.teachTypes type)
    {
        switch (type)
        {
            case TutorialChecker.teachTypes.BasicAtt:
                pInput.Player.BasicAttack.started += ctx => PerformAttack(basicAttack, true);
                pInput.Player.BasicAttack.canceled += ctx => SetPressed(false);
                break;
            case TutorialChecker.teachTypes.SecondAtt:
                pInput.Player.SecondAttack.started += ctx => PerformAttack(secondAttack, true);
                pInput.Player.SecondAttack.canceled += ctx => SetPressed(false);
                break;
            case TutorialChecker.teachTypes.RocketAtt:
                pInput.Player.Ability1.started += ctx => PerformAttack(qAbility, false);
                pInput.Player.Ability1.canceled += ctx => SetPressed(false);
                break;
        }
    }
    #endregion

    public void OnDisable()
    {
        pInput.Disable();
    }
}
