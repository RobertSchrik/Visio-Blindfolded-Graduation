using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Transform teleportPosition; // Reference to the teleport destination
    public float teleportDuration = 10f; // Duration (in seconds) to hold down the button for teleportation

    private float buttonPressTime = 0f; // Time when the teleport button was pressed

    void Update()
    {
        // Check if the teleport button is pressed
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            buttonPressTime = Time.time;
        }

        // Check if the teleport button is held down for the specified duration
        if (OVRInput.Get(OVRInput.Button.One) && Time.time - buttonPressTime >= teleportDuration)
        {
            // Teleport the player to the designated position
            TeleportPlayer();
        }

        // Reset teleportation timer when the button is released
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            buttonPressTime = 0f;
        }
    }

    void TeleportPlayer()
    {
        // Teleport the player to the designated position
        if (teleportPosition != null)
        {
            transform.position = teleportPosition.position;
        }
    }
}