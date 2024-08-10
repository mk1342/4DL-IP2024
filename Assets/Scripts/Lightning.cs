using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Lightning : MonoBehaviour
{
    public Light directionalLight;           // The directional light to control
    public float minIntensity = 1f;          // Minimum intensity
    public float maxIntensity = 10f;         // Maximum intensity
    public AudioSource bgmSource;            // Reference to the AudioSource playing the BGM
    public float[] thunderTimestamps;        // Array of thunder timestamps in seconds
    public Material skybox;

    private Sequence thunderSequence;

    private void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = GetComponent<Light>();
        }

        if (bgmSource == null || thunderTimestamps.Length == 0)
        {
            Debug.LogError("Please assign the BGM AudioSource and thunder timestamps.");
            return;
        }

        // Start playing the BGM
        bgmSource.loop = true;
        bgmSource.Play();

        // Start syncing the thunder sequence with the BGM loop
        StartCoroutine(SyncSequenceWithBGM());
    }

    private void CreateThunderSequence()
    {
        thunderSequence = DOTween.Sequence();

        // Loop through all the thunder timestamps
        for (int i = 0; i < thunderTimestamps.Length; i++)
        {
            float waitTime = thunderTimestamps[i];

            // Wait until the thunderclap
            thunderSequence.AppendInterval(waitTime - (i == 0 ? 0 : thunderTimestamps[i - 1]));

            // Simulate lightning by increasing and decreasing the light intensity
            thunderSequence.Append(directionalLight.DOIntensity(maxIntensity, 0.2f))
                        .Join(skybox.DOFloat(3, "_Exposure",  0.2f)) // Tween the skybox exposure at the same time
                        .Append(directionalLight.DOIntensity(minIntensity, 0.8f))
                        .Join(skybox.DOFloat(1, "_Exposure", 0.8f)); // Return skybox exposure to original
        }

        thunderSequence.Pause(); // Pause the sequence initially
    }

    private IEnumerator SyncSequenceWithBGM()
    {
        while (true)
        {
            // Create the sequence before starting it
            CreateThunderSequence();

            // Play the sequence
            thunderSequence.Play();

            // Wait for the BGM to complete its loop
            yield return new WaitForSeconds(bgmSource.clip.length);

            // Kill the sequence to reset it properly
            thunderSequence.Kill();
        }
    }
}