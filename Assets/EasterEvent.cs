using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEvent : MonoBehaviour
{
    public List<GameObject> firstEggList;
    public List<GameObject> secondEggList;

    // Start is called before the first frame update
    void Start()
    {
        ActivateNthEggs(firstEggList, 2);
        ActivateNthEggs(secondEggList, 3);
    }

    void ActivateNthEggs(List<GameObject> eggList, int numToActivate)
    {
        // Shuffle the egg list using Fisher-Yates algorithm
        for (int i = 0; i < eggList.Count; i++)
        {
            int randomIndex = Random.Range(i, eggList.Count);
            GameObject temp = eggList[i];
            eggList[i] = eggList[randomIndex];
            eggList[randomIndex] = temp;
        }

        // Activate the first 'numToActivate' eggs in the shuffled list
        for (int i = 0; i < eggList.Count; i++)
        {
            if (i < numToActivate)
            {
                eggList[i].SetActive(true);
            }
            else
            {
                eggList[i].SetActive(false);
            }
        }
    }
}
