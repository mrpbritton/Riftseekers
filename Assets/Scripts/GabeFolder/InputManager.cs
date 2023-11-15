using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Action SwapToController = delegate {};
    public static Action SwapToKeyboard = delegate {};

    private PInput pInput;
    private bool isController; 
    private void OnEnable()
    {
        pInput = new PInput();
        pInput.Enable();

        pInput.Player.AnyKey.performed += ctxt => Keyboard();
        pInput.Player.AnyController.performed += ctxt => Controller();
    }

    private void OnDisable()
    {
        pInput.Disable();

        pInput.Player.AnyKey.performed -= ctxt => Keyboard();
        pInput.Player.AnyController.performed -= ctxt => Controller();
    }

    private void Controller()
    {
        if(!isController) //not controller
        {
            SwapToController();
        }
    }

    private void Keyboard()
    {
        if (isController) //is controller
        {
            SwapToKeyboard();
        }
    }

}