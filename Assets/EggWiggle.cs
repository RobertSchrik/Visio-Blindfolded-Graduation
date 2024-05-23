using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggWiggle : MonoBehaviour
{
    public Animator animator;
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
        animator.SetBool("Wiggle", true);
        isWiggling = true;
        Invoke("StopWiggle", waitTime);
    }

    void StopWiggle()
    {
        animator.SetBool("Wiggle", false);
        isWiggling = false;
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        Invoke("StartWiggle", waitTime);
    }

    void OnDisable()
    {
        // Cancel Invoke when the script is disabled to avoid errors
        CancelInvoke();
    }

    private void OnEnable()
    {
        StartWiggle();
    }
}
