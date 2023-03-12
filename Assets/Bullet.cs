using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;
    public string PitchCode { set; get; }

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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("Bullet collided with " + other.collider.name);

        // Create explosion animation before destroying
        // GameManager.Instance.CreateExplosion(transform.position)

        DestroySelf();

    }

}
