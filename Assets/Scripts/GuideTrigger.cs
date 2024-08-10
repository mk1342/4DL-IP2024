using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuideTrigger : MonoBehaviour
{
    public Guide guide;
    public bool appear = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            guide.TriggerGuideAppearance();
        }
    }

}
