using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputerText : MonoBehaviour
{
    public string coordinate;
    private TextMeshPro textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }
}
