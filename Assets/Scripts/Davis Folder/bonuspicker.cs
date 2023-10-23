using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonuspicker : MonoBehaviour
{
    public Animator bonusScreen;

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
}
