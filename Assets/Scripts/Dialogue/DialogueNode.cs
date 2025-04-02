using System.Collections.Generic;


/// <summary>存储一次对话，包含可能有的台词，和可能有的选项</summary>
[System.Serializable]
public class DialogueNode
{
    /// <summary>单句对话</summary>
    [System.Serializable]
    public class DialogueLine
    {
        /// <summary>角色名</summary>
        public string speaker;
        /// <summary>单次对话的文本</summary>
        public string text;
    }
    /// <summary>对话结束的选项</summary>
    [System.Serializable]
    public class DialogueOption
    {
        /// <summary>选项文本</summary>
        public string optionText;
        /// <summary>对应的下一个对话节点</summary>
        public string nextNodeId;
    }
    /// <summary>对话节点的唯一索引id</summary>
    public string id;
    /// <summary>整个对话的所有文本</summary>
    public List<DialogueLine> lines;
    public List<DialogueOption> options;
}