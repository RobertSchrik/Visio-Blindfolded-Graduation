using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public float rotationAngle = 90f;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        rotationAngle++;
    }
}
