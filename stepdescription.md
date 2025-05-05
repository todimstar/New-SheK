# Unity 3DRPG UI系统使用指南

本文档将详细说明如何在Unity中设置和使用我们的UI系统。即使您是第一次接触Unity开发，按照以下步骤操作，也能轻松上手。

## 第一部分：基础设置

### 1. 创建PanelManager

1. 在Unity的Hierarchy面板中，右键单击 -> 选择 "Create Empty"，创建一个空物体
2. 将该空物体命名为 "PanelManager"
3. 在Inspector面板中，点击 "Add Component" -> 搜索并添加 "PanelManager" 脚本
4. 确保勾选 "Don't Destroy On Load" 选项（如果脚本中没有自动处理）

> **重要提示**：PanelManager必须在场景加载时首先初始化，所以请确保在Script Execution Order设置中，PanelManager的执行顺序优先于其他脚本。

### 2. 创建Canvas和UI面板

1. 在Hierarchy面板中，右键单击 -> 选择 "UI" -> "Canvas"，创建一个Canvas
2. 确保Canvas设置为"Screen Space - Overlay"模式（在Canvas组件中）
3. 在Canvas下创建一个空物体，命名为"Panels"，用于组织所有UI面板

## 第二部分：创建UI面板预制体

下面以创建StartUI面板为例，其他面板也遵循类似步骤：

### 1. 创建StartUI面板

1. 在Panels下，右键单击 -> 选择 "UI" -> "Panel"，创建一个Panel
2. 将Panel命名为 "StartUI"
3. 在Inspector面板中，点击 "Add Component" -> 搜索并添加 "StartUI" 脚本
4. 设计您的UI界面，添加按钮、文本等元素

### 2. 连接UI元素与脚本事件

1. 选择StartUI中的"开始游戏"按钮
2. 在Inspector面板的Button组件中，找到 "On Click()" 部分
3. 点击 "+" 按钮添加一个新的事件
4. 将StartUI面板拖拽到Object字段
5. 在函数下拉菜单中，选择 "StartUI" -> "StartBtn_Event_Open"
6. 重复上述步骤，为其他按钮连接相应事件

### 3. 将面板设置为预制体（可选但推荐）

1. 设计好UI面板后，将整个面板从Hierarchy拖拽到Project窗口的Prefabs文件夹中
2. 这样您就创建了一个可重用的UI面板预制体

## 第三部分：UI面板之间的联动

### 1. 正确初始化面板

在每个面板的脚本中，确保在Awake()或Start()方法中调用Init()方法：

```csharp
private void Awake()
{
    Init(); // 注册到PanelManager
    // 其他初始化代码
}
```

### 2. 使用PanelManager控制面板显示

在需要显示其他面板的地方（如按钮点击事件），使用PanelManager.OpenPanel方法：

```csharp
public void SettingtBtn_Event_Open()
{
    // 显示设置面板
    PanelManager.Instance.OpenPanel(typeof(SettingUI));
}
```

### 3. 处理面板返回逻辑

在面板的"返回"按钮事件中，使用ClosePanel方法关闭当前面板：

```csharp
public void BackBtn_Event_Close()
{
    // 关闭当前面板，返回上一个面板
    PanelManager.Instance.ClosePanel(typeof(SettingUI));
}
```

## 第四部分：修复当前存在的问题

### 1. 修复RegisterPanel方法

当前PanelManager.cs中的RegisterPanel方法存在问题，应该修改为：

```csharp
public void RegisterPanel(BasePanel panel)
{
    if (panelDict.ContainsKey(panel.GetType()))
    {
        return;
    }
    panelDict.Add(panel.GetType(), panel);
}
```

### 2. 确保UITable正确初始化

在PanelManager的Awake方法中，应当初始化所有层级的栈：

```csharp
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
        
        // 初始化所有层级的栈
        foreach (PanelLayer layer in Enum.GetValues(typeof(PanelLayer)))
        {
            UITable[layer] = new Stack<BasePanel>();
        }
    }
}
```

### 3. 修复SettingUI的Back功能

确保SettingUI的Back按钮事件正确调用ClosePanel方法：

```csharp
public void BackBtn_Click()
{
    // 正确的关闭当前面板方式
    PanelManager.Instance.ClosePanel(typeof(SettingUI));
}
```

## 第五部分：使用技巧与最佳实践

### 1. 层级管理

- 使用Normal层级显示常规UI面板（如主界面、设置界面等）
- 使用PopWindow层级显示弹窗（如确认框、提示框等）

```csharp
// 显示弹窗
PanelManager.Instance.OpenPanel(typeof(ConfirmDialog), PanelManager.PanelLayer.PopWindow);
```

### 2. 场景切换处理

当切换场景时，确保UI状态正确：

1. 在场景切换前，隐藏所有UI面板
2. 在新场景加载后，根据需要显示相应的UI面板

### 3. 调试技巧

- 使用Debug.Log记录面板显示和隐藏的过程
- 在Unity编辑器中查看PanelManager的面板字典和栈的状态

## 注意事项

1. 面板显示顺序由栈结构决定，后显示的面板会覆盖先显示的面板
2. 同一层级内，关闭当前面板会自动显示栈中的上一个面板
3. 确保每个面板都正确调用了Init()方法，否则将无法被PanelManager管理

## 下一步学习

1. 学习如何添加面板过渡动画
2. 探索如何与其他系统（如游戏状态系统）集成
3. 研究如何优化UI性能 