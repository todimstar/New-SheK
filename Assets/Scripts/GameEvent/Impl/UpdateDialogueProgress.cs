using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 更新任务进度
/// </summary>
public struct UpdateDialogueProgress : IEvent
{
    public void OnEvent(){
        
    }
    public DialogueNode.DialogueLine line;
    public List<DialogueNode.DialogueOption> option;

    public UpdateDialogueProgress(DialogueNode.DialogueLine line,List<DialogueNode.DialogueOption> option=null)
    {
        this.line = line;
        this.option = option;
    }
}