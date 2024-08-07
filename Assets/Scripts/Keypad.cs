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
    private int numDisplay;

    private string inputCode = "";
    private AudioSource audioSource;
    public bool solved = false;

    private List<int> codeDisplayIndices = new List<int>();
    private List<string> codeCoordinates = new List<string>(); // To store coordinates (or indices) of code digits
    private void Start()
    {
        numDisplay = digitTexts.Length;
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
        if (numOfDigits < numDisplay)
        {
            System.Random rand = new System.Random();
            HashSet<int> usedIndices = new HashSet<int>();

            // Assign each digit of the code to a random TextMeshPro display
            for (int i = 0; i < numOfDigits; i++)
            {
                int index;
                do
                {
                    index = rand.Next(numDisplay);
                } while (usedIndices.Contains(index));
                usedIndices.Add(index);
                digitTexts[index].text = correctCode[i].ToString();

                // Store the index and coordinates of the assigned digit
                codeDisplayIndices.Add(index);
                codeCoordinates.Add("A1"); // Assuming a 6x6 grid
            }

            // Assign random digits to the remaining TextMeshPro displays
            for (int i = 0; i < numDisplay; i++)
            {
                if (!usedIndices.Contains(i))
                {
                    digitTexts[i].text = UnityEngine.Random.Range(0, 10).ToString();
                }
            }
        }

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
