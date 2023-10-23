using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

   public void loadNextScene()
    {
        StartCoroutine(levelTransition(SceneManager.GetActiveScene().buildIndex + 1, "none"));
    }

   public void playerDeath()
    {
        StartCoroutine(levelTransition(3, "death"));
    }
    
    public void loadSpecific(int index)
    {
        StartCoroutine(levelTransition(index, "none"));
    }

    IEnumerator levelTransition(int levelIndex, string trigger)
    {
        if (trigger != "none")
        {
            transition.SetTrigger(trigger);
        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
