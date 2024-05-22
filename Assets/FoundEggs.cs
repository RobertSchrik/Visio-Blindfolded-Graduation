using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoundEggs : MonoBehaviour
{
    public int score = 0;
    public AudioClip collectSound; // Audio clip for collecting the egg
    public AudioSource audioSource; // Reference to the AudioSource component
    public TextMeshProUGUI victoryText; // Reference to the TextMeshPro text
    private bool victoryTextActive = false;
    private bool buttonPressed = false;
    private float buttonPressTime = 0f;
    private float victoryTextActiveTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EasterEgg"))
        {
            // Disable the Easter Egg object
            other.gameObject.SetActive(false);
            audioSource.PlayOneShot(collectSound);
            // Increase the score
            score++;

            // Print the score to the console (optional)
            Debug.Log("Score: " + score);

            if (score == 5 && victoryText != null)
            {
                victoryText.gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        // Check if victory text is active
        if (victoryText.gameObject.activeSelf)
        {
            victoryTextActiveTime += Time.deltaTime;
        }

        // Check if any button is pressed down
        if (Input.anyKeyDown)
        {
            buttonPressed = true;
            buttonPressTime = Time.time;
        }

        // Check if the button has been pressed down for 3 seconds
        if (buttonPressed && Time.time - buttonPressTime >= 3f && victoryTextActiveTime >= 10f)
        {
            // Restart the scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
