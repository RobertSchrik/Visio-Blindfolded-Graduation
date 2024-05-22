using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFindObjectV2 : MinigameBase
{

    [SerializeField] GameObject ObjectToFind;

    [SerializeField] List<GameObject> Interiors;

    [SerializeField] List<GameObject> SpawnPoints;

    [SerializeField] AudioSource SoundObj;
    [SerializeField] AudioSource VictorySoundObj;

    [SerializeField] int ScoreReqForComplete = 0;

    bool Init = false;

    int CurrentScore;

    // Start is called before the first frame update
    void Start()
    {
        if (SpawnPoints.Count <= 0 && Interiors.Count > 0)
        {
            GameObject interior = Instantiate(Interiors[Random.Range(0, Interiors.Count)]);
            interior.transform.SetParent(transform);
            interior.transform.position = transform.position;
            foreach (Transform spawnpoint in interior.transform.GetChild(0))
            {
                SpawnPoints.Add(spawnpoint.gameObject);
            }
        }else
        {
            Debug.LogWarning("No spawnpoints or interiors set.");
        }
        ResetMinigame();
    }

    public void CompleteMinigame()
    {
        gameObject.SetActive(false);
    }

    public void ResetMinigame()
    {
        VictorySoundObj.transform.position = ObjectToFind.transform.position;
        if (Init)
        {
            VictorySoundObj.Play();
            CurrentScore++;

            if (CurrentScore >= ScoreReqForComplete)
            {
                OnMinigameComplete.Invoke();
            }

        }else
        {
            Init = true;
        }

        ObjectToFind.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position;
        ObjectToFind.SetActive(true);

    }
}
