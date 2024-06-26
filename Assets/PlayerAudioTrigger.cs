using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioTrigger : MonoBehaviour
{
    private List<AudioSource> audioSources = new List<AudioSource>();

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        if (other.CompareTag("Present"))
        {
            AudioSource presentAudioSource = other.GetComponent<AudioSource>();
            if (presentAudioSource != null)
            {
                presentAudioSource.spatialBlend = 0.4f; // Set to 2D
                audioSources.Add(presentAudioSource);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Present"))
        {
            AudioSource presentAudioSource = other.GetComponent<AudioSource>();
            if (presentAudioSource != null)
            {
                presentAudioSource.spatialBlend = 1f; // Set to 3D
                audioSources.Remove(presentAudioSource);
            }
        }
    }

    void OnDisable()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null)
            {
                audioSource.spatialBlend = 1f; // Reset to 3D when this script is disabled
            }
        }
        audioSources.Clear();
    }
}