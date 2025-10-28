using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;

    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;
    public int poolSize = 10; // object pool size
    public float spawnDistance = 10f;
    public float minSpawnGap = 40f;
    public float maxSpawnGap = 90f;
    public float recycleXOffset = -10f;

    //[SerializeField]private Transform player;
    public Transform player;
    private List<GameObject> obstaclePool = new List<GameObject>();
    private float lastSpawnX = 0f;

    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)]);
            obj.SetActive(false);
            obj.AddComponent<ObstacleRecycle>();
            obstaclePool.Add(obj);
        }
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (player.position.x + spawnDistance > lastSpawnX)
        {
            SpawnObstacle();
        }
    }

    public void StartSpawning()
    {
        isPlaying = true;
    }

    public void StopSpawning()
    {
        isPlaying = false;
    }

    private void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)];
        GameObject obstacle = GetPooledObject(prefab);

        if (obstacle != null)
        {
            float spawnY = -3f;  // 디노런의 새 역할 을 할 장애물은 y값 설정 필요
            if (obstacle.name.Contains("Obstacle_Imp"))
                spawnY = UnityEngine.Random.Range(-3f, 0f);
            float gap = UnityEngine.Random.Range(minSpawnGap, maxSpawnGap);
            obstacle.transform.position = new Vector3(lastSpawnX + gap, spawnY, 0);
            obstacle.SetActive(true);

            lastSpawnX += gap;
        }
    }

    private GameObject GetPooledObject(GameObject prefab)
    {
        foreach (var obj in obstaclePool)
        {
            if (!obj.activeInHierarchy && obj.name.Contains(prefab.name))
                return obj;
        }

        // 풀 부족 시 새로 생성
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        obstaclePool.Add(newObj);
        return newObj;
    }

    public void RecycleObstacle(GameObject obstacle)
    {
        obstacle.SetActive(false);
    }
}

public class ObstacleRecycle : MonoBehaviour
{
    private void Update()
    {
        if (ObstacleManager.Instance == null)
            return;

        if (transform.position.x < ObstacleManager.Instance.player.position.x + ObstacleManager.Instance.recycleXOffset)
        {
            gameObject.SetActive(false);
        }
    }
}