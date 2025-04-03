目前：
1.SettingPanel的Back那几个没有跟栈的UIManager做好关联，关闭得很乱
解决方案：缕清PanelManager的使用方法

2.PlayStopPanel的时间暂停还没写，尝试跟教程写了全局esc唤出Stop面板，还没实验

3.三个面板打不通了，得好好研究为什么，之前没有UIManager的简单逻辑能跑，但是上了规范就还不适应

-----------------------------
更新(2024-03-24 10:23)：

已解决的问题：

1. PlayStopPanel的Back功能与栈的UIManager关联问题
   - 现象：PlayStopPanel中已正确调用PanelManager.ClosePanel但是无法关闭
   - 发现原因：PlayerHUDUI在调用PlayStopUI时没有使用OpenPanel方法，导致PlayStopPanel面板没有入栈
   - 解决方案：确保所有面板交互都通过PanelManager的OpenPanel和ClosePanel方法
   - 添加了详细的调试日志，方便排查问题

2. PlayStopPanel的时间暂停功能
   - 解决方案：已实现PlayStopUI的PauseGame和ResumeGame方法
   - 使用Time.timeScale = 0f暂停时间，Time.timeScale = 1f恢复时间
   - 按ESC键可以打开/关闭暂停面板

3. TextMeshPro组件获取问题
   - 问题：之前使用GetComponent<Text>()无法获取TextMeshPro组件
   - 解决方案：更新UpdateSoundButtonText方法，同时支持TextMeshPro和传统Text组件
   - 现在可以正确更新音效按钮文本



下一步计划：
1. 测试时间暂停功能
2. 完善所有面板间的交互
3. 考虑添加面板过渡动画效果
4. 优化代码结构，提高可维护性

-------------------
2025.03.25 01:08
问题1：
时停好是好了，但是PlayStopUI的Back按钮要按两下才关闭成功，看日志发现第一次按下比第二次按下多了一段。
发现从PlayerHUDUI里的菜单按钮按下也会触发两次尝试打开面板，两次显示面板，中间夹杂一次隐藏面板，导致这次打开实际TimeScale先被设置成0又1又0，最后才打印输出"游戏已暂停"
关闭事件似乎同理，但是没有被调用两次尝试关闭面板，一次就找到匹配的打印出正在关闭面板，并隐藏，时间设为1，但是从这里到打印出"游戏已恢复"之前又显示了一次面板，导致时间缩放又为0，导致第一次关闭无效。但是再按一次就不会出现'打印出"游戏已恢复"之前又显示了一次面板'的现象。
早上得尝试用rider什么的调试一下看看流程是怎么样的

-01:32
问题2：
测试又发现从StartScene转到GameingScene后，GameingScene运行不正常，注册面板都是空的报错，然后GameingScene的PanelManager好像没正常工作

已解决问题1  
2025.03.25 09:55
原因：在PlayHUDUI中调用了GamePuse，自己又手动OpenUI，所以双重面板且选择了Noraml层级
解决方案：将GamePuse细化为只有时停功能，不负责打开面板，并调整其余调用GamePuse之处并均修改为PopWindows层级  
    
已解决问题2
2025.03.25 10:59
问题2的错误出现在场景切换后，新场景的UI组件(SettingUI/PlayStopUI/PlayerHUDUI)尝试注册到PanelManager时。
原因：在场景切换后，PanelManager仍然保留了对已销毁的StartUI对象的引用。
解决方案
修改 PanelManager.cs 的 RegisterPanel 方法，添加对已销毁对象的检测和清理

------------------
2025.03.25 11:59
解决了esc控制暂停面板不同步问题，完善暂停面板功能
问题：由于没有全局监听器，只有在PlayerHUDUI按下esc才能被监测，出现键按Esc和点按home/back按钮不同步
解决方案：使用单例isGamePause同步PlayStopUI和PlayerHUDUI，在PlayerHUDUI监测到Esc时调用PlayStopUI的函数，增加耦合，修复了时停不同步

------------------
2025.03.25 12:04
目前UI系统基本完成
PlayStop暂停面板开关与时停功能完成
PlayerHUDUI交互功能完成(未暴露接口给剧情系统)
Setting面板留存开关信息功能完成，
StartUI跳转GameingScene功能完成
PanelManager使用订阅-监听模式已实现切换场景时销毁为空的已注册的面板功能，层级栈功能封装完善
StartSettingUI返回功能正常，待添加有效设置功能


总结：各基础UI面板交互完善，实现互通

------------------
2025.04.02 16:15
修改UI系统，简化为单一场景，可能之后的场景切换变为坐标值变化
修改需求：
   开始游戏到游戏中UI收进GamingScene里，     ✓
   暴露任务接口，  √       在HUD加，stop面板内放置存档；本来想分离stop面板为一个单独按钮，使HUD面板在右上角有退出和设置按钮，
                           但是发现那样又要在setting面板开启期间也对isGamePause操作，还是直接本来的就好了
   统一设置页面为一个，   ✓
   做个对话UI，          半√
   存档系统实验(存进度、设置static、角色位置vector3)(主要是跟对话系统融合)
   留存心情值系统通过一个static

修改途中问题：
   1.收为一个场景时，StartUI进入HUD后再通过SettingUI返回StartUI再进入时Time Scale还是保持之前打开StopUI时的0，也许需要每次从StartUI进入时变更Time Scale。
      注：不能直接粗暴的改为每次加载HUD时都将Time为1，因为从HUD可能在其他时候被再次加载，比如打开关闭SettingUi时会因为都是Normal层UI相互调用对方的Show和Hide
   -已成功修复，在StartUI里恢复TimeScale和StopUI的isGamePaused属性
   2.多添加了stop按钮的图标更新，才发现原来没考虑到当stop面板弹出时玩家还可以点击未被遮住的stop按钮，导致无反应甚至多开了很多个stop面板。
   -已修复：现在OnStopBtnClick根据isGamePause执行开关面板修复了这个问题，遗留问题：代码略有冗余，可以再抽象开关stop面板的函数，但是可能导致使用者不清晰和增加函数调用开销
   3.成功对接对话系统，使用ai辅助理解对话系统，成功为对话场景打上整合补丁
   4.成功暴露任务接口