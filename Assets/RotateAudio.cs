using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAudio : MonoBehaviour
{
    public GameObject targetObject;  // The GameObject whose rotation will be changed
    public float rotationX = 0f;     // The x-axis rotation value to be set when entering
    private Vector3 initialRotation; // To store the initial rotation of the targetObject

    private void Start()
    {
        if (targetObject != null)
        {
            initialRotation = targetObject.transform.rotation.eulerAngles;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Assuming the player GameObject has the tag "Player"
        {
            if (targetObject != null)
            {
                Vector3 newRotation = new Vector3(rotationX, targetObject.transform.rotation.eulerAngles.y, targetObject.transform.rotation.eulerAngles.z);
                targetObject.transform.Rotate(0f, 180f, 0f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  // Assuming the player GameObject has the tag "Player"
        {
            if (targetObject != null)
            {
                targetObject.transform.Rotate(0f, 180f, 0f);
            }
        }
    }
}