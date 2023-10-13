using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public bool bTest;

    [SerializeField]
    private int sceneIndex;
    [SerializeField]
    private Image blackScreen;
    private float rate;
    private bool bActive;

    private void Update()
    {
        if(bTest) 
        {
            bTest = false;
            LoadScene();
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void LoadScene()
    {
        if (bActive) return;
        StartCoroutine(nameof(SceneLoadingSequence));
    }
    IEnumerator SceneLoadingSequence()
    {
        bActive = true;
        //Fade to black
        while(rate < 1)
        {
            rate += Time.deltaTime;
            blackScreen.color = Color.Lerp(Color.clear, Color.black, rate);
            yield return new WaitForEndOfFrame();
        }
        rate = 0;
        //load scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (asyncOperation.progress != 1)
            yield return new WaitForEndOfFrame();
       
        //Fade in
        while (rate < 1)
        {
            rate += Time.deltaTime;
            blackScreen.color = Color.Lerp(Color.black, Color.clear, rate);
            yield return new WaitForEndOfFrame();
        }
        bActive = false;
    }
}
