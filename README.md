# RPG游戏UI系统

本项目是一个RPG游戏的UI系统，用于显示游戏中各种界面元素，包括游戏内UI、菜单UI、对话UI等。

## 系统架构

UI系统采用模块化设计，主要包含以下组件：

1. **UIManager**: 全局UI管理器，负责UI面板的显示、隐藏和切换
2. **UIPanel**: 所有UI面板的基类，提供通用功能
3. **具体UI面板**:
   - GameplayUI: 游戏主界面，显示角色状态、心情条等
   - MainMenuUI: 游戏主菜单
   - SettingsUI: 设置界面
   - DialogueUI: 对话界面
   - QuestUI: 任务界面
   - InventoryUI: 物品栏界面

## 功能说明

### 游戏主界面 (GameplayUI)
- 心情条: 使用Microlight的MicroBar资源实现
- 状态显示: 显示角色的生命值、能量等

- 任务提示: 显示当前任务简要信息

### 主菜单 (MainMenuUI)
- 开始游戏
- 设置
- 退出游戏

### 设置界面 (SettingsUI)
- 音量控制
- 画面设置
- 按键设置


### 任务界面 (QuestUI)
- 当前任务列表
- 任务详情


## 使用方法

1. 所有UI面板均由UIManager统一管理
2. 通过UIManager.Show<T>()方法显示指定面板
3. 通过UIManager.Hide<T>()方法隐藏指定面板
4. 通过UIManager.Toggle<T>()方法切换指定面板的显示状态

## 技术细节

- 使用单例模式实现UIManager
- 使用工厂模式创建UI面板
- 使用观察者模式响应游戏事件
- 使用状态模式管理UI状态

