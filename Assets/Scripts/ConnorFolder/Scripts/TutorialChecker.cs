using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialChecker : MonoBehaviour {

    PInput controls;

    //  0 - move, 1 - dash
    [SerializeField] List<TextMeshProUGUI> tutTexts = new List<TextMeshProUGUI>();

    int lastShownInd = -1;

    private void Awake() {
        controls = new PInput();
        controls.Enable();
        controls.Player.Movement.performed += ctx => changeShownText(1);
        controls.Player.Dash.performed += ctx => changeShownText(2);

        changeShownText(0);
    }

    private void OnDisable() {
        controls.Disable();
    }

    void changeShownText(int ind) {
        if(ind <= lastShownInd)
            return;
        lastShownInd = ind;
        for(int i = 0; i < tutTexts.Count; i++)
            tutTexts[i].enabled = i == ind;
    }
}
