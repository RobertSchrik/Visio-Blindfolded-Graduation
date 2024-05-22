using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryNodes", menuName = "ScriptableObjects/MasterNode", order = 1)]
public class MasterNodeSO : ScriptableObject
{
    [Header("Connecting nodes")]
    public List<ScriptableObject> ConntectingNodes = new List<ScriptableObject>();
    [Range(2,4)] public int StoryLength;

    [Header("Ending Node")]
    public ScriptableObject ConclusionNode;

    [Header("Master Node Location")]
    public string LocationNode;

    [Header("Audio List")]
    public List<AudioClip> VocalProgressionLog;
}
