// using UnityEngine;

// /// <summary>
// /// 全局按键监听器，处理ESC键等全局按键
// /// </summary>
// public class KeyboardManager : MonoBehaviour
// {
//     // 单例模式
//     private static KeyboardManager _instance;
//     public static KeyboardManager Instance => _instance;

//     private void Awake()
//     {
//         if (_instance != null && _instance != this)
//         {
//             Destroy(gameObject);
//         }
//         else
//         {
//             _instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//     }

//     void Update()
//     {
//         // 监听ESC键
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             Debug.Log("KeyboardManager: 检测到ESC键按下");
            
//             // 检查PlayStopUI是否存在
//             PlayStopUI playStopUI = FindObjectOfType<PlayStopUI>();
            
//             if (playStopUI != null)
//             {
//                 // 检查当前游戏是否已暂停
//                 if (Time.timeScale < 0.1f) // 近似为0
//                 {
//                     playStopUI.ResumeGame();
//                     Debug.Log("KeyboardManager: 恢复游戏");
//                 }
//                 else
//                 {
//                     playStopUI.PauseGame();
//                     Debug.Log("KeyboardManager: 暂停游戏");
//                 }
//             }
//             else
//             {
//                 Debug.LogWarning("KeyboardManager: 找不到PlayStopUI实例");
//             }
//         }
//     }
// } 