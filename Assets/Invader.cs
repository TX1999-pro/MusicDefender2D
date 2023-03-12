using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{

    internal void DestroySelf()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.GetComponent<Bullet>())
        // if not hit by bullets, do nothing
        {
            return;
        }
        else
        {
            Debug.Log("Enemy collided with " + other.collider.name);
            DestroySelf();
        }

    }



}
