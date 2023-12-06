using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{
   public void startGame()
    {
        SceneManager.LoadScene(4);
    }

    public void endGame()
    {
        Application.Quit();
    }
}
