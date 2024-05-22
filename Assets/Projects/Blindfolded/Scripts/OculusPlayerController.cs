using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class OculusPlayerController : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float rotationSpeed = 90.0f;

    private CharacterController characterController;
    private Transform cameraTransform;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        // Ensure XR settings are correctly configured for Oculus Quest 2
        XRSettings.LoadDeviceByName("Oculus");
        XRSettings.enabled = true;
    }

    private void Update()
    {
        // Player movement
        Vector3 moveDirection = Vector3.zero;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        moveDirection = (forward * inputZ + right * inputX).normalized;

        if (moveDirection != Vector3.zero)
        {
            characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
        }

        // Player rotation
        float inputRotation = Input.GetAxis("Rotation");

        if (inputRotation != 0)
        {
            transform.Rotate(Vector3.up * inputRotation * rotationSpeed * Time.deltaTime);
        }
    }
}
