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
        SaveData.deleteCurrentSave();
        StartCoroutine(levelTransition(1, "death"));
    }
    
    public void loadSpecific(int index)
    {
        if(index == 1)
        {
            StartCoroutine(levelTransition(index, "death"));
        } else if(index == 3)
        {
            StartCoroutine(levelTransition(index, "alive"));
        }
        StartCoroutine(levelTransition(index, "none"));
    }

    IEnumerator levelTransition(int levelIndex, string trigger)
    {
        if (trigger != "none")
        {
            transition.SetTrigger(trigger);
        }
        yield return new WaitForSeconds(transitionTime);
        if (levelIndex < 8)
        {
            SceneManager.LoadScene(levelIndex);
        } else
        {
            SceneManager.LoadScene(0);
        }
    }
}
