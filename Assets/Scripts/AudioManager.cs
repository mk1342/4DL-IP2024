using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource sfxAudioSource;
    public AudioSource bgmAudioSource;

    private Dictionary<string, AudioClip> audioClips;

    void Start()
    {
        LoadAudioClips();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadAudioClips()
    {
        audioClips = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }
    }

    public void PlaySfx(string clipName, AudioSource audioSource)
    {
        if (audioClips.ContainsKey(clipName))
        {
            audioSource = audioSource? audioSource:sfxAudioSource;
            audioSource.clip = audioClips[clipName];
            audioSource.Play();
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }
}
