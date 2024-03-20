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

    [Header("Player UI Canvases (DO NOT CHANGE)")]
    public List<PlayerUICanvas> puiCanvases;
    
    [Header("Animator used for the Character")]
    public Animator character;

    private PInput pInput;
    private bool bIsMainPressed;
    private bool bIsAbilPressed;
    private bool bIsMovePressed;
    private bool bIsMainAttacking; //is true when a main attack is happening
    private bool bIsAbilAttacking; //is true when a special ability is happening
    private bool bIsMoveAttacking; //is true when a movement ability is happening
    
    public bool canAttack = true;

    //this dictionary is used to store which attackType each AttackScript is. This is so I don't have to make
    //  a massive switch statement for every single attack script in a function down below.
    private readonly Dictionary<AttackScript, Attack.AttackType> attackTypeDict = new()
    {
        { AttackScript.Handgun, Attack.AttackType.Ranged },
        { AttackScript.Shotgun, Attack.AttackType.Ranged },
        { AttackScript.Sword, Attack.AttackType.Melee },
        { AttackScript.Rocket, Attack.AttackType.Special },
        { AttackScript.Dash, Attack.AttackType.Movement },
        { AttackScript.None, Attack.AttackType.None}

    };
    
    public void OnEnable()
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
        Inventory.addItem(AugmentLibrary.I.FindItem(rangedAttack.AScript));
        //Inventory.addItem(AugmentLibrary.I.FindItem(specialAttack.AScript));
        //Inventory.addItem(AugmentLibrary.I.FindItem(movementAttack.AScript));
        Inventory.saveInventory();
        #region Attack Subscriptions

        if (FindObjectOfType<TutorialChecker>() == null)
        {
            #region Started Subscriptions
            pInput.Player.BasicAttack.started += ctx => PerformAttack(meleeAttack, Attack.AttackType.Melee);
            pInput.Player.SecondAttack.started += ctx => PerformAttack(rangedAttack, Attack.AttackType.Ranged);
            pInput.Player.Ability1.started += ctx => PerformAttack(specialAttack, Attack.AttackType.Special);
            pInput.Player.Dash.started += ctx => PerformAttack(movementAttack, Attack.AttackType.Movement);
            #endregion

            #region Canceled Subscriptions
            pInput.Player.BasicAttack.canceled += ctx => SetMainPressed(false);
            pInput.Player.SecondAttack.canceled += ctx => SetMainPressed(false);
            pInput.Player.Ability1.canceled += ctx => SetAbilPressed(false);
            pInput.Player.Ability2.canceled += ctx => SetAbilPressed(false);
            pInput.Player.Ability3.canceled += ctx => SetAbilPressed(false);
            pInput.Player.Dash.canceled += ctx => SetMovePressed(false);
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
    /// <param name="aType">Which attack is being performed</param>
    private void PerformAttack(Attack attack, Attack.AttackType aType)
    {
        if(!canAttack) return;
        if((aType == Attack.AttackType.Melee || aType == Attack.AttackType.Ranged) && !bIsMainAttacking)
        {
            SetMainPressed(true);
            bIsMainAttacking = true;
            StartCoroutine( MainAttackWaiter(attack) );
        }
        else if (aType == Attack.AttackType.Special && !bIsAbilAttacking)
        {
            SetAbilPressed(true);
            bIsAbilAttacking = true;
            StartCoroutine( SpecialAttackWaiter(attack) );
        }
        else if(aType == Attack.AttackType.Movement && !bIsMoveAttacking)
        {
            SetMovePressed(true);
            bIsMoveAttacking = true;
            StartCoroutine( MovementAttackWaiter(attack) );
        }
    }

    private void ActivatePUI(Attack attack)
    {
        switch(attack.AType)
        {
            case Attack.AttackType.Melee: //sword
                puiCanvases[0].updateSlider(attack.GetCooldownTime());
                break;
            case Attack.AttackType.Ranged: //gun
                puiCanvases[1].updateSlider(attack.GetCooldownTime());
                break;
            case Attack.AttackType.Special: //rocket
                puiCanvases[2].updateSlider(attack.GetCooldownTime());
                break;
            case Attack.AttackType.Movement: //dash
                puiCanvases[3].updateSlider(attack.GetCooldownTime());
                break;
            default:
                Debug.Log($"aType of attack is not melee, ranged, special, or movement. Check that it isn't \"None\".");
                break;
        }
    }

    IEnumerator MainAttackWaiter(Attack curAttack)
    {
        do
        {
            ActivatePUI(curAttack);
            curAttack.DoAttack();
            //curAttack.Anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.GetCooldownTime());
            curAttack.ResetAttack();
        } while (bIsMainPressed);
        //curAttack.Anim(character, true);
        bIsMainAttacking = false;
    }

    IEnumerator SpecialAttackWaiter(Attack curAttack)
    {
        do
        {
            ActivatePUI(curAttack);
            curAttack.DoAttack();
            //curAttack.Anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.GetCooldownTime());
            curAttack.ResetAttack();
        } while (bIsAbilPressed);
        //curAttack.Anim(character, true);
        bIsAbilAttacking = false;
    }

    IEnumerator MovementAttackWaiter(Attack curAttack)
    {
        do
        {
            ActivatePUI(curAttack);
            curAttack.DoAttack();
            curAttack.Anim(character, false); //THIS IS THE ANIMATOR
            yield return new WaitForSeconds(curAttack.GetCooldownTime());
            curAttack.ResetAttack();
        } while (bIsMovePressed);
        curAttack.Anim(character, true);
        bIsMoveAttacking = false;
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
    private void SetMovePressed(bool newValue)
    {
        bIsMovePressed = newValue;
    }

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

    #region Tutorial-Specific Subscriptions
    //  function for Connor's tutorial
    public void activateSubscription(TutorialChecker.teachTypes type)
    {
        switch (type)
        {
            case TutorialChecker.teachTypes.BasicAtt:
                pInput.Player.BasicAttack.started += ctx => PerformAttack(meleeAttack, Attack.AttackType.Melee);
                pInput.Player.BasicAttack.canceled += ctx => SetMainPressed(false);
                break;
            case TutorialChecker.teachTypes.SecondAtt:
                pInput.Player.SecondAttack.started += ctx => PerformAttack(rangedAttack, Attack.AttackType.Ranged);
                pInput.Player.SecondAttack.canceled += ctx => SetMainPressed(false);
                break;
            case TutorialChecker.teachTypes.RocketAtt:
                pInput.Player.Ability1.started += ctx => PerformAttack(specialAttack, Attack.AttackType.Special);
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
