using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggWiggle : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip audioClip;

    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;

    private float waitTime;
    private bool isWiggling = false;

    void Start()
    {
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        StartWiggle();
    }

    void StartWiggle()
    {
        StartCoroutine(WiggleCoroutine());
    }

    IEnumerator WiggleCoroutine()
    {
        while (true)
        {
            animator.SetBool("Wiggle", true);
            isWiggling = true;
            PlayAudioIfAvailable();

            yield return new WaitForSeconds(waitTime);

            animator.SetBool("Wiggle", false);
            isWiggling = false;
            waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(audioClip.length);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void PlayAudioIfAvailable()
    {
        if (audioSource && audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    void OnDisable()
    {
        StopAllCoroutines(); // Stop the coroutine when the script is disabled
    }

    void OnEnable()
    {
        StartWiggle(); // Restart the coroutine when the script is enabled
    }
}
