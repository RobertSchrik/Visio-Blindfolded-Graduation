using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeManager : MonoBehaviour
{
#if UNITY_EDITOR
    [CustomEditor(typeof(NodeManager))]
    public class NodeManagerEditor : Editor
    {
        private bool errorHandlingFoldout = true;

        public override void OnInspectorGUI()
        {
            NodeManager script = (NodeManager)target;
            errorHandlingFoldout = EditorGUILayout.Foldout(errorHandlingFoldout, "Error Handling");

            if (errorHandlingFoldout)
            {
                EditorGUI.indentLevel++;

                script.RandomSelectedMasterNode = EditorGUILayout.IntField("RandomSlectedMasterNode", script.RandomSelectedMasterNode);
                EditorGUILayout.LabelField("The startup has failed: ", $"{script.SelectingNodeFailCheck} out of {1} tasks");
                EditorGUILayout.LabelField("Story has Spawned", $"{script.masterCurrentStoryLength + 1} out of {script.masterNodeStoryLength + 1}");

                EditorGUI.indentLevel--;
            }
            DrawDefaultInspector();
        }
    }
#endif

    [Header("Level generation")]
    [SerializeField] SeedManager Seed; 
    System.Random CurrentSeed;

    [Header("Connecting nodes")]
    public List<ScriptableObject> StoryPrompts = new List<ScriptableObject>();
    private int NodePromtListLength;

    [Header("Story")]
    public int StoryProgressionNumber;
    public bool StoryHasReachedWaypoint;
    public string CurrentObjectiveNodeString;

    [Header("Story Tasks")]
    public ScriptableObject CurrentStoryNodeTask;
    public ScriptableObject CurrentStoryEndNode;
    public List<ScriptableObject> ThisStoryTaskList = new List<ScriptableObject>();

    private int RandomSelectedMasterNode;
    private MasterNodeSO masternodeSO;

    private int SelectingNodeFailCheck;
    private int masterNodeStoryLength;
    private int masterCurrentStoryLength;

    void Start()
    {
        CurrentSeed = Seed.LevelSeed;
        SelectingNodeStory();
        StoryProgressionBookmark();
    }
    private void Update()
    {
        StoryProgressionBookmark(); 
    }

    private void SelectingNodeStory()
    {
        if (RandomSelectedMasterNode == 0)
        {
            NodePromtListLength = StoryPrompts.Count;
            RandomSelectedMasterNode = CurrentSeed.Next(0, NodePromtListLength);
            CurrentStoryNodeTask = StoryPrompts[RandomSelectedMasterNode];
            BuildStory(CurrentStoryNodeTask);
        }
        else if (SelectingNodeFailCheck == 0)
        {
            SelectingNodeFailCheck++;
            Debug.LogError("Thrown Exception: Level failed to spawn task, number of tasks: " + NodePromtListLength);
            NodePromtListLength = StoryPrompts.Count;
            RandomSelectedMasterNode = CurrentSeed.Next(0, NodePromtListLength);
            CurrentStoryNodeTask = StoryPrompts[RandomSelectedMasterNode];
            SelectingNodeStory();
        }
        else
        {
            Debug.LogError("Fata/l Error: Level failed to spawn task.");
            return;
        }

    }

    private void BuildStory(ScriptableObject currentNode)
    {
        if (currentNode == null)
        {
            return;
        }

        if (currentNode is MasterNodeSO)
        {
            MasterNodeSO masterNodeSO = (MasterNodeSO)currentNode;
            masterNodeStoryLength = (masterNodeSO.StoryLength + 1);
            ThisStoryTaskList.Add(masterNodeSO);
            CurrentStoryEndNode = masterNodeSO.ConclusionNode;
            List<ScriptableObject> connectingNodes = masterNodeSO.ConntectingNodes;

            if (ThisStoryTaskList.Count < masterNodeStoryLength)
            {
                int randomConnectingNodeIndex = CurrentSeed.Next(0, connectingNodes.Count);
                ScriptableObject nextNode = connectingNodes[randomConnectingNodeIndex];
                ThisStoryTaskList.Add(nextNode);

                BuildStory(nextNode);
            }
            else if (ThisStoryTaskList.Count == masterNodeStoryLength)
            {
                ThisStoryTaskList.Add(masterNodeSO.ConclusionNode);
                masterCurrentStoryLength = masterNodeStoryLength;
                BuildStory(null);
            }
        }
        else if (currentNode is ObjectiveNodeSO)
        {
            ObjectiveNodeSO objectiveNodeSO = (ObjectiveNodeSO)currentNode;
            List<ScriptableObject> connectingNodes = objectiveNodeSO.ConntectingNodes;

            if (ThisStoryTaskList.Count < masterNodeStoryLength)
            {
                int randomConnectingNodeIndex = CurrentSeed.Next(0, connectingNodes.Count);
                ScriptableObject nextNode = connectingNodes[randomConnectingNodeIndex];
                ThisStoryTaskList.Add(nextNode);

                BuildStory(nextNode);
            }
            else if (ThisStoryTaskList.Count == masterNodeStoryLength)
            {
                ThisStoryTaskList.Add(CurrentStoryEndNode);
                masterCurrentStoryLength = masterNodeStoryLength;
                BuildStory(null);
            }
        }
    }

    public void StoryProgressionBookmark()
    {
        if (StoryHasReachedWaypoint && StoryProgressionNumber != masterCurrentStoryLength + 1)
        {
            CurrentStoryNodeTask = ThisStoryTaskList[StoryProgressionNumber];

            CurrentObjectiveNodeString = string.Empty;

            switch (CurrentStoryNodeTask)
            {
                case MasterNodeSO masterNode:
                    CurrentObjectiveNodeString = masterNode.LocationNode;
                    break;
                case ObjectiveNodeSO objectiveNode:
                    CurrentObjectiveNodeString = objectiveNode.LocationNode;
                    break;
                case ConclusionNodeSO conclusionNode:
                    CurrentObjectiveNodeString = conclusionNode.LocationNode;
                    break;
                default:
                    Debug.Log("Failed Fataly.");
                    break;
            }

            StoryProgressionNumber++;
            StoryHasReachedWaypoint = !StoryHasReachedWaypoint;
            Debug.Log(StoryProgressionNumber + "Out of" + masterNodeStoryLength + 1);
        }
        if (StoryHasReachedWaypoint && StoryProgressionNumber == masterNodeStoryLength + 1)
        {
            StoryProgressionNumber++;
            StoryHasReachedWaypoint = !StoryHasReachedWaypoint;
            Debug.LogError("Story has ended.");
        }
    }
}
