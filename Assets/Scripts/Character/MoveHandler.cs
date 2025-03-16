using UnityEngine;

[System.Serializable]
public class MoveHandler
{
    public CharacterController controller;      // 依赖的组件

    /* 空间移动的处理 */
    public Vector3 velocityXZ = Vector3.zero;
    public Vector3 velocityY = Vector3.zero;

    /* 移动参数 */
    [Header("Properties of the Physics")]
    public float dVelocity = 80f;

    [Header("Properties on the move")]
    public float maxWalk = 10f;
    public float maxRun = 20f;
    public float maxJump = 1.5f;
    // 空间移动
    public void Move()
    {
        Vector3 direction = (
            Input.GetAxis("Horizontal") * controller.transform.right +
            Input.GetAxis("Vertical") * controller.transform.forward
            ).normalized;

        float maxSpeed = Input.GetKey(KeyCode.LeftShift) ? maxRun : maxWalk;

        if (controller.isGrounded)
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
            velocityY + Physics.gravity * Time.deltaTime,
            maxRun * Vector3.down       // 下落速度上限
        );

        controller.Move((velocityXZ + velocityY) * Time.deltaTime);
    }


    private Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 targetDirection, float dVelocity)
    {
        // v + a * t
        return currentVelocity + (targetDirection - currentVelocity).normalized * dVelocity * Time.deltaTime;
    }
}