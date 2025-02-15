using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour {
    [SerializeField] GameObject background;
    [SerializeField] Button button;
    PInput controls;
    PlayerMovement pm;

    bool shown = false;

    private void OnEnable() {
        Time.timeScale = 1.0f;
        pm = FindObjectOfType<PlayerMovement>();
        controls = new PInput();
        controls.Enable();
        controls.UI.Pause.performed += ctx => toggleShown();
        background.SetActive(false);
    }

    private void OnDisable() {
        controls.UI.Pause.performed -= ctx => toggleShown();
        controls.Disable();
    }

    void toggleShown() {
        shown = !shown;
        Time.timeScale = shown ? 0f : 1f;
        background.SetActive(shown);
        button.Select();
        pm.enabled = !shown;
        AttackManager.I.canAttack = !shown;
    }

    //  Buttons
    public void resume() {
        toggleShown();
    }
    public void options() {
        OptionsCanvas.I.show();
    }
    public void mainMenu() {
        SceneManager.LoadScene(0);
    }
}
