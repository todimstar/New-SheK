using System.Collections;
using UnityEngine;

// 交互检测，通过协程检测，检测到可交互物体时，激活可交互物体的OnDetectionEnter()方法
public class InteractionDetector : MonoBehaviour
{
    [Header("Detector Setting")]
    /// <summary>
    /// 检测频率,每次检测的间隔时间
    /// </summary>
    [SerializeField] private float detectFrequency = 0.2f;
    /// <summary>
    /// 检测范围
    /// </summary>
    [SerializeField] private float detectRange = 3f;     
    /// <summary>
    /// 可交互物体的层
    /// </summary>       
    [SerializeField] private LayerMask interactableLayer;

    /// <summary>
    /// 当前检测到的目标
    /// </summary>
    private IInteractable currentTarget;

    /** 检测的相关设置 **/
    
    /// <summary>
    /// 检测源
    /// </summary>
    public Transform detectOrigin;
    private Ray ray;
    private void Awake()
    {
        if(detectOrigin == null){
            detectOrigin = Camera.main.transform;
        }
    }
    private void Start() {
        // 开始协程检测
        StartCoroutine(DetectionRoutine());
    }

    /// <summary>
    /// 刷新射线
    /// </summary>
    private void SetRay(){
        ray.origin = detectOrigin.position;
        ray.direction = detectOrigin.forward;
    }

    /// <summary>
    /// 更新范围内的可交互对象，通过协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator DetectionRoutine() {
        while(true) {
            SetRay();
            Debug.Log("Detecting");
            if(Physics.Raycast(ray, out RaycastHit hit, detectRange, interactableLayer)) {

                IInteractable interactable = hit.collider.GetComponent<IInteractable>();    // 检测物体需要要实现IInteractable接口的脚步
                
                if(interactable != null && interactable.IsInteractable) {
                    // 切换检测到的物体
                    if(currentTarget != interactable) {
                        currentTarget?.OnDetectionExit(); // 前一个目标退出
                        currentTarget = interactable;
                        currentTarget.OnDetectionEnter();
                    }
                }
            } else {
                // 范围内没有物体，当前存储的可交互物体退出
                currentTarget?.OnDetectionExit();
                currentTarget = null;
            }
            yield return new WaitForSeconds(detectFrequency); // 性能优化，每 detectFrequency 秒检测一次
        }
    }

    public IInteractable GetCurrentInteractable() => currentTarget;
}