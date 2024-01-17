using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour {
    [SerializeField] GameObject background;

    //  Buttons
    public void resume() {
        GetComponent<InventoryUI>().hide();
    }
    public void options() {
        Debug.Log("here's the options");
    }
    public void mainMenu() {
        SceneManager.LoadScene(0);
    }
}
