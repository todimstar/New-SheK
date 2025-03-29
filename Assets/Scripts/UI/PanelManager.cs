using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Debug.LogWarning("面板管理器已存在，销毁新的");
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // 初始化所有层级的栈
            foreach (PanelLayer layer in Enum.GetValues(typeof(PanelLayer)))
            {
                UITable[layer] = new Stack<BasePanel>();
            }
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
        // 获取面板实际运行时类型，而不是编译时类型
        Type panelType = panel.GetType();
        Debug.Log("注册面板: " + panelType.Name);

        // 检查该类型是否已经注册过
        if (panelDict.ContainsKey(panelType))
        {
            Debug.Log("面板已存在，不重复注册: " + panelType.Name);
            return;
        }

        // 以面板的实际类型作为键，添加到字典
        panelDict.Add(panelType, panel);

        // 打印当前注册的所有面板
        Debug.Log("当前已注册面板数量: " + panelDict.Count);
        foreach (var key in panelDict.Keys)
        {
            Debug.Log("- 已注册: " + key.Name + " -> " + panelDict[key].name);
        }
    }

    #region 切换场景时用于销毁为空的已注册的面板

    // 在PanelManager类中添加
    private void OnEnable()
    {
        // 订阅场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 取消订阅场景加载事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 专门清理已销毁的面板引用
    private void CleanupDestroyedPanels()
    {
        List<Type> keysToRemove = new List<Type>();

        foreach (var key in panelDict.Keys)
        {
            if (panelDict[key] == null)
                keysToRemove.Add(key);
        }

        foreach (var key in keysToRemove)
        {
            Debug.Log("清理已销毁的面板: " + key.Name);
            panelDict.Remove(key);
        }
    }

    // 当任何场景加载完成时会调用此方法
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"场景 {scene.name} 已加载，正在重置UI系统");

        // 清理无效的面板引用
        CleanupDestroyedPanels();

        // 重置UI栈
        foreach (PanelLayer layer in Enum.GetValues(typeof(PanelLayer)))
        {
            if (UITable.ContainsKey(layer))
            {
                UITable[layer].Clear();
            }
        }
    }

    #endregion

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
        Debug.Log("尝试打开面板: " + panel.Name);

        // 确保该层级的栈已经初始化
        if (!UITable.ContainsKey(layer))
        {
            UITable[layer] = new Stack<BasePanel>();
        }

        // 确保面板已注册
        if (!panelDict.ContainsKey(panel))
        {
            Debug.LogError($"层级{layer}的面板 {panel.Name} 尚未注册！");

            // 打印当前注册的所有面板
            Debug.Log($"层级{layer}当前已注册面板: ");
            foreach (var key in panelDict.Keys)
            {
                Debug.Log("- 已注册: " + key.Name);
            }
            return;
        }

        Debug.Log($"面板是层级{layer}");
        // 如果是PopWindow层级，直接显示
        if (layer == PanelLayer.PopWindow)
        {
            UITable[layer].Push(panelDict[panel]);
            panelDict[panel].Show();
            return;
        }
        // 如果是Normal层级，需要隐藏上一个面板
        else
        {
            if (UITable[layer].Count > 0)
            {
                BasePanel topPanel = UITable[layer].Peek();
                topPanel.Hide();
                Debug.Log($"隐藏了上一个面板{topPanel.GetType().Name}");
            }
            //当前面板入栈并展示
            UITable[layer].Push(panelDict[panel]);
            UITable[layer].Peek().Show();
        }
    }

    /// <summary>
    /// 关闭面板方法
    /// </summary>
    public void ClosePanel(Type panel, PanelLayer layer = PanelLayer.Normal)
    {
        Debug.Log("尝试关闭面板: " + panel.Name);

        // 确保该层级的栈已经初始化
        if (!UITable.ContainsKey(layer) || UITable[layer].Count == 0)
        {
            Debug.LogWarning("关闭面板失败，栈为空或未初始化");
            return;
        }

        BasePanel topPanel = UITable[layer].Peek();
        Debug.Log("栈顶面板类型: " + topPanel.GetType().Name);

        if (UITable[layer].Peek().GetType() == panel)//第一个就是，直接弹出并隐藏
        {
            Debug.Log("找到匹配面板，正在关闭: " + panel.Name);
            UITable[layer].Pop().Hide();
            if (UITable[layer].Count > 0)
            {
                UITable[layer].Peek().Show();
            }
        }
        else//在更深层的话，需要向下寻找删除隐藏并恢复其余上层面板怪怪的逻辑，之后看效果再说
        {
            Debug.LogWarning("未在栈顶找到匹配面板，尝试在栈内查找");
            Stack<BasePanel> temp = new();
            while (UITable[layer].Count > 0)
            {
                BasePanel currentPanel = UITable[layer].Pop();
                Debug.Log("检查面板: " + currentPanel.GetType().Name);

                if (currentPanel.GetType() == panel)
                {
                    Debug.Log("在栈内找到匹配面板，正在关闭: " + panel.Name);
                    currentPanel.Hide();
                    break;
                }
                temp.Push(currentPanel);
            }
            Debug.Log("恢复其他面板到栈中");
            while (temp.Count > 0)
            {
                UITable[layer].Push(temp.Pop());
            }
        }
    }


    /// <summary>
    /// 隐藏指定层级的所有面板
    /// </summary>
    /// <param name="layer">要隐藏的面板层级</param>
    public void CloseAllPanels(PanelLayer layer)
    {
        if (UITable.ContainsKey(layer))
        {
            while (UITable[layer].Count > 0)
            {
                UITable[layer].Pop().Hide();
            }
        }
    }

    /// <summary>
    /// 隐藏所有层级的所有面板
    /// </summary>
    public void CloseAllPanels()
    {
        foreach (PanelLayer layer in Enum.GetValues(typeof(PanelLayer)))
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

}