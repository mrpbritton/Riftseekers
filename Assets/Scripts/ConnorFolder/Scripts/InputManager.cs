using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static Action<bool> switchInput = delegate { };
    static bool usingKeyboard;

    PInput pInput;

    private void OnEnable() {
        pInput = new PInput();
        pInput.Enable();

        switchInput += updateInputState;
        pInput.Player.AnyKey.performed += ctxt => trySwitchInput(true);
        pInput.Player.AnyController.performed += ctxt => trySwitchInput(false);
    }

    private void OnDisable() {
        pInput.Disable();

        switchInput -= updateInputState;
        pInput.Player.AnyKey.performed -= ctxt => switchInput(true);
        pInput.Player.AnyController.performed -= ctxt => switchInput(false);
    }

    void updateInputState(bool b) {
        usingKeyboard = b;
    }

    void trySwitchInput(bool b) {
        if(usingKeyboard != b)
            switchInput(b);
    }

    public static bool isUsingKeyboard() {
        return usingKeyboard;
    }
}