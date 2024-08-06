using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public string itemName;
    public string message;

    public abstract void Interact(Player player);
}
