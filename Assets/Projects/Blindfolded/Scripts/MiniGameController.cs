using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class MiniGameController : MonoBehaviour
{
    public GameObject[] stage1Objects;
    public GameObject[] stage2Objects;
    public GameObject[] stage3Objects;
    public GameObject[] stage4Objects;
    public GameObject backgroundNoise;
    public GameObject player;
    public float detectionRadius = 2f;
    public int currentStage = 1;
    private GameObject[] currentStageObjects;
    public AudioClip collectSound;
    public AudioSource audioSource;
    private int objectsTouched = 0;
    public TextMeshProUGUI victoryText;
    private Coroutine soundCoroutine;
    private GameObject closestObject;
    public Animator backgroundNoiseAnimator; // Reference to the Animator component

    void Start()
    {
        DeactivateAllObjects();
        victoryText.gameObject.SetActive(false);
        StartStage(currentStage);
    }

    void Update()
    {
        if (currentStageObjects != null)
        {
            foreach (GameObject obj in currentStageObjects)
            {
                float distance = Vector3.Distance(player.transform.position, obj.transform.position);

                if (currentStage == 2 || currentStage == 4)
                {
                    MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.enabled = (distance <= detectionRadius);
                    }
                }

                if (distance <= 1f && obj.activeSelf)
                {
                    audioSource.PlayOneShot(collectSound);
                    obj.SetActive(false); // Deactivate object once touched
                    objectsTouched++;
                    if (obj == closestObject)
                    {
                        StopCoroutine(soundCoroutine); // Stop current sound coroutine
                        ChooseNewSoundObject();
                    }
                }
            }

            if (objectsTouched >= currentStageObjects.Length)
            {
                if (currentStage < 5)
                {
                    NextStage();
                }
                return;
            }

            UpdateClosestObject();
        }
    }

    void StartStage(int stage)
    {
        objectsTouched = 0;

        switch (stage)
        {
            case 1:
                currentStageObjects = stage1Objects;
                break;
            case 2:
                currentStageObjects = stage2Objects;
                break;
            case 3:
                currentStageObjects = stage3Objects;
                backgroundNoise.SetActive(true);
                if (backgroundNoiseAnimator != null)
                {
                    backgroundNoiseAnimator.enabled = true; // Enable Animator
                }
                break;
            case 4:
                currentStageObjects = stage4Objects;
                backgroundNoise.SetActive(true);
                if (backgroundNoiseAnimator != null)
                {
                    backgroundNoiseAnimator.enabled = true; // Enable Animator
                }
                break;
        }

        foreach (GameObject obj in currentStageObjects)
        {
            obj.SetActive(true);
            if (stage == 2 || stage == 4)
            {
                MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }
        }

        ChooseNewSoundObject();
    }

    void NextStage()
    {
        foreach (GameObject obj in currentStageObjects)
        {
            obj.SetActive(false);
        }
        backgroundNoise.SetActive(false);
        if (backgroundNoiseAnimator != null)
        {
            backgroundNoiseAnimator.enabled = false; // Disable Animator
        }

        currentStage++;

        if (currentStage > 4)
        {
            victoryText.gameObject.SetActive(true);
            Debug.Log("Game Completed!");
        }
        else
        {
            StartStage(currentStage);
        }
    }

    void ChooseNewSoundObject()
    {
        UpdateClosestObject();
        if (closestObject != null)
        {
            soundCoroutine = StartCoroutine(PlaySoundWithRandomIntervals(closestObject));
        }
    }

    void UpdateClosestObject()
    {
        float closestDistance = float.MaxValue;
        closestObject = null;

        foreach (GameObject obj in currentStageObjects)
        {
            if (obj.activeSelf)
            {
                float distance = Vector3.Distance(player.transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }
        }
    }

    IEnumerator PlaySoundWithRandomIntervals(GameObject soundObject)
    {
        AudioSource objAudioSource = soundObject.GetComponent<AudioSource>();

        while (true)
        {
            if (objAudioSource != null)
            {
                objAudioSource.Play();
                float randomDelay = Random.Range(1f, 5f); // Random delay between 1 and 5 seconds
                yield return new WaitForSeconds(randomDelay);
            }
            else
            {
                yield break;
            }
        }
    }

    void DeactivateAllObjects()
    {
        foreach (GameObject obj in stage1Objects)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in stage2Objects)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in stage3Objects)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in stage4Objects)
        {
            obj.SetActive(false);
        }
        backgroundNoise.SetActive(false);
        if (backgroundNoiseAnimator != null)
        {
            backgroundNoiseAnimator.enabled = false; // Disable Animator
        }
    }
}