using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    [Header("Attacks and Abilities")]
    public Attack meleeAttack;
    public Attack rangedAttack;
    public Attack specialAttack;
    public Attack movementAttack;
    
    [Header("Animator used for the Character")]
    public Animator character;

    [Header("Bullet Prefab"), Tooltip("Prefab used for Ranged Attack")]
    public GameObject bullet;

    private PInput pInput;
    bool mainAttackCooled = true, secondaryAttackCooled = true, abilAttackCooled = true, moveAttackCooled = true;
    Coroutine bMainAttacker, bSecondaryAttacker, bAbilAttacker, bMoveAttacker;
    
    public bool canAttack = true;

    //this dictionary is used to store which attackType each AttackScript is. This is so I don't have to make
    //  a massive switch statement for every single attack script in a function down below.
    public readonly Dictionary<AttackScript, Attack.AttackType> attackTypeDict = new()
    {
        { AttackScript.Handgun, Attack.AttackType.Ranged },
        { AttackScript.Shotgun, Attack.AttackType.Ranged },
        { AttackScript.Sword, Attack.AttackType.Melee },
        { AttackScript.Rocket, Attack.AttackType.Special },
        { AttackScript.Dash, Attack.AttackType.Movement },
        { AttackScript.None, Attack.AttackType.None}

    };
    
    public void Start()
    {
        //SaveData.wipe(); //this is for debugging purposes, DO NOT HAVE THIS ON AT ALL TIMES
        pInput = new PInput();
        pInput.Enable();
        /*
         - gun and sword can't swing together
         - rocket could go at anytime
         - dash could go at anytime
        */

        Inventory.loadInventory();
        UpdateAttack();
        //Inventory.addItem(AugmentLibrary.I.FindItem(meleeAttack.AScript));
        if(Inventory.getItems(AugmentLibrary.I).Count == 0)
            Inventory.addItem(AugmentLibrary.I.FindItem(rangedAttack.AScript));
        //Inventory.addItem(AugmentLibrary.I.FindItem(specialAttack.AScript));
        //Inventory.addItem(AugmentLibrary.I.FindItem(movementAttack.AScript));
        Inventory.saveInventory();
        #region Attack Subscriptions

        if (FindObjectOfType<TutorialChecker>() == null || true)
        {
            #region Started Subscriptions
            pInput.Player.BasicAttack.started += ctx => PerformAttack(meleeAttack, Attack.AttackType.Melee);
            pInput.Player.SecondAttack.started += ctx => PerformAttack(rangedAttack, Attack.AttackType.Ranged);
            pInput.Player.Ability1.started += ctx => PerformAttack(specialAttack, Attack.AttackType.Special);
            pInput.Player.Dash.started += ctx => PerformAttack(movementAttack, Attack.AttackType.Movement);
            #endregion

            #region Canceled Subscriptions
            pInput.Player.BasicAttack.canceled += ctx => { bMainAttacker = null; };
            pInput.Player.SecondAttack.canceled += ctx => { bSecondaryAttacker = null; };
            pInput.Player.Ability1.canceled += ctx => { bAbilAttacker = null; };
            pInput.Player.Ability2.canceled += ctx => { bAbilAttacker = null; };
            pInput.Player.Ability3.canceled += ctx => { bAbilAttacker = null; };
            pInput.Player.Dash.canceled += ctx => { bMoveAttacker = null; };    //  this one's different
            pInput.Player.Ult.canceled += ctx => { bAbilAttacker = null; };
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
    /// <param name="aType">Which attack is being performed</param>
    private void PerformAttack(Attack attack, Attack.AttackType aType)
    {
        if(!canAttack) return;
        if(aType == Attack.AttackType.Melee && bMainAttacker == null && mainAttackCooled)
        {
            bMainAttacker = StartCoroutine( MainAttackWaiter(attack, false) );
        }
        if(aType == Attack.AttackType.Ranged && bSecondaryAttacker == null && secondaryAttackCooled)
        {
            bSecondaryAttacker = StartCoroutine( MainAttackWaiter(attack, true) );
        }
        else if (aType == Attack.AttackType.Special && bAbilAttacker == null && abilAttackCooled)
        {
            bAbilAttacker = StartCoroutine( SpecialAttackWaiter(attack) );
        }
        else if(aType == Attack.AttackType.Movement && bMoveAttacker == null && moveAttackCooled)
        {
            bMoveAttacker = StartCoroutine( MovementAttackWaiter(attack) );
        }
    }

    #region Attack Waiters
    IEnumerator MainAttackWaiter(Attack curAttack, bool secondary)
    {
        do
        {
            while(secondary && bMainAttacker != null)
                yield return new WaitForFixedUpdate();
            if(UIManager.I != null)
                UIManager.I.ActivatePUI(curAttack);
            curAttack.DoAttack();
            if(secondary)
                secondaryAttackCooled = false;
            else
                mainAttackCooled = false;
            //curAttack.Anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.GetCooldownTime());
            curAttack.ResetAttack();
            if(secondary)
                secondaryAttackCooled = true;
            else
                mainAttackCooled = true;
        } while (secondary ? bSecondaryAttacker != null : bMainAttacker != null);
    }

    IEnumerator SpecialAttackWaiter(Attack curAttack)
    {
        do {
            if(UIManager.I != null)
                UIManager.I.ActivatePUI(curAttack);
            curAttack.DoAttack();
            abilAttackCooled = false;
            //curAttack.Anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.GetCooldownTime());
            curAttack.ResetAttack();
            abilAttackCooled = true;
        } while (bAbilAttacker != null);
    }

    IEnumerator MovementAttackWaiter(Attack curAttack)
    {
        do {
            if(UIManager.I != null)
                UIManager.I.ActivatePUI(curAttack);
            curAttack.DoAttack();
            curAttack.Anim(character, false); //THIS IS THE ANIMATOR
            moveAttackCooled = false;
            yield return new WaitForSeconds(curAttack.GetCooldownTime());
            moveAttackCooled = true;
            curAttack.ResetAttack();
        } while (bMoveAttacker != null);
        curAttack.Anim(character, true);
    }
    #endregion
    #endregion

    #region Updating Attacks
    public void UpdateAttack()
    {
        foreach (ConItem item in Inventory.getItems(FindObjectOfType<AugmentLibrary>()))
        {
            if (item != null)
            {
                if (item.overrideAbil != Attack.AttackType.None) //if it isn't a passive item
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

    public Attack.AttackType GetAttackType(AttackScript aScript)
    {
        return attackTypeDict[aScript];
    }

    public AttackScript GetAttackScript(Attack.AttackType aType)
    {
        switch(aType)
        {
            case Attack.AttackType.Melee:
                return meleeAttack.AScript;

            case Attack.AttackType.Ranged:
                return rangedAttack.AScript;

            case Attack.AttackType.Special:
                return specialAttack.AScript;

            case Attack.AttackType.Movement:
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
            if (item.overrideAbil != Attack.AttackType.None) //if it isn't an augment
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

    public void OnDisable()
    {
        pInput.Disable();
    }
}
