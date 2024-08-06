using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class KeypadClear : KeypadButtons
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

            keypad.ClearCode();
            Debug.Log("Clear");
            StartCoroutine(ResetClickCooldown());
        }
    }
}
