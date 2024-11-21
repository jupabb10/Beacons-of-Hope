using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton para acceso global

    public AudioSource backgroundAudioSource; // Para música de fondo
    public AudioSource effectsAudioSource;

    public AudioClip gameOverSound;
    public AudioClip correctSound;
    public AudioClip mainMenuSound;

    private void Awake()
    {
        // Configura el Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Permite que persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundSound(AudioClip clip, bool loop = false)
    {
        if (backgroundAudioSource != null && clip != null)
        {
            backgroundAudioSource.clip = clip;
            backgroundAudioSource.loop = loop;
            backgroundAudioSource.Play();
        }
    }

    public void PlayEffectSound(AudioClip clip)
    {
        if (effectsAudioSource != null && clip != null)
        {
            effectsAudioSource.PlayOneShot(clip);
        }
    }

    public void StopBackgroundSound()
    {
        if (backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
    }
}
