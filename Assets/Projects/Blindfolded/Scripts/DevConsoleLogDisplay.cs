using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevConsoleLogDisplay : MonoBehaviour
{
    [SerializeField] ScrollRect Scroller;
    public float scrollSpeed = 1.0f;

    TextMeshProUGUI consoleTxt;
    string timestamp = System.DateTime.Now.ToString("HH:mm:ss");

    private string logBuffer = "";

    // Start is called before the first frame update
    void Start()
    {
        consoleTxt = GetComponent<TextMeshProUGUI>();
        Application.logMessageReceived += HandleLog;
    }
    private void Update()
    {

        //Scroll behaviour
        // Get input from your VR controller for vertical scrolling (e.g., thumbstick input)
        float verticalInput = Input.GetAxis("Vertical");
        //Debug.Log(Input.GetAxis("Vertical"));
        // Adjust the scroll position based on input
        float currentScroll = Scroller.verticalNormalizedPosition;
        currentScroll += verticalInput * scrollSpeed * Time.deltaTime;
        currentScroll = Mathf.Clamp01(currentScroll);

        Scroller.verticalNormalizedPosition = currentScroll;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Append the log message to your log buffer
        
        timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        logBuffer += "[" + timestamp + "] " + logString + "\n";
        consoleTxt.text = logBuffer; // Update the text on your UI element
    }

    public void LogMessage(string message)
    {
        // Append the custom log message to the log buffer
        timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        consoleTxt.text += "[" + timestamp + "] " + message + "\n";
    }


}


