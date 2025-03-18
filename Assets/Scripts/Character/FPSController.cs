using UnityEngine;


/// <summary>
/// 第一人称控制器
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    /* 依赖组件 */

    /// <summary>
    /// 移动控制器
    /// </summary>
    [SerializeField] private MoveHandler moveHandler;
    /// <summary>
    /// 视角控制器
    /// </summary>
    [SerializeField] private ViewHandler viewHandler;

    private void Awake()
    {
        moveHandler.controller = GetComponent<CharacterController>();

        viewHandler.camera = GameObject.Find("camera").transform;
    }
    private void Update()
    {
        viewHandler.Look();     // 视角
        moveHandler.Move();     // 移动
    }

}