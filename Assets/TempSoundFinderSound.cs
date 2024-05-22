using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSoundFinderSound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<TempSoundFinderManager>().ResetMinigame();
        }
    }
}
