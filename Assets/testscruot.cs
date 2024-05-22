using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscruot : MonoBehaviour
{
    void Update()
    {
        Rigidbody Rb = GetComponent<Rigidbody>();
        if ((Mathf.Abs(Rb.velocity.x) + Mathf.Abs(Rb.velocity.y) + Mathf.Abs(Rb.velocity.z)) / 3 < 0.00001f)
            Rb.velocity = new Vector3(0.00001f, 0.00001f, 0.00001f);
    }

    public float pushForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the hand
        if (other.CompareTag("Hand"))
        {
            // Add force to the door
            Rigidbody doorRb = GetComponent<Rigidbody>();
            if (doorRb != null)
            {
                // Calculate the force direction (you might need to adjust this based on your door's orientation)
                Vector3 forceDirection = transform.forward; // Assuming the door opens along the forward axis

                // Apply force to the door
                doorRb.AddForce(forceDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}
