using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG;

public class Elevator : MonoBehaviour
{
    public Transform LDoor;
    public Transform RDoor;
    public bool openOnStart = false;
    public Button button;
    public TextMeshPro numDisplay;
    public TextMeshPro arrowDisplay;
    public int currentFloor;
    public GameObject barrier;
    public int waitTime;
    public bool isOpen;

    private Vector3 LDoorClosed;
    private Vector3 RDoorClosed;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        numDisplay.text = currentFloor.ToString();
        isOpen = openOnStart;
        // Set the doors to the closed position at the start
        LDoor.position = LDoorClosed;
        RDoor.position = RDoorClosed;

        // Immediately open the doors if isOpen is true
        if (isOpen)
        {
            OpenDoor();
            barrier.GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenDoor()
    {

    }

    void CloseDoor()
    {

    }
}
