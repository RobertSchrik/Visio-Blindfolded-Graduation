using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryNodes", menuName = "ScriptableObjects/ConclusionNode", order = 3)]
public class ConclusionNodeSO : ScriptableObject
{
    [Header("Master Node Location")]
    public string LocationNode;

    [Header("Audio List")]
    public List<AudioClip> VocalProgressionLog;
}
