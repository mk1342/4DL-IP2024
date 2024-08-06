using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.ProBuilder.Shapes;
using TMPro;
using System;

public class Keypad : MonoBehaviour
{
    public Door door;
    public bool isRandom = false;
    public int numOfDigits = 4;
    public string correctCode;
    public TextMeshPro[] digitTexts;

    private string inputCode = "";
    private AudioSource audioSource;
    public bool solved = false;
    private void Start()
    {
        door.SetLock(true);
        if (isRandom)
        {
            GenerateRandomCode();
            AssignCodeToUIText();
        }
        audioSource = GetComponent<AudioSource>();
    }


    void GenerateRandomCode()
    {
        // Generate a random 4-digit number
        correctCode = "";
        int randomNumber = UnityEngine.Random.Range((int)Math.Pow(10, numOfDigits-1), (int)Math.Pow(10, numOfDigits));
        correctCode = randomNumber.ToString();
        Debug.Log(correctCode);
    }

    void AssignCodeToUIText()
    {
        for (int i = 0; i < numOfDigits; i++)
        {
            digitTexts[i].text = correctCode[i].ToString();
        }
    }
    public void EnterDigit(string digit)
    {
        inputCode += digit;
        Sound();
    }
    public void CheckCode()
    {
        if (inputCode == correctCode)
        {
            door.SetLock(false);
            solved = true;
            Debug.Log("Unlocked");
        }
        inputCode = "";
        Sound();
    }

    public void ClearCode()
    {
        inputCode = "";
        Sound();
    }

    void Sound()
    {
        //AudioManager.Instance.PlaySfx("Button", audioSource);
    }
}
