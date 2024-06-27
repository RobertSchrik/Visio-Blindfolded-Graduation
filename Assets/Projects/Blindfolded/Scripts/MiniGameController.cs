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
    private List<GameObject> activeStageObjects = new List<GameObject>();
    public AudioClip collectSound;
    public AudioSource audioSource;
    public TextMeshProUGUI victoryText;

    private int objectsTouched = 0;
    private bool tutorialCompleted = false;
    private GameObject currentAudioPlayingObject;

    // Tutorial audio clips
    public AudioClip introClip;
    public AudioClip tutorialPart1Clip;
    public AudioClip tutorialPart1CompleteClip;
    public AudioClip tutorialPart2Clip;
    public AudioClip tutorialPart2ClipPart2;
    public AudioClip tutorialPart2CompleteClip;
    public AudioClip tutorialPart3Clip;
    public AudioClip tutorialPart3SuccessClip;
    public AudioClip tutorialPart3FailClip;

    // Level audio clips
    public AudioClip level1StartClip;
    public AudioClip level2StartClip;
    public AudioClip level3StartClip;
    public AudioClip level4StartClip;

    // Game Complete clip
    public AudioClip gameCompletionClip;

    public Transform doorEntrance;
    public Transform roomCenter;
    public AudioSource eggAudioSource;
    public GameObject[] tutorialEggLocations;

    public GameObject blockade_1;
    public GameObject blockade_2;
    public GameObject blockade_3;

    public GameObject audioMainSource;

    private bool isStarted = false;


    void Start()
    {
        DeactivateAllObjects();
        victoryText.gameObject.SetActive(false); 
    }

    void Update()
    {
        if (!isStarted && IsAnyButtonPressed())
        {
            isStarted = true;
            StartCoroutine(TutorialSequence());
        }

        if (tutorialCompleted)
        {
            HandleGameplay();
        }
    }

    bool IsAnyButtonPressed()
    {
        // Check if any of the Oculus buttons are pressed
        return OVRInput.GetDown(OVRInput.Button.Any);
    }

    IEnumerator TutorialSequence()
    {
        // Introduction
        audioSource.PlayOneShot(introClip);
        yield return new WaitForSeconds(introClip.length + 1f);

        // Tutorial Part 1
        audioSource.PlayOneShot(tutorialPart1Clip);
        yield return new WaitForSeconds(tutorialPart1Clip.length + 1f);
        yield return StartCoroutine(Task_WalkToLocation(doorEntrance.position));
        audioSource.PlayOneShot(tutorialPart1CompleteClip);
        yield return new WaitForSeconds(tutorialPart1CompleteClip.length + 1f);

        // Tutorial Part 2
        audioSource.PlayOneShot(tutorialPart2Clip);
        yield return new WaitForSeconds(tutorialPart2Clip.length + 1f);
        audioSource.PlayOneShot(tutorialPart2ClipPart2);
        yield return new WaitForSeconds(tutorialPart2ClipPart2.length + 15f);
        audioSource.PlayOneShot(tutorialPart2CompleteClip);
        yield return new WaitForSeconds(tutorialPart2CompleteClip.length + 1f);
        yield return StartCoroutine(Task_WalkToLocation(roomCenter.position));

        // Tutorial Part 3
        audioSource.PlayOneShot(tutorialPart3Clip);
        yield return new WaitForSeconds(tutorialPart3Clip.length + 1f);
        yield return StartCoroutine(Task_FindEgg());

        // Mark tutorial as completed
        tutorialCompleted = true;

        // Start the game
        StartStage(currentStage);
    }

    IEnumerator Task_WalkToLocation(Vector3 targetPosition)
    {
        while (Vector3.Distance(player.transform.position, targetPosition) > 1f)
        {
            yield return null;
        }
    }

    IEnumerator Task_FindEgg()
    {
        bool eggFound = false;
        eggAudioSource.gameObject.SetActive(true);
        eggAudioSource.Play();

        while (!eggFound)
        {
            float distance = Vector3.Distance(player.transform.position, eggAudioSource.transform.position);
            if (distance <= 1f)
            {
                eggFound = true;
                eggAudioSource.gameObject.SetActive(false);
                audioSource.PlayOneShot(tutorialPart3SuccessClip);
                yield return new WaitForSeconds(tutorialPart3SuccessClip.length + 1f);
                //audioMainSource.transform.Rotate(0f, 180f, 0f);
            }
            else
            {
                yield return null;
            }
        }

        if (!eggFound)
        {
            audioSource.PlayOneShot(tutorialPart3FailClip);
            yield return new WaitForSeconds(tutorialPart3FailClip.length + 1f);
            yield return StartCoroutine(Task_FindEgg());
        }
    }

    void HandleGameplay()
    {
        if (currentStageObjects != null)
        {
            GameObject closestObject = null;
            float closestDistance = float.MaxValue;

            foreach (GameObject obj in activeStageObjects)
            {
                if (!obj.activeSelf)
                    continue;

                float distance = Vector3.Distance(player.transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }

                if (currentStage == 2 || currentStage == 4)
                {
                    SetMeshRenderersEnabled(obj, distance <= detectionRadius);
                }

                if (distance <= 1f && obj.activeSelf)
                {
                    // Check if the object is already being collected
                    Animator animator = obj.GetComponent<Animator>();
                    if (animator != null && !animator.GetBool("PresentFound"))
                    {
                        StartCoroutine(PlayCollectAnimationAndDisable(obj));
                    }
                }
            }

            if (closestObject != null && closestObject != currentAudioPlayingObject)
            {
                if (currentAudioPlayingObject != null)
                {
                    // Stop the previous audio source if any
                    currentAudioPlayingObject.GetComponent<AudioSource>().Stop();
                }

                currentAudioPlayingObject = closestObject;
                currentAudioPlayingObject.GetComponent<AudioSource>().Play();
            }

            if (objectsTouched >= 3) // Now only 3 objects are required
            {
                if (currentStage < 5)
                {
                    NextStage();
                }
                return;
            }
        }
    }

    void SetMeshRenderersEnabled(GameObject obj, bool enabled)
    {
        MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = enabled;
        }
    }

    IEnumerator PlayCollectAnimationAndDisable(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            // Set the PresentFound flag to true to mark this object as being collected
            animator.SetBool("PresentFound", true);
            audioSource.PlayOneShot(collectSound);

            // Mute the object's audio source
            AudioSource objAudioSource = obj.GetComponent<AudioSource>();
            if (objAudioSource != null)
            {
                objAudioSource.mute = true;
            }

            // Wait for the animation to complete
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 4f);

            // Disable the object after the animation completes
            obj.SetActive(false);
            objectsTouched++;
        }
    }


    void StartStage(int stage)
    {
        objectsTouched = 0;

        switch (stage)
        {
            case 1:
                currentStageObjects = stage1Objects;
                audioSource.PlayOneShot(level1StartClip);
                break;
            case 2:
                currentStageObjects = stage2Objects;
                audioSource.PlayOneShot(level2StartClip);
                break;
            case 3:
                currentStageObjects = stage3Objects;
                backgroundNoise.SetActive(true);
                audioSource.PlayOneShot(level3StartClip);
                break;
            case 4:
                currentStageObjects = stage4Objects;
                backgroundNoise.SetActive(true);
                audioSource.PlayOneShot(level4StartClip);
                break;
        }

        activeStageObjects.Clear();
        List<GameObject> shuffledObjects = new List<GameObject>(currentStageObjects);
        Shuffle(shuffledObjects);
        activeStageObjects.AddRange(shuffledObjects.GetRange(0, 3));

        foreach (GameObject obj in activeStageObjects)
        {
            obj.SetActive(true);
            if (stage == 2 || stage == 4)
            {
                SetMeshRenderersEnabled(obj, false);
            }

            // Ensure each object has an AudioSource component
            if (obj.GetComponent<AudioSource>() == null)
            {
                obj.AddComponent<AudioSource>();
            }
        }
    }

    void NextStage()
    {
        foreach (GameObject obj in activeStageObjects)
        {
            obj.SetActive(false);
        }
        backgroundNoise.SetActive(false);

        currentStage++;

        if (currentStage > 4)
        {
            victoryText.gameObject.SetActive(true);
            audioSource.PlayOneShot(gameCompletionClip);
            blockade_1.SetActive(false);
            blockade_2.SetActive(false);
            blockade_3.SetActive(false);
            Debug.Log("Game Completed!");
        }
        else
        {
            StartStage(currentStage);
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
    }

    void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
