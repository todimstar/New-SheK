using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSettingUI : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
        Hide();
    }
    public void Click_BackBtn()
    {
        // 返回按钮点击事件
        Debug.Log("返回按钮点击事件");
        PanelManager.Instance.ClosePanel(typeof(StartSettingUI));
    }

}
