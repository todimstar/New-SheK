using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    /* dependencies */
    [SerializeField] private MoveHandler moveHandler;
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