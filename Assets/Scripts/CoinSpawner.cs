using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform ground;
    public int numberOfCoins = 10;
    public float minYPosition = 1.0f;
    public float maxYPosition = 3.0f;

    private float groundWidth;

    void Start()
    {
        groundWidth = ground.GetComponent<SpriteRenderer>().bounds.size.x;
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            float randomX = Random.Range(-groundWidth / 2, groundWidth / 2);
            float randomY = Random.Range(minYPosition, maxYPosition);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}