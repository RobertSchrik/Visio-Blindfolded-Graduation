using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCaneHaptics : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            StartCoroutine(Haptics(1, 1, 0.1f, true, false));
        }       
    }

    IEnumerator Haptics(float frequency, float amplitude, float duration, bool rightHand, bool leftHand)
    {
        if (rightHand) OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.RTouch);
        if (leftHand) OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.LTouch);

        yield return new WaitForSeconds(duration);

        if (rightHand) OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        if (leftHand) OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    }
}
