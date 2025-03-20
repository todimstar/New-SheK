using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板管理器
/// </summary>
/// <remarks>
/// 记得在场景中创建一个空物体，挂载这个脚本，作为面板管理器
/// 然后调整脚本的执行顺序，确保在其他脚本之前执行
/// </remarks>
public class PanelManager : MonoBehaviour
{

    /// <summary>
    /// 面板层级
    /// </summary>
    public enum PanelLayer
    {
        Normal,
        PopWindow,
    }
    private static PanelManager _instance;//单例
    public static PanelManager Instance => _instance ??= FindObjectOfType<PanelManager>();

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

    /// <summary>
    /// 面板字典
    /// </summary>
    private Dictionary<Type, BasePanel> panelDict = new();

    /// <summary>
    /// 注册面板进字典
    /// </summary>
    public void RegisterPanel(BasePanel panel)
    {
        if (panelDict.ContainsKey(typeof(BasePanel)))
        {
            return;
        }
        panelDict.Add(typeof(BasePanel), panel);
    }

    /// <summary>
    /// UI层次对应的面板栈
    /// </summary>
    private Dictionary<PanelLayer, Stack<BasePanel>> UITable = new();
    /// <summary>
    /// 显示面板方法
    /// </summary>
    /// <param name="panel"></param>
    public void OpenPanel(Type panel, PanelLayer layer = PanelLayer.Normal)
    {
        if (layer == PanelLayer.PopWindow)
        {
            if (!UITable.ContainsKey(layer))
            {
                UITable.Add(layer, new Stack<BasePanel>());
            }
            UITable[layer].Push(panelDict[panel]);
            panelDict[panel].Show();
            return;
        }
        else
        {
            if (UITable[layer].Count > 0)
            {
                BasePanel topPanel = UITable[layer].Peek();
                topPanel.Hide();
            }
            UITable[layer].Push(panelDict[panel]);
            UITable[layer].Peek().Show();
        }

    }

    /// <summary>
    /// 关闭面板方法
    /// </summary>
    public void ClosePanel(Type panel, PanelLayer layer = PanelLayer.Normal)
    {
        if (UITable[layer].Count == 0)
        {
            return;
        }
        if (UITable[layer].Peek().GetType() == panel)//第一个就是，直接弹出并隐藏
        {
            UITable[layer].Pop().Hide();
            if (UITable[layer].Count > 0)
            {
                UITable[layer].Peek().Show();
            }
        }
        else//在更深层的话，需要向下寻找删除隐藏并恢复其余上层面板怪怪的逻辑，之后看效果再说
        {
            Stack<BasePanel> temp = new();
            while (UITable[layer].Count > 0)
            {
                BasePanel topPanel = UITable[layer].Pop();
                if (topPanel.GetType() == panel)      
                {   
                    topPanel.Hide();
                    break;
                }
                temp.Push(topPanel);
            }
            while (temp.Count > 0)
            {
                UITable[layer].Push(temp.Pop());
            }
        }
    }

    /// <summary>
    /// 隐藏所有面板
    /// </summary>
    /// <param name="layer"></param>
    public void CloseAllPanels(PanelLayer layer = PanelLayer.Normal)
    {
        if (UITable.ContainsKey(layer))
        {
            while (UITable[layer].Count > 0)
            {
                UITable[layer].Pop().Hide();
            }
        }
    }

}