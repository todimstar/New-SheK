# Unity 3DRPG UI系统

这是一个为3DRPG游戏设计的简单而高效的UI管理系统。该系统采用面板管理的设计模式，提供了灵活的UI界面管理功能。

## 系统架构

本UI系统主要由以下组件构成：

### 1. BasePanel

所有UI面板的基类，提供了基础的面板操作功能：
- `Init()`: 将面板注册到PanelManager
- `Show()`: 显示面板
- `Hide()`: 隐藏面板

### 2. PanelManager

UI面板管理器，负责所有面板的管理，使用单例模式实现：
- 面板注册与存储
- 面板显示与隐藏
- 面板层级管理（Normal层、PopWindow层）
- 使用栈结构管理同层级面板显示顺序

### 3. 具体面板实现

在`Impl`目录下包含了多个具体的面板实现：
- `StartUI`: 游戏开始界面
- `SettingUI`: 游戏设置界面
- `PlayerHUDUI`: 玩家HUD界面
- `PlayStopUI`: 游戏暂停界面

## 使用方法

### 创建新面板

1. 创建一个继承自`BasePanel`的类
2. 在`Awake()`或`Start()`方法中调用`Init()`
3. 根据需要重写`Show()`和`Hide()`方法

```csharp
public class MyNewPanel : BasePanel
{
    private void Awake()
    {
        Init(); // 注册到PanelManager
    }
    
    // 可以重写Show和Hide方法以添加自定义行为
    public override void Show()
    {
        base.Show();
        // 自定义显示逻辑
    }
}
```

### 显示和隐藏面板

```csharp
// 显示面板
PanelManager.Instance.OpenPanel(typeof(MyNewPanel));

// 显示弹窗层面板
PanelManager.Instance.OpenPanel(typeof(MyNewPanel), PanelManager.PanelLayer.PopWindow);

// 隐藏面板
PanelManager.Instance.ClosePanel(typeof(MyNewPanel));

// 隐藏所有面板
PanelManager.Instance.CloseAllPanels();
```

### 在Unity编辑器中设置

1. 创建一个空物体，添加PanelManager组件
2. 为每个UI面板创建预制体并添加对应的面板脚本
3. 确保在场景加载时PanelManager优先初始化

## 系统特点

- **层级管理**：支持Normal和PopWindow两个层级，方便管理不同优先级的UI
- **栈结构**：采用栈结构管理同层级UI，便于实现UI导航和返回功能
- **松耦合**：面板之间解耦，每个面板只需关注自身逻辑
- **易扩展**：可以方便地添加新的面板和UI功能

## 已知问题

- SettingPanel的Back功能与面板管理器的栈结构存在兼容问题
- PlayStopPanel的暂停功能尚未完成
- 部分面板之间的连接存在问题

## 下一步计划

- 完善PanelManager的RegisterPanel方法
- 实现PlayStopPanel的暂停功能
- 优化面板之间的切换逻辑
- 添加面板动画过渡效果

