using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float openDuration = 1f;
    public bool locked = false;
    public bool autoOpen = false;
    public Transform pivot;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private AudioSource audioSource;

    void Start()
    {
        closedRotation = pivot.transform.rotation;
        openRotation = Quaternion.Euler(pivot.transform.eulerAngles + new Vector3(0, openAngle, 0));
        audioSource = GetComponent<AudioSource>();
    }

    public void Open()
    {
        if (!isOpen && !locked)
        {
            pivot.transform.DORotateQuaternion(openRotation, openDuration).OnComplete(() => isOpen = true);
            AudioManager.Instance.PlaySfx("openDoor", audioSource);
        }
        else if (isOpen && !locked)
        {
            pivot.transform.DORotateQuaternion(closedRotation, openDuration).OnComplete(() => isOpen = false);
            AudioManager.Instance.PlaySfx("closeDoor", audioSource);
        }
        else if (locked)
        {

        }
    }

    public void SetLock(bool value)
    {
        locked = value;
    }
}
