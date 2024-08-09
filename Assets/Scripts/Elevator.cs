using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public int duration = 2;
    public int waitTime;
    public bool isOpen;
    public bool arriving = false;
    public int sceneIndex;

    private Vector3 LDoorClosed;
    private Vector3 RDoorClosed;
    public Transform LDoorOpened;
    public Transform RDoorOpened;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (arriving)
        {
            numDisplay.text = (currentFloor-1).ToString();
            Arriving();
        }
        else
        {
            numDisplay.text = (currentFloor).ToString();
        }
        isOpen = openOnStart;
        // Set the doors to the closed position at the start
        LDoorClosed = LDoor.position;
        RDoorClosed = RDoor.position;

        // Immediately open the doors if isOpen is true
        if (isOpen)
        {
            OpenDoor();
            barrier.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Arriving()
    {
        // Create a new sequence
        Sequence elevatorSequence = DOTween.Sequence();
        elevatorSequence.AppendCallback(() =>
        {
                arrowDisplay.enabled = true;
        });

        for (int i = 0; i < waitTime; i++)
        {
            elevatorSequence.AppendCallback(() =>
            {
                arrowDisplay.lineSpacing = -100;
            });
            elevatorSequence.AppendInterval(1/3f);
            elevatorSequence.AppendCallback(() =>
            {
                arrowDisplay.lineSpacing = -60;
            });
            elevatorSequence.AppendInterval(1 / 3f);
            elevatorSequence.AppendCallback(() =>
            {
                arrowDisplay.lineSpacing = -20;
            });
            elevatorSequence.AppendInterval(1 / 3f);
        }
        elevatorSequence.AppendCallback(() =>
        {
            arrowDisplay.enabled = false;
        });


        elevatorSequence.AppendCallback(() =>
        {
            numDisplay.text = currentFloor.ToString();
        });

        elevatorSequence.AppendInterval(3f);

        // Open the door
        elevatorSequence.AppendCallback(() =>
        {
            OpenDoor();
        });

        // Start the sequence
        elevatorSequence.Play();
    }

    void OpenDoor()
    {
        LDoor.DOMove(LDoorOpened.position, duration);
        RDoor.DOMove(RDoorOpened.position, duration);
        barrier.GetComponent<BoxCollider>().enabled = false;
        isOpen = true;
    }

    void CloseDoor()
    {
        LDoor.DOMove(LDoorClosed, duration);
        RDoor.DOMove(RDoorClosed, duration);
        barrier.GetComponent<BoxCollider>().enabled = true;
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!arriving && isOpen)
        {
            CloseDoor();
        }
    }
}
