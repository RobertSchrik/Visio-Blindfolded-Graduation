using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MinigameBase))]
public class MinigameFindObject : MonoBehaviour
{

    [SerializeField] GameObject ObjectToFind;

    [SerializeField] float RangeX;
    [SerializeField] float RangeZ;

    // Start is called before the first frame update
    void Start()
    {
        StartMinigame();
    }

    void StartMinigame()
    {
        ObjectToFind.transform.position = new Vector3(transform.position.x + Random.Range(-RangeX, RangeX), transform.position.y, transform.position.z + Random.Range(-RangeZ, RangeZ));
        ObjectToFind.SetActive(true);
    }
}
