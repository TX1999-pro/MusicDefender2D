using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("hit coin!");
            ScoreSystem.instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}