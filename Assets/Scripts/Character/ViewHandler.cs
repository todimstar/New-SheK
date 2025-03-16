using UnityEngine;

[System.Serializable]
public class ViewHandler
{
    public Transform camera;     // 依赖的摄像机
    private float currentRotationX = 0f; // 当前的仰角
    [Header("Properties on the look")]
    public float dRotation = 100f;     // 旋转速度
    [Range(0,90)]public float upperAngle = 45f;     // 最大仰角
    [Range(0,90)]public float downAngle = 45f;      // 最大俯视角
    
    // 视角旋转
    public void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        camera.parent.transform.Rotate(Vector3.up, mouseX * dRotation * Time.deltaTime);

        currentRotationX = Mathf.Clamp(
            currentRotationX - mouseY * dRotation * Time.deltaTime,
            -upperAngle,
            downAngle
        );

        camera.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
    }
}