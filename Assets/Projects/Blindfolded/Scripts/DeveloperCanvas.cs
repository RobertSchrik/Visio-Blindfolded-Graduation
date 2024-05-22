using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperCanvas : MonoBehaviour
{

    void Start()
    {
        Console.SetActive(!Console.activeSelf);
    }

    [SerializeField] GameObject Console;
    // Update is called once per frame
    void Update()
    {
        //OVRInput.Update();

        if (OVRInput.GetDown(OVRInput.Button.One) && OVRInput.GetDown(OVRInput.Button.Two))
        {
            Console.SetActive(!Console.activeSelf);
        }
    }
}
