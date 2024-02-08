using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

[RequireComponent(typeof(GameActionSequence))]
[RequireComponent(typeof(UpdateStat_GA))]
public class bonuspicker : MonoBehaviour
{
    public Animator bonusScreen;
    public TMP_Text button1;
    public TMP_Text button2;
    public TMP_Text button3;
    public GameActionSequence sequence;
    public UpdateStat_GA stat;
    private int bonus1, bonus2, bonus3 = 10;
    public bool picked = false;

    public static Action EnablePlayerMovement = delegate { };
    public static Action DisablePlayerMovement = delegate { };
    public string[] descriptions = new string[8];
    public int[] bonuses = new int[8];
    //public float transitionTime = 1f;

    IEnumerator animationPlay(string trigger, float timeToWait)
    {
        bonusScreen.SetTrigger(trigger);
        yield return new WaitForSeconds(timeToWait);
    }

    private void Start()
    {
        sequence = GetComponent<GameActionSequence>();
        stat = GetComponent<UpdateStat_GA>();
    }

    public void bonusOpener()
    {
        AkSoundEngine.PostEvent("Bonus_Menu_Open", gameObject);

        StartCoroutine(animationPlay("open", 1f));
    }

    public void bonusCloser()
    {
        AkSoundEngine.PostEvent("Bonus_Menu_Close", gameObject);
        EventSystem.current.SetSelectedGameObject(null);

        StartCoroutine(animationPlay("close", 1f));
    }

    public int randomPicker()
    {
        int randomNumber = UnityEngine.Random.Range(0, 8);
        return randomNumber;
    }

    private void validateBonuses(int bonus)
    {
        if(bonus == 1)
        {
            if(bonus1 != bonus2 && bonus1 != bonus3)
            {
                Debug.Log("Validated bonus 1");
            } else
            {
                bonus1 = randomPicker();
                validateBonuses(1);
            }
        } else if(bonus == 2)
        {
            if (bonus2 != bonus1 && bonus2 != bonus3)
            {
                Debug.Log("Validated bonus 2");
            }
            else
            {
                bonus2 = randomPicker();
                validateBonuses(2);
            }
        }
        else if(bonus == 3)
        {
            if (bonus3 != bonus1 && bonus3 != bonus2)
            {
                Debug.Log("Validated bonus 3");
            }
            else
            {
                bonus3 = randomPicker();
                validateBonuses(3);
            }
        } else
        {
            Debug.LogError("Not a valid bonus!");
        }
    }

    public void setup()
    {
        picked = false;
        bonus1 = randomPicker();
        bonus2 = randomPicker();
        bonus3 = randomPicker();
        for(int c = 1; c < 4; c++)
        {
            validateBonuses(c);
        }
        button1.text = descriptions[bonus1];
        button2.text = descriptions[bonus2];
        button3.text = descriptions[bonus3];
        //Character.gameObject.GetComponent<PlayerMovement>().enabled = false;
        DisablePlayerMovement();
        bonusOpener();
    }

    public void chooseBonus(int button)
    {
        if(button == 1)
        {
            applyBonus(bonus1);
        } else if(button == 2)
        {
            applyBonus(bonus2);
        } else if(button == 3)
        {
            applyBonus(bonus3);
        }
        bonusCloser();
        EnablePlayerMovement();
        picked = true;
        //Character.enabled = true;
        //Character.gameObject.GetComponent<PlayerMovement>().enabled = true;
        //Character.gameObject.GetComponent<Basic_Proj>().enabled = true;
        //Character.gameObject.GetComponent<GSword>().enabled = true;
    }

    public void applyBonus(int choice)
    {
        switch (choice)
        {
            case 0:
                stat.stat = CharStats.attackDamage;
                stat.modifier *= 1.2f;
                break;
            case 1:
                stat.stat = CharStats.attackSpeed;
                stat.modifier *= 1.1f;
                break;
            case 2:
                stat.stat = CharStats.cooldownMod;
                stat.modifier *= 1.3f;
                break;
            case 3:
                stat.stat = CharStats.dashCharges;
                stat.modifier = 1f;
                break;
            case 4:
                stat.stat = CharStats.dashDistance;
                stat.modifier = 1.1f;
                break;
            case 5:
                stat.stat = CharStats.dashSpeed;
                stat.modifier = 1.15f;
                break;
            case 6:
                stat.stat = CharStats.maxHealth;
                stat.modifier = 1.1f;
                break;
            case 7:
                stat.stat = CharStats.moveSpeed;
                stat.modifier = 1.2f;
                break;
            default:
                break;
        }
        Debug.Log("Bonus Applied! " + choice);
        #region Saving
        StateSaveData temp = new StateSaveData();
        temp.bonuses.Add(stat.stat);
        temp.mods.Add(stat.modifier);
        var data = JsonUtility.ToJson(temp);
        SaveData.setString("Bonuses", data);
        #endregion
        sequence.Play();
    }

    private void OnEnable()
    {
        // EnemyController.levelComplete += setup;

    }
    private void OnDisable()
    {
        //EnemyController.levelComplete -= setup;
    }
}

[System.Serializable]
public class StateSaveData
{
    public List<CharStats> bonuses = new();
    public List<float> mods = new();
}
