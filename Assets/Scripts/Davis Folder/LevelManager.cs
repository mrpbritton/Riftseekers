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
        StartCoroutine(levelTransition(SceneManager.GetActiveScene().buildIndex + 1));
    }

   public void playerDeath()
    {
        StartCoroutine(levelTransition(1));
    }
    
    public void loadSpecific(int index)
    {
        StartCoroutine(levelTransition(index));
    }

    IEnumerator levelTransition(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
