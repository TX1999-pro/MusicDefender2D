using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField] private string m_pitch;
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
            Bullet bulletInCollision = other.gameObject.GetComponent<Bullet>(); 
            Debug.Log("Enemy collided with " + bulletInCollision.PitchCode);

            if (bulletInCollision.PitchCode == m_pitch)
            // if code matches
            {
                DestroySelf();
            }

            // play mismatch animation
            // misMatch();
        }

    }



}
