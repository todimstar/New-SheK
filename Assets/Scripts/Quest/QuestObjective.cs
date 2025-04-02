using UnityEngine;

[System.Serializable]
public abstract class QuestObjective : MonoBehaviour
{
    public bool isActive = true;
    public QuestData questData;
    public int questID => questData.questID;
    protected void RegisterQuestObjective()
    {
        // 将自己注册到对应的任务中
        QuestManager.Instance.RegisterQuestObjective(this);
    }
}