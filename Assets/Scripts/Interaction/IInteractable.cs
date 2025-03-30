// 接口类，给InteractDetector使用
public interface IInteractable
{
    /// <summary>
    /// 是否可交互（任务锁定等状态）
    /// </summary>
    bool IsInteractable{get;set;} 
    /// <summary>
    /// 主要交互逻辑
    /// </summary>
    /// <param name="interactor">交互者的信息</param>
    void OnInteract(Interactor interactor); 
    /// <summary>
    /// 进入检测范围
    /// </summary>
    void OnDetectionEnter(); 
    /// <summary>
    /// 离开检测范围
    /// </summary>
    void OnDetectionExit(); 
}
