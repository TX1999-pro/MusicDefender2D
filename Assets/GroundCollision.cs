using UnityEngine;

public class GroundCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            // display end game UI
            FindObjectOfType<GameManager>().EnemyLanded();
        }
    }

}
