using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public abstract  class KeypadButtons : MonoBehaviour
{
    public float clickCooldown = 0.2f; // Cooldown duration in seconds
    protected bool canClick = true;
    public abstract void PressButton();

    public void Interact()
    {
        if (canClick)
        {
            PressButton();
        }
    }
    public IEnumerator ResetClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(clickCooldown);
        canClick = true;
    }
}
