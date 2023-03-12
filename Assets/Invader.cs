using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public PitchCode _pitchCodeBook; // Codebook

    [SerializeField] private string m_pitch; // A1, B1, C1, D1, etc.
    [SerializeField] private Color m_pitchColor;
    //private void OnEnable()
    //{
    //    _pitchCodeBook = FindObjectOfType<PitchCode>();
    //    SpriteRenderer rend = this.GetComponent<SpriteRenderer>();

    //    if (_pitchCodeBook.ColorDictionary.ContainsKey(m_pitch))
    //    // check if the upcoming pitch matches any dictionary key
    //    {
    //        m_pitchColor = _pitchCodeBook.ColorDictionary[m_pitch];
    //        rend.color = m_pitchColor; // set the enemy renderer to the corresponding color
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Enemy pitch not assigned");
    //        m_pitch = "unknown";
    //        rend.color = new Color(0, 0, 0, 255); // else set black
    //    }
    //}
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
