using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAudio : MonoBehaviour
{
    public GameObject targetObject;
    private bool isRotated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetObject != null && !isRotated)
            {
                targetObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                isRotated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetObject != null && isRotated)
            {
                targetObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                isRotated = false;
            }
        }
    }
    public void DisableRotationAndResetPlayer()
    {
        targetObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        this.enabled = false;
        CheckForNewCollider();
    }

    private void CheckForNewCollider()
    {
        float radius = 1f;

        Collider[] hitColliders = Physics.OverlapSphere(targetObject.transform.position, radius);

        foreach (Collider hitCollider in hitColliders)
        {
            RotateAudio otherRotateAudio = hitCollider.GetComponentInChildren<RotateAudio>();
            if (otherRotateAudio != null && otherRotateAudio.enabled)
            {
                otherRotateAudio.ForcePlayerRotation();
            }
        }
    }
    public void ForcePlayerRotation()
    {
        if (targetObject != null)
        {
            targetObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
