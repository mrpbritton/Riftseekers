using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour {
    [SerializeField] GameObject background;

    PInput controls;
    PlayerMovement pm;

    bool shown = false;

    private void Start() {
        pm = FindObjectOfType<PlayerMovement>();
        controls = new PInput();
        controls.Enable();
        controls.UI.Pause.performed += ctx => toggleShown();
        background.SetActive(false);
    }

    private void OnDisable() {
        controls.UI.Pause.performed -= ctx => toggleShown();
    }

    void toggleShown() {
        shown = !shown;
        Debug.Log("here");
        background.SetActive(shown);

        pm.enabled = !shown;
        AttackManager.I.canAttack = !shown;
    }

    //  Buttons
    public void resume() {
        toggleShown();
    }
    public void options() {
        Debug.Log("here's the options");
    }
    public void mainMenu() {
        SceneManager.LoadScene(0);
    }
}
