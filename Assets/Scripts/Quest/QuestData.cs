using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "new Quest Data", menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public int questID => this.GetInstanceID();

    [Header("任务信息")]
    public string questName;
    [TextArea] public string questDesc = "任务描述";

    [Header("前置任务")]
    public List<QuestData> preQuests;

    private void Awake()
    {
        // 如果任务名为空，则从文件名中获取
        if (string.IsNullOrEmpty(questName))
        {
            questName = Path.GetFileNameWithoutExtension(
                AssetDatabase.GetAssetPath(this.GetInstanceID())
                );
        }
    }
}