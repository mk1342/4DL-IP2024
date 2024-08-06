using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class KeypadEnter : KeypadButtons
{
    public GameObject keypadGrp;
    Keypad keypad;

    void Start()
    {
        keypad = keypadGrp.GetComponent<Keypad>();
    }
    public override void PressButton()
    {
        if (keypad != null && canClick && !keypad.solved)
        {

            keypad.CheckCode();
            Debug.Log("Check");
            StartCoroutine(ResetClickCooldown());
        }
    }
}
