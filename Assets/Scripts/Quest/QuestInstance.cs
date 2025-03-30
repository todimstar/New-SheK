using UnityEngine;

[System.Serializable]
public class QuestInstance : MonoBehaviour
{
    public int questID;
    public int requireObjective = 0;
    public int currentObjective = 0;

    public QuestInstance(int questID,int requireObjective,int currentObjective){
        this.questID = questID;
        this.requireObjective = requireObjective;
        this.currentObjective = currentObjective;
    }
}