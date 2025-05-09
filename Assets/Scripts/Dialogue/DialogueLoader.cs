using System.Collections.Generic;
using UnityEngine;

public class DialogueLoader
{
    /// <summary>用来缓冲json数据结构的类，应该不会在其他地方用到</summary>
    protected class DialogueCache
    {
        /// <summary>一次对话中所有的节点</summary>
        public List<DialogueNode> dialogue;
    }
    /// <summary>
    /// 加载对话数据
    /// </summary>
    /// <param name="fileName">对话文件名，该文件应该存放在Assets/Resources/Dialogue目录下</param>
    /// <returns>已经加载好的对话数据</returns>
    public static DialogueData LoadDialogueData(string fileName)
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>($"Dialogue/{fileName}");

        if (jsonAsset == null)
        {
            Debug.LogError($"Dialogue Load ERROR: {fileName} not found");
            return null;
        }

        DialogueCache nodes = JsonUtility.FromJson<DialogueCache>(jsonAsset.text);

        if (nodes == null && nodes.dialogue == null)
        {
            Debug.LogError("Dialogue Load ERROR: Dialogue is null");
            return null;
        }

        DialogueData data = new DialogueData();

        if (data.nodes == null)
        {
            data.nodes = new Dictionary<string, DialogueNode>();
        }

        foreach (DialogueNode node in nodes.dialogue)
        {
            if (node.id.Equals("entry"))
            {
                data.entryNode = node;
            }
            data.nodes.Add(node.id, node);
        }
        return data;
    }

}