using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAudioTimer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float minAudioDelay = 1f;
    public float maxAudioDelay = 4f;

    private float audioDelay;

    void Start()
    {
        // Start playing audio with initial delay
        PlayAudioWithDelay();
    }

    void PlayAudioWithDelay()
    {
        // Generate a random delay for the next audio playback
        audioDelay = Random.Range(minAudioDelay, maxAudioDelay);
        Invoke("PlayAudio", audioDelay);
    }

    void PlayAudio()
    {
        if (audioSource && audioClip)
        {
            Debug.Log("Playing audio...");

            // Play the audio clip
            audioSource.clip = audioClip;
            audioSource.Play();

            // Schedule the next audio playback
            PlayAudioWithDelay();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing!");
        }
    }

    void OnDisable()
    {
        // Cancel Invoke when the script is disabled to avoid errors
        CancelInvoke();
    }
}
