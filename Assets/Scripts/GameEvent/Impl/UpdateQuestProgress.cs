using UnityEngine;

/// <summary>
/// 更新任务进度
/// </summary>
public struct UpdateQuestProgress : IEvent
{
    public void OnEvent(){
        
    }
    /// <summary>
    /// 任务ID
    /// </summary>
    public int questID;
    public UpdateQuestProgress(int questID){
        this.questID = questID;
    }
}