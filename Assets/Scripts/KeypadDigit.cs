using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadDigit : KeypadButtons
{
    public string digit;
    public GameObject keypadGrp;
    Keypad keypad;

    void Start()
    {
        keypad = keypadGrp.GetComponent<Keypad>();
    }

    public override void PressButton()
    {
        if (canClick && keypad != null && !keypad.solved)
        {
            keypad.EnterDigit(digit);
            Debug.Log(digit);
            StartCoroutine(ResetClickCooldown());
        }
    }
}
