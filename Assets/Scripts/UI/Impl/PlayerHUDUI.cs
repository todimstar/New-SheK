using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏中的玩家HUD面板
/// </summary>
public class PlayerHUDUI : BasePanel
{
    [SerializeField] private GameObject menuButton;

    private void Start()
    {
        // 注册菜单按钮点击事件
        menuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClick);
    }
    
    public void OnMenuButtonClick()
    {
        // 菜单按钮点击事件
        Debug.Log("菜单按钮点击事件");
        //打开菜单面板
        //寻找父Canvas
        Transform canvas = transform.parent;
        //寻找菜单面板
        GameObject playStopPanel = canvas.Find("PlayStopPanel").gameObject;

        
        if (playStopPanel != null)
        {
            playStopPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("PlayStopPanel 不在其父Canvas下");
        }
    }
}
