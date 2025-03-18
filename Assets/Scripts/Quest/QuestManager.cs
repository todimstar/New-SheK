using UnityEngine;
using System.Collections.Generic;

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
    }

    private void Start()
    {
        // 监听 任务目标注册事件
        GameEventBus.Instance.Subscribe<RegisterQuestObjective>(
            RegisterQuestObjectiveHandler
        );
        GameEventBus.Instance.Subscribe<UpdateQuestProgress>(
            UpdateQuestProgressHandler
        );
    }

    private void OnDestroy()
    {
        GameEventBus.Instance.Unsubscribe<RegisterQuestObjective>(
            RegisterQuestObjectiveHandler
        );
        GameEventBus.Instance.Unsubscribe<UpdateQuestProgress>(
            UpdateQuestProgressHandler
        );
    }

    private void UpdateQuestProgressHandler(UpdateQuestProgress data)
    {
        UpdateQuestProgress(data.questID);
        Debug.Log($"更新任务 {data.questID} 进度");
    }

    private void RegisterQuestObjectiveHandler(RegisterQuestObjective data)
    {
        RegisterQuestObjective(data.questObjective);

        Debug.Log($"注册 {data.questObjective}");
    }

    public Dictionary<int, QuestInstance> activeQuest = new();     // 当前激活的任务
    public Dictionary<int, List<QuestObjective>> questObjectives = new();      // 任务目标

    // 持久化任务
    public void SaveQuest() { }

    // 任务周期
    private bool TryAcceptQuest(QuestData quest)
    {
        foreach (var perQuest in quest.preQuests)
        {
            if (!activeQuest.ContainsKey(perQuest.questID))
            {
                return false;
            }
        }
        return true;
    }

    public void AcceptQuest(QuestData quest)
    {
        if (!TryAcceptQuest(quest)) return;

        QuestInstance questInstance = new QuestInstance(
            quest.questID,
            questObjectives[quest.questID].Count,       // 任务目标数量
            0
        );

        activeQuest.Add(questInstance.questID, questInstance);

        foreach (var objective in questObjectives[quest.questID])
        {
            objective.isActive=true;
        }

        Debug.Log($"成功接受任务: {quest.questName}");
    }

    public void UpdateQuestProgress(int questID)
    {
        QuestInstance questInstance = activeQuest[questID];

        questInstance.currentObjective++;

        if (questInstance.currentObjective >= questInstance.requireObjective)
        {
            CompleteQuest(questID);
        }
    }

    private void CompleteQuest(int questID)
    {
        Debug.Log($"{this.activeQuest[questID].questID}任务完成");
        // TODO: 任务完成
        activeQuest.Remove(questID);
    }

    // 任务目标周期
    public void RegisterQuestObjective(QuestObjective objective)
    {
        objective.isActive=false;
        if (!questObjectives.ContainsKey(objective.questID))
        {
            questObjectives.Add(objective.questID, new List<QuestObjective>());
        }
        questObjectives[objective.questID].Add(objective);
    }
}