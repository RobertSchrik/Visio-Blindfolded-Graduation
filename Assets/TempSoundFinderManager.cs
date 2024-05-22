using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSoundFinderManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Nodes;

    [SerializeField] AudioSource SoundObj;
    [SerializeField] AudioSource VictorySoundObj;


    bool Init = false;
    // Start is called before the first frame update
    void Start()
    {
        ResetMinigame();
    }

    public void ResetMinigame()
    {
        if (Init)
        {
            VictorySoundObj.Play();
        }
        else
        {
            Init = true;
        }
        SoundObj.transform.position = Nodes[Random.Range(0, Nodes.Count)].transform.position;
    }
}
