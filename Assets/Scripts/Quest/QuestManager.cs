using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class QuestManager : MonoBehaviour
{
    public static QuestManager _instance;
    public static QuestManager Instance => _instance ??= FindObjectOfType<QuestManager>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // 监听 任务目标注册事件
        GameEventBus.Instance.Subscribe<RegisterQuestObjective>(
            RegisterQuestObjectiveHandler
        );
        // 监听 任务进度更新事件
        GameEventBus.Instance.Subscribe<UpdateQuestProgress>(
            UpdateQuestProgressHandler
        );
    }

    private void OnDestroy()
    {
        // 取消监听 任务目标注册事件
        GameEventBus.Instance.Unsubscribe<RegisterQuestObjective>(
            RegisterQuestObjectiveHandler
        );
        // 取消监听 任务进度更新事件
        GameEventBus.Instance.Unsubscribe<UpdateQuestProgress>(
            UpdateQuestProgressHandler
        );
    }
    /// <summary>
    /// 任务进度更新 事件处理
    /// </summary>
    /// <param name="data">封装的任务信息</param>
    private void UpdateQuestProgressHandler(UpdateQuestProgress data)
    {
        UpdateQuestProgress(data.questID);
        Debug.Log($"更新任务 {data.questID} 进度");
    }
    /// <summary>
    /// 任务目标注册 事件处理
    /// </summary>
    /// <param name="data">封装的任务目标信息</param>
    private void RegisterQuestObjectiveHandler(RegisterQuestObjective data)
    {
        RegisterQuestObjective(data.questObjective);

        Debug.Log($"注册 {data.questObjective}");
    }

    /// <summary>
    /// 当前激活的任务
    /// </summary>
    public Dictionary<int, QuestInstance> activeQuest = new();
    /// <summary>
    /// 任务目标表
    /// </summary>
    public Dictionary<int, List<QuestObjective>> questObjectivesTable = new();

    /// <summary>
    /// 持久化任务
    /// </summary>
    public void SaveQuest() { }

    /* 任务周期 */

    /// <summary>
    /// 尝试接受任务
    /// </summary>
    /// <param name="quest">任务信息</param>
    /// <returns>成功接受就返回 true ，失败就返回 false </returns>
    private bool TryAcceptQuest(QuestData quest)
    {
        foreach (var perQuest in quest.preQuests)
        {
            if (!activeQuest.ContainsKey(perQuest.questID))
            {
                return false;
            }
            else
            {
                // 判断前置任务是否完成
                if (activeQuest[perQuest.questID].currentObjective < activeQuest[perQuest.questID].requireObjective){
                    return false;
                }
            }
        }
        return true;
    }
    /// <summary>
    /// 接受任务
    /// </summary>
    /// <param name="quest">任务信息</param>
    public void AcceptQuest(QuestData quest)
    {
        if (!TryAcceptQuest(quest)) return;

        QuestInstance questInstance = new QuestInstance(
            quest.questID,
            questObjectivesTable[quest.questID].Count,       // 任务目标数量
            0
        );

        activeQuest.Add(questInstance.questID, questInstance);

        foreach (var objective in questObjectivesTable[quest.questID])
        {
            objective.isActive = true;
        }

        Debug.Log($"成功接受任务: {quest.questName}");
    }
    /// <summary>
    /// 更新任务进度
    /// </summary>
    /// <param name="questID">任务的 id </param>
    public void UpdateQuestProgress(int questID)
    {
        QuestInstance questInstance = activeQuest[questID];

        questInstance.currentObjective++;

        if (questInstance.currentObjective >= questInstance.requireObjective)
        {
            CompleteQuest(questID);
        }
    }
    /// <summary>
    /// 完成任务 需要进行的事件
    /// </summary>
    /// <param name="questID">任务的 id </param>
    private void CompleteQuest(int questID)
    {
        Debug.Log($"{this.activeQuest[questID].questID}任务完成");
        // TODO: 任务完成
    }

    // 任务目标周期

    /// <summary>
    /// 注册任务目标
    /// </summary>
    /// <param name="objective">任务目标</param>
    public void RegisterQuestObjective(QuestObjective objective)
    {
        objective.isActive = false;
        if (!questObjectivesTable.ContainsKey(objective.questID))
        {
            questObjectivesTable.Add(objective.questID, new List<QuestObjective>());
        }
        questObjectivesTable[objective.questID].Add(objective);
    }
    /// <summary>
    /// 移除任务目标
    /// </summary>
    /// <param name="objective"></param>
    public void UnRegisterQuestObjective(int questID)
    {
        questObjectivesTable.Remove(questID);
    }
}