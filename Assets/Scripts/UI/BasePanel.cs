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
        PanelManager.Instance.RegisterPanel(this);
    }
    public virtual void Show()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
}//2025.03.20 15:48 对比一下两套UI框架代码，选择性吸收
//2025.03.22 14:31 尝试使用PanelManager管理UI面板，学习UI框架的设计思想