using UnityEngine;


/// <summary>
/// 所有面板的基类
/// </summary>
public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 将面板注册到UIManager
    /// </summary>
    public void Init()
    {
        Debug.Log($"BasePanel.Init: 注册面板 {this.GetType().Name}");
        PanelManager.Instance.RegisterPanel(this);
    }
    public virtual void Show()
    {
        Debug.Log($"BasePanel.Show: 显示面板 {this.GetType().Name}");
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        Debug.Log($"BasePanel.Hide: 隐藏面板 {this.GetType().Name}");
        this.gameObject.SetActive(false);
    }
}//2025.03.20 15:48 对比一下两套UI框架代码，选择性吸收
//2025.03.22 14:31 尝试使用PanelManager管理UI面板，学习UI框架的设计思想