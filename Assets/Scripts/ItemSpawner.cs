using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject honeyPrefab;
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject moneyPrefab;

    [Header("Honey Spawn")]
    [SerializeField] private float honeySpawnInterval = 15f;
    [SerializeField] private float honeySpawnRandomRange = 5f;

    [Header("Flower Spawn")]
    [SerializeField] private float flowerSpawnInterval = 20f;
    [SerializeField] private float flowerSpawnRandomRange = 8f;
    [SerializeField] private float flowerLifetime = 15f;
    
    [Header("Money Spawn")]
    [SerializeField] private float moneySpawnInterval = 15f;
    [SerializeField] private float moneySpawnRandomRange = 5f;

    [Header("Spawn Area")]
    [SerializeField] private float spawnMargin = 0.5f;

    private void Start()
    {
        StartCoroutine(SpawnHoneyRoutine());
        StartCoroutine(SpawnFlowerRoutine());
        StartCoroutine(SpawnMoneyRoutine());
    }

    private IEnumerator SpawnHoneyRoutine()
    {
        while (!GameFlow.I || !GameFlow.I.IsRunning) yield return null;

        while (!GameFlow.I.IsGameOver)
        {
            float waitTime = Mathf.Max(0.05f, honeySpawnInterval + Random.Range(-honeySpawnRandomRange, honeySpawnRandomRange));
            yield return new WaitForSeconds(waitTime);

            if (!GameFlow.I.IsGameOver && honeyPrefab != null)
            {
                Vector2 spawnPos = GetRandomPositionInScreen();
                Instantiate(honeyPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
    
    private IEnumerator SpawnMoneyRoutine()
    {
        while (!GameFlow.I || !GameFlow.I.IsRunning) yield return null;

        while (!GameFlow.I.IsGameOver)
        {
            float waitTime = Mathf.Max(0.05f, moneySpawnInterval + Random.Range(-moneySpawnRandomRange, moneySpawnRandomRange));
            yield return new WaitForSeconds(waitTime);

            if (!GameFlow.I.IsGameOver && moneyPrefab != null)
            {
                Vector2 spawnPos = GetRandomPositionInScreen();
                Instantiate(moneyPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    private IEnumerator SpawnFlowerRoutine()
    {
        while (!GameFlow.I || !GameFlow.I.IsRunning) yield return null;

        while (!GameFlow.I.IsGameOver)
        {
            float waitTime = Mathf.Max(0.05f, flowerSpawnInterval + Random.Range(-flowerSpawnRandomRange, flowerSpawnRandomRange));
            yield return new WaitForSeconds(waitTime);

            if (!GameFlow.I.IsGameOver && flowerPrefab != null)
            {
                Vector2 spawnPos = GetRandomPositionInScreen();
                GameObject flower = Instantiate(flowerPrefab, spawnPos, Quaternion.identity);
                Destroy(flower, flowerLifetime);
            }
        }
    }

    private Vector2 GetRandomPositionInScreen()
    {
        if (CameraBounds2D.I == null) return Vector2.zero;

        Rect rect = CameraBounds2D.I.GetWorldRect(0f);
        float x = Random.Range(rect.xMin + spawnMargin, rect.xMax - spawnMargin);
        float y = Random.Range(rect.yMin + spawnMargin, rect.yMax - spawnMargin);
        return new Vector2(x, y);
    }
}
