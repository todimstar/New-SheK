using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏初始面板
/// </summary>
public class StartUI : BasePanel
{
    private void Awake() {
        Init();
        // 隐藏所有面板
        PanelManager.Instance.CloseAllPanels();
        //开启开始面板
        PanelManager.Instance.OpenPanel(typeof(StartUI));
    }

    public void StartBtn_Event_Open()
    {
        // 开始游戏按钮点击事件
        Debug.Log("开始游戏按钮点击事件");
        LoadGameScene();
    }

    public void OnContinueGameButtonClick()
    {
        // 继续游戏按钮点击事件
        Debug.Log("继续游戏按钮点击事件");
        //之后接到存档系统，再进行处理
    }

    public void SettingtBtn_Event_Open()
    {
        // 设置按钮点击事件
        Debug.Log("设置按钮点击事件");
        PanelManager.Instance.OpenPanel(typeof(SettingUI));
    }

    /// <summary>
    /// 加载游戏场景
    /// </summary>
    /// <remarks>
    /// 需要依靠场景管理器加载场景，在Build Settings中先设置好场景顺序。
    /// 确保在Build Settings中已正确设置场景顺序，否则可能加载错误的场景。
    /// </remarks>
    private void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("加载游戏场景");
    }
}