using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    /// <summary>
    /// Only used for physical triggers
    /// </summary>
    public int sceneIndex;
    public Image fadeImage; // Assign this in the Inspector
    public float fadeDuration = 1.5f; // Duration of the fade effect

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        fadeImage.DOFade(0, fadeDuration);
    }

    public void FadeToBlackAndLoadScene(int sceneIndex)
    {
        fadeImage.DOFade(1, fadeDuration).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneIndex);
        });
    }

    public void OnTriggerEnter(Collider other)
    {
        FadeToBlackAndLoadScene(sceneIndex);
    }
}
