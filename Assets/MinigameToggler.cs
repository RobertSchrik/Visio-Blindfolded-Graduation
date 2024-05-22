using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameToggler : MonoBehaviour
{

    [SerializeField] GameObject ActiveWhileOn;
    [SerializeField] GameObject ActiveWhileOff;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ActiveWhileOff.SetActive(false);
            ActiveWhileOn.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ActiveWhileOff.SetActive(true);
            ActiveWhileOn.SetActive(false);
        }
    }
}
