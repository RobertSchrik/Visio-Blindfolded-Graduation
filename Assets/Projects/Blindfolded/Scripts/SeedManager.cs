using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{

    public System.Random LevelSeed;
    [SerializeField] int Seed;
    [SerializeField] bool AutoGenerateSeed;

    // Start is called before the first frame update
    void Start()
    {
        if (AutoGenerateSeed)
        {
            Seed = Random.Range(0, int.MaxValue);
        }

        LevelSeed = new System.Random(Seed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
