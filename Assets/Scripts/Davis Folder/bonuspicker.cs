using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class bonuspicker : MonoBehaviour
{
    public Animator bonusScreen;
    public UnityEngine.UIElements.Button button1;
    public UnityEngine.UIElements.Button button2;
    public UnityEngine.UIElements.Button button3;
    public CharacterFrame Character;


    public string[] descriptions = new string[8];
    public int[] bonuses = new int[8];
    //public float transitionTime = 1f;

    IEnumerator animationPlay(string trigger, float timeToWait)
    {
        bonusScreen.SetTrigger(trigger);
        yield return new WaitForSeconds(timeToWait);
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

    private void verifyNumber()
    {

    } 
}
