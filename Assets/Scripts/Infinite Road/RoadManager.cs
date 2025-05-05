using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject roadPrefab; // 道路预制体
    public int roadCount = 3; // 保持的道路段数量
    public float roadLength = 100f; // 单段路径长度
    public float moveSpeed = 10f; // 场景移动速度
    
    private GameObject[] roads;
    private int currentRoadIndex = 0;

    void Start()
    {
        roads = new GameObject[roadCount];
        // 初始化道路段
        for (int i = 0; i < roadCount; i++)
        {
            roads[i] = Instantiate(roadPrefab, 
                new Vector3(0, 0, i * roadLength), 
                Quaternion.identity);
        }
    }

    void Update()
    {
        // 移动所有道路段
        for (int i = 0; i < roadCount; i++)
        {
            roads[i].transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        
        // 检查是否有道路段需要移动到前方
        // 找到Z坐标最小的道路段（最后面的道路）
        int minZIndex = 0;
        float minZ = roads[0].transform.position.z;
        for (int i = 1; i < roadCount; i++)
        {
            if (roads[i].transform.position.z < minZ)
            {
                minZ = roads[i].transform.position.z;
                minZIndex = i;
            }
        }
        
        // 如果最后面的道路移出了视野，将其移到最前面
        if (minZ < -roadLength)
        {
            // 找到当前Z坐标最大的道路段（最前面的道路）
            int maxZIndex = 0;
            float maxZ = roads[0].transform.position.z;
            for (int i = 1; i < roadCount; i++)
            {
                if (roads[i].transform.position.z > maxZ)
                {
                    maxZ = roads[i].transform.position.z;
                    maxZIndex = i;
                }
            }
            
            // 将后面的道路移到最前面
            roads[minZIndex].transform.position = new Vector3(
                roads[minZIndex].transform.position.x,
                roads[minZIndex].transform.position.y,
                maxZ + roadLength
            );
        }
    }
}