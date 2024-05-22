using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryNodes", menuName = "ScriptableObjects/ObjectiveNode", order = 2)]
public class ObjectiveNodeSO : ScriptableObject
{
    [Header("Connecting nodes")]
    public List<ScriptableObject> ConntectingNodes = new List<ScriptableObject>();

    [Header("Master Node Location")]
    public string LocationNode;

    [Header("Audio List")]
    public List<AudioClip> VocalProgressionLog;
}
