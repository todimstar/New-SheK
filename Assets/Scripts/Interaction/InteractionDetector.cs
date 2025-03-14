using System.Collections;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [Header("Detector Setting")]
    [SerializeField] private float detectFrequency = 0.2f;      // 检测频率,每次检测的间隔时间
    [SerializeField] private float detectRange = 3f;            // 检测范围
    [SerializeField] private LayerMask interactableLayer;       // 可交互物体的层

    private IInteractable currentTarget;

    /** 检测的相关设置 **/
    public Transform detectOrigin;
    private Ray ray;
    private void Awake()
    {
        if(detectOrigin == null){
            detectOrigin = Camera.main.transform;
        }
    }
    
    private void Start() {
        StartCoroutine(DetectionRoutine());
    }

    private void SetRay(){
        ray.origin = detectOrigin.position;
        ray.direction = detectOrigin.forward;
    }

    private IEnumerator DetectionRoutine() {
        while(true) {
            SetRay();

            if(Physics.Raycast(ray, out RaycastHit hit, detectRange, interactableLayer)) {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if(interactable != null && interactable.IsInteractable()) {
                    if(currentTarget != interactable) {
                        currentTarget?.OnDetectionExit(); // 前一个目标退出
                        currentTarget = interactable;
                        currentTarget.OnDetectionEnter();
                    }
                }
            } else {
                currentTarget?.OnDetectionExit();
                currentTarget = null;
            }
            yield return new WaitForSeconds(detectFrequency); // 性能优化
        }
    }

    public IInteractable GetCurrentInteractable() => currentTarget;
}