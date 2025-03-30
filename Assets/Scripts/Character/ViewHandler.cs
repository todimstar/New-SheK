using UnityEngine;

[System.Serializable]
public class ViewHandler
{
    /// <summary>依赖的摄像机</summary>
    public Transform camera;
    /// <summary>当前的仰角</summary>
    private float currentRotationX = 0f;
    [Header("Properties on the look")]
    /// <summary>鼠标灵敏度</summary>
    public float dRotation = 100f;
    /// <summary>最大仰角</summary>
    [Range(0,90)]public float upperAngle = 45f;
    /// <summary>最大俯视角</summary>
    [Range(0,90)]public float downAngle = 45f;
    
    /// <summary>视角控制</summary>
    public void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        // 旋转自己
        camera.parent.transform.Rotate(Vector3.up, mouseX * dRotation * Time.deltaTime);

        currentRotationX = Mathf.Clamp(
            currentRotationX - mouseY * dRotation * Time.deltaTime,
            -upperAngle,
            downAngle
        );
        // 上下旋转摄像机
        camera.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
    }
}