using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoorLogic : MonoBehaviour
{
    [Header("AnimatorHandler")]
    public Animator DoorAnimation;

    [Header("AudioSources")]
    public AudioSource doorOpenAudioSource;
    public AudioClip DoorOpenSound;

    public AudioSource doorCloseAudioSource;
    public AudioClip DoorCloseSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoorAnimation.SetBool("PlayerEntersSlideDoorCollider", true);
            if (DoorOpenSound != null && doorOpenAudioSource != null)
            {
                doorOpenAudioSource.clip = DoorOpenSound;
                doorOpenAudioSource.Play();
            }
            else
            {
                Debug.LogError("Audio File :" + this.gameObject + "Is Missing");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoorAnimation.SetBool("PlayerEntersSlideDoorCollider", false);
            if (DoorCloseSound != null && doorCloseAudioSource != null)
            {
                // Play the door open sound
                doorCloseAudioSource.clip = DoorCloseSound;
                doorCloseAudioSource.Play();
            }
            else
            {
                Debug.LogError("Audio File :" + this.gameObject + "Is Missing");
            }
        }
    }
}
