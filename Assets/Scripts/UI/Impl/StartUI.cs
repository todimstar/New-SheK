using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏初始面板
/// </summary>
public class StartUI : BasePanel
{
    private void Start()
    {
        Init();
        // 隐藏所有面板
        PanelManager.Instance.CloseAllPanels();
        //开启开始面板
        PanelManager.Instance.OpenPanel(typeof(StartUI));
    }

    public void Click_StartBtn()
    {
        // 开始游戏按钮点击事件
        Debug.Log("开始游戏按钮点击事件");

        // 加载场景
        // LoadGameScene();

        //关闭开始面板并显示HUD
        PanelManager.Instance.CloseAllPanels();
        PanelManager.Instance.ClosePanel(typeof(StartUI));
        PanelManager.Instance.OpenPanel(typeof(PlayerHUDUI));
        // 恢复游戏时间
        Time.timeScale = 1f;
        PlayStopUI.isGamePaused = false;
    }


    public void Click_ContiueBtn()
    {
        // 继续游戏按钮点击事件
        Debug.Log("继续游戏按钮点击事件");
        //之后接到存档系统，再进行处理
    }

    public void Click_SettingBtn()
    {
        // 设置按钮点击事件
        Debug.Log("设置按钮点击事件");
        PanelManager.Instance.OpenPanel(typeof(SettingUI)); //2025.04.02 统一设置面板
    }

    public void Click_ExitBtn()
    {
        // 退出按钮点击事件
        Debug.Log("退出按钮点击事件");
        //测试一下关闭后有没有多余的东西
        PanelManager.Instance.ClosePanel(typeof(StartUI));
    }

    /// <summary>
    /// 加载游戏场景
    /// </summary>
    /// <remarks>
    /// 需要依靠场景管理器加载场景，在Build Settings中先设置好场景顺序。
    /// 确保在Build Settings中已正确设置场景顺序，否则可能加载错误的场景。
    /// </remarks>
    // private void LoadGameScene()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //     Debug.Log("加载游戏场景");
    // }
}