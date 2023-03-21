using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField] private float fallSpeed;
    [SerializeField] private PitchCode _pitchCodeBook;
    [SerializeField] private string m_pitch; // A1, B1, C1, D1, etc.
    [SerializeField] private Color m_pitchColor;
    private float DropCounter;
    private float dropInterval; // how long should the counter achieve to start dropping

    private void OnEnable()
    {
        // populate active enemy in the scene with defined color => not used anymore

        //_pitchCodeBook = FindObjectOfType<PitchCode>();
        //SpriteRenderer rend = this.GetComponent<SpriteRenderer>();
        //if (_pitchCodeBook.CodeBook.ContainsKey(m_pitch))
        //// check if the upcoming pitch matches any dictionary key
        //{
        //    m_pitchColor = _pitchCodeBook.CodeBook[m_pitch];
        //    rend.color = m_pitchColor; // set the enemy renderer to the corresponding color
        //}
        //else
        //{
        //    Debug.LogWarning("Enemy pitch not assigned");
        //    m_pitch = "unknown";
        //    rend.color = new Color32(0, 0, 0, 255); // else set the invader block as black
        //}
        // set it's child position to be Vector3.zero to it's parent
        // use transform.localPosition
        foreach (Transform child in this.transform)
        {
            child.localPosition = Vector3.zero;
        }
        fallSpeed = FindObjectOfType<GameManager>().dropSpeed;
        dropInterval = 1f; // every 1 second, fall
    }
    internal void DestroySelf()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);

    }

    private void Update()
    {
        // every x seconds drop, controlled by fallSpeed
        DropCounter += Time.deltaTime;
        if (DropCounter > dropInterval)
        {
            transform.Translate(fallSpeed * dropInterval * Vector2.down);
            DropCounter = 0;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.CompareTag("Bonus"))
        {
            // if the invader enter the bonus region
            // invoke OSC transmitter message
            FindObjectOfType<GameManager>().SendOutMidi(m_pitch);

        }
        if (other.collider.GetComponent<Bullet>())
        {
            Bullet bulletInCollision = other.gameObject.GetComponent<Bullet>(); 

            Debug.Log("Enemy " + m_pitch + " collided with" + bulletInCollision.PitchCode);

            if (bulletInCollision.PitchCode == m_pitch)
            // if code matches
            {
                DestroySelf();
            }

            // then, if not match, play mismatch animation
            // misMatch();
        }

    }
    private void OnDestroy()
    {
        FindObjectOfType<GameManager>()?.EnemyKilled(this.gameObject);
    }

}
