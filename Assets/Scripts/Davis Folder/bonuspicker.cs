using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
[RequireComponent(typeof(GameActionSequence))]
[RequireComponent(typeof(UpdateStat_GA))]
public class bonuspicker : MonoBehaviour
{
    public Animator bonusScreen;
    public TMP_Text button1;
    public TMP_Text button2;
    public TMP_Text button3;
    public CharacterFrame Character;
    public GameActionSequence sequence;
    public UpdateStat_GA stat;
    private int bonus1, bonus2, bonus3 = 10;


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
        StartCoroutine(animationPlay("open", 1f));
    }

    public void bonusCloser()
    {
        StartCoroutine(animationPlay("close", 1f));
    }

    public int randomPicker()
    {
        int randomNumber = Random.Range(0, 8);
        return randomNumber;
    }

    private int verifyNumber(int number)
    {
        if(number == bonus1 || number == bonus2)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void generateBonuses()
    {
        int buffer, verify = 10;
        int picked = 0;
        while (picked < 3)
        {
            buffer = randomPicker();
            verify = verifyNumber(buffer);
            if (verify == 0 && bonus1 == 10)
            {
                bonus1 = buffer;
                picked++;
            }
            else if (verify == 0 && bonus2 == 10)
            {
                bonus2 = buffer;
                picked++;
            }
            else if (verify == 0 && bonus3 == 10)
            {
                bonus3 = buffer;
                picked++;
            }
        }
        Debug.Log("Bonuses are: " + bonus1 + bonus2 + bonus3);
    }

    public void setup()
    {
        //generateBonuses();
        button1.text = descriptions[randomPicker()];
        button2.text = descriptions[randomPicker()];
        button3.text = descriptions[randomPicker()];
    }

    public void chooseBonus(int choice)
    {
        switch(choice)
        {
            case 0:
                stat.stat = CharStats.attackDamage;
                stat.modifier = 1f;
                break;
            default:
                break;
        }

        sequence.Play();
    }
}
