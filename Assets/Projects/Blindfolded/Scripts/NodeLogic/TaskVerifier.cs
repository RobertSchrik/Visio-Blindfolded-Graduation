using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskVerifier : MonoBehaviour
{
    public string Task_Number;
    public NodeManager nodeManager;
    [SerializeField]bool RequireNodeContact;
    [SerializeField]bool RequireEventCompleted;
    bool EventCompleted = false;
    bool NodeContacted = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NodeContacted = true;
            if (!RequireEventCompleted || RequireEventCompleted && EventCompleted)
            {
                CompleteTask();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NodeContacted = false;
        }
    }

    public void EventComplete()
    {
        EventCompleted = true;
        if (!RequireNodeContact || RequireNodeContact && NodeContacted)
        {
            CompleteTask();
        }
    }

    void CompleteTask()
    {
        if (nodeManager.StoryProgressionNumber != 7 && nodeManager.CurrentObjectiveNodeString == Task_Number)
        {
            Debug.Log("Hit");
            nodeManager.StoryHasReachedWaypoint = (true);
            nodeManager.StoryProgressionBookmark();
        }
    }
}
