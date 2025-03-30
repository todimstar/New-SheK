using UnityEngine;

[System.Serializable]
public class MoveHandler
{
    public CharacterController controller;      // 依赖的组件

    /* 空间移动的处理 */
    public Vector3 velocityXZ = Vector3.zero;   // 水平速度
    public Vector3 velocityY = Vector3.zero;    // 垂直速度

    /* 移动参数 */
    [Header("Properties of the Physics")]
    public float dVelocity = 80f;               // 加速度

    [Header("Properties on the move")]
    public float maxWalk = 10f;                 // 最大走路速度
    public float maxRun = 20f;                  // 最大跑速
    public float maxJump = 1.5f;                // 最大跳跃高度 
    /// <summary>空间移动</summary>
    public void Move()
    {
        // 获得标准化的方向
        Vector3 direction = (
            Input.GetAxis("Horizontal") * controller.transform.right +
            Input.GetAxis("Vertical") * controller.transform.forward
            ).normalized;

        // 可能的最大速度
        float maxSpeed = Input.GetKey(KeyCode.LeftShift) ? maxRun : maxWalk;

        if (controller.isGrounded)  // 如果在地面上
        {
            velocityY.y = Input.GetAxis("Jump") != 0 ?
                Mathf.Sqrt(2 * -maxJump * Physics.gravity.y) :  // sqrt 2 * g * h
                -0.05f;     // 在地面时，给下压力
        }
        /* 水平移动处理 */

        velocityXZ = CalculateVelocity(velocityXZ, direction * maxSpeed, dVelocity);    // 最大速度

        /* 竖直移动处理 */
        // 跳跃
        velocityY = Vector3.Max(
            velocityY + Physics.gravity * Time.deltaTime,   // v + g t
            maxRun * Vector3.down       // 下落速度上限
        );

        controller.Move((velocityXZ + velocityY) * Time.deltaTime);
    }

    /// <summary>计算速度</summary>
    /// <param name="currentVelocity">当前速度</param>
    /// <param name="targetDirection">目标速度</param>
    /// <param name="dVelocity">加速度</param>
    /// <returns></returns>
    private Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 targetDirection, float dVelocity)
    {
        // v + a * t
        return currentVelocity + (targetDirection - currentVelocity).normalized * dVelocity * Time.deltaTime;
    }
}