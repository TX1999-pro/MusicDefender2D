using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public Transform playerTransform;
    public GameManager _gameManager;
    public GameObject coinPrefab;
    public Transform ground;
    public int numberOfCoins = 10;
    public float minYPosition = 1.0f;
    public float maxYPosition = 3.0f;

    public float minDistanceFromPlayer = 2.0f;
    public float minSpawnInterval = 5.0f;
    public float maxSpawnInterval = 10.0f;


    private float groundWidth;
    private List<GameObject> spawnedCoins;

    private void OnEnable()
    {
        GameManager.OnGameEnd += DestroyAllCoins;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnd -= DestroyAllCoins;
    }
    private void Start()
    {
        groundWidth = ground.GetComponent<SpriteRenderer>().bounds.size.x;
        playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        spawnedCoins = new List<GameObject>();

        StartCoroutine(SpawnCoins());

    }

    private IEnumerator SpawnCoins()
    {
        while (!GameManager._instance.isGameOver)
        {
            float randomX;
            float playerX = playerTransform.position.x;
            do
            {
                randomX = Random.Range(-groundWidth / 2, groundWidth / 2);
            } while (Mathf.Abs(randomX - playerX) < minDistanceFromPlayer);

            float randomY = Random.Range(minYPosition, maxYPosition);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

            GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            spawnedCoins.Add(newCoin);

            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    void DestroyAllCoins()
    {
        foreach (GameObject coin in spawnedCoins)
        {
            Destroy(coin);
        }
        spawnedCoins.Clear();
    }

    //void SpawnCoins()
    //{
    //    for (int i = 0; i < numberOfCoins; i++)
    //    {
    //        float randomX = Random.Range(-groundWidth / 2, groundWidth / 2);
    //        float randomY = Random.Range(minYPosition, maxYPosition);
    //        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

    //        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    //    }
    //}
}