using UnityEngine;


/// <summary>
/// 注册任务目标对象
/// </summary>
public struct RegisterQuestObjective : IEvent
{
    /// <summary>
    /// 任务目标对象
    /// </summary>
    public QuestObjective questObjective;
    public RegisterQuestObjective(QuestObjective questObjective)
    {
        this.questObjective = questObjective;
    }
    public void OnEvent()
    {
        Debug.Log($"注册 {questObjective.questData.questName} 任务的目标对象");
    }
}