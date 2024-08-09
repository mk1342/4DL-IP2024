using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonEuclidean : MonoBehaviour
{
    public GameObject currentSection;
    public GameObject nextSection;
    public GameObject prevSection;

    public bool isNextTrigger = true;  // Determines if this trigger moves to the next section

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isNextTrigger)
            {
                EnableSection(nextSection);
                DisableSection(prevSection);
            }
            else
            {
                EnableSection(prevSection);
                DisableSection(nextSection);
            }
        }
    }

    private void EnableSection(GameObject section)
    {
        if (section != null)
        {
            section.SetActive(true);
        }
    }

    private void DisableSection(GameObject section)
    {
        if (section != null)
        {
            section.SetActive(false);
        }
    }
}