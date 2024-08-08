using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.ProBuilder.Shapes;
using TMPro;
using System;
using System.Linq;

public class Keypad : MonoBehaviour
{
    public Door door;
    public bool isRandom = false;
    public int numOfDigits = 4;
    public string correctCode;
    public TextMeshPro[] digitTexts;
    public TextMeshPro coordinateDisplay;

    private int numDisplay;
    private string inputCode = "";
    private AudioSource audioSource;
    private List<TextMeshPro> displayClone;
    public bool solved = false;

    private List<char> currentDigits;
    private string coordinates;
    private void Start()
    {
        numDisplay = digitTexts.Length;
        door.SetLock(true);
        displayClone = digitTexts.ToList();
        Debug.Log(displayClone);
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
        currentDigits = new List<char>(correctCode.ToCharArray());
        Debug.Log(correctCode);
    }

    void AssignCodeToUIText()
    {
        if (numOfDigits < numDisplay)
        {
            System.Random rand = new System.Random();
            int len = numDisplay;

            // Assign each digit of the code to a random TextMeshPro display
            for (int i = 0; i < numOfDigits; i++) 
            {
                int index = UnityEngine.Random.Range(0, len - 1);
                len -= 1;
                TextMeshPro textMeshPro = displayClone[index];
                char c = currentDigits[i];
                textMeshPro.text = c.ToString();
                //textMeshPro.color = Color.green;
                displayClone.Remove(textMeshPro);
                ComputerText coordinate = textMeshPro.GetComponent<ComputerText>();
                coordinates += coordinate.coordinate;
            }
            for (int i = 0; i < numDisplay-numOfDigits; i++)
            {
                int index = UnityEngine.Random.Range(0, len - 1);
                len -= 1;
                TextMeshPro textMeshPro = displayClone[index];
                textMeshPro.text = UnityEngine.Random.Range(0, 9).ToString();
                displayClone.Remove(textMeshPro);
            }

            coordinateDisplay.text = coordinates;

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
