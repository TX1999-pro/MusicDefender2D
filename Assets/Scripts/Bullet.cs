using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public string PitchCode;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 4f;


    internal void DestroySelf()
    {

        gameObject.SetActive(false);
        Destroy(gameObject);

    }

    private void Awake()
    {
        Invoke(nameof(DestroySelf), lifeTime);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);

        if (transform.position.y >= 10f)
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("Bullet collided with " + other.collider.name);


        if (other.collider.CompareTag("Enemy"))
        {
            // Create explosion animation before destroying
            // GameManager.Instance.CreateExplosion(transform.position)
            DestroySelf();
        }

        if (other.collider.CompareTag("Bounds"))
        {
            this.GetComponent<BoxCollider2D>().enabled = false; // disable collider so that it won't cause collision with hidden enemies
        }
    }

}
