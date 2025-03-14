public interface IInteractable
{
    bool IsInteractable(); // 是否可交互（任务锁定等状态）
    void OnInteract(Interactor interactor); // 主要交互逻辑
    void OnDetectionEnter(); // 进入检测范围
    void OnDetectionExit(); // 离开检测范围
}
