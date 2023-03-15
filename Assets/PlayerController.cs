using extOSC;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float targetPosition;
    public float leftBound;
    public float rightBound;
    public string address_1;
    public string address_2;
    public string address_3;
    public bool isMusicDetected;
    public GameManager _gameManager;

    // dictionary seems not working, setting D1 as none
    [SerializeField] private GameObject player;
    [SerializeField] private float playerSpeed;
    [SerializeField] private OSCReceiver receiver;


    [SerializeField] private AudioSource sfx;
    [SerializeField] private Transform speaker;
    [SerializeField] private AudioClip shooting;
    [SerializeField] private float coolDownTime = 0.5f;
    [SerializeField] private Bullet bulletPrefab;
    private float shootTimer;
    [SerializeField] private Color pitchColour = new (245, 40, 145, 255);

    [SerializeField] private Dictionary<string, Color32> pitchCodeBook;


    void Start()
    {

        _gameManager = FindObjectOfType<GameManager>();

        address_1 = "/player/position";
        address_2 = "/player/audio/status"; // true/false
        address_3 = "/player/audio/pitch"; // pitch code name e.g. C3
        // get the receiver
        receiver = GetComponent<OSCReceiver>();
        receiver.Bind(address_1, PositionMessageReceived);
        receiver.Bind(address_2, MusicMessageReceived);
        receiver.Bind(address_3, MusicMessageReceived);
        pitchCodeBook = FindObjectOfType<PitchCode>().CodeBook;


    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPosition(targetPosition);

        // use a timer to control the wait time between shots
        shootTimer += Time.deltaTime;

    }

    private void UpdatePlayerPosition(float targetPosition)
    {
        Vector3 inBoundPosition = new Vector3(targetPosition, 0.76f, 0);

        if (targetPosition < leftBound) // boundary detection
        {
            inBoundPosition.x = leftBound;
        }
        if (targetPosition > rightBound)
        {
            inBoundPosition.x = rightBound;
        }
        transform.position = Vector2.MoveTowards(transform.position, inBoundPosition, playerSpeed * Time.deltaTime);
    }

    #region Fire bullets
    void ShootBullet(String pitch_code,Color m_colour)
    {

        if (shootTimer > coolDownTime)
        {
            shootTimer = 0f;
            // InstantiateBullet()
            GenerateBulletOfColour(pitch_code, m_colour);
            GameManager._instance.PlaySfx(shooting);
        }
    }

    void GenerateBulletOfColour(String pitch_code, Color m_colour)
    {
        Bullet newBullet = Instantiate(bulletPrefab, speaker.position, Quaternion.identity);
        newBullet.PitchCode = pitch_code;
        SpriteRenderer rend = newBullet.GetComponent<SpriteRenderer>();
        rend.color = m_colour;  //  change the color after it is instantiated
    }
    #endregion
    #region OSC message handler
    void PositionMessageReceived(OSCMessage message)
    {
        // check if the received message is of the right
        Debug.Log("message received from " + address_1);
        Debug.Log(message);

        if (message.ToFloat(out var value))
        {
            targetPosition = value;
            //Debug.Log(value);
        } else
        {
            Debug.Log("Message type error. Float required.");
        }
    }

    void MusicMessageReceived(OSCMessage message)
    {
        // control whether the sound is received (as if shoot button was pressed)
        
        if (message.ToInt(out var bo))
        {
            // Toggle shooting on and off
            if (bo == 0) isMusicDetected = false;
            if (bo == 1)
            {
                isMusicDetected = true;
            }
            /////###############################//////
            ShootBullet("NA", new Color32(0,0,0,255));
            /////###############################//////
        } 
        else if (message.ToString(out var pitch))
        {
            isMusicDetected = true;

            if (pitchCodeBook.ContainsKey(pitch))
            // check if the upcoming pitch matches any dictionary key
            {
                Color color = pitchCodeBook[pitch];
                pitchColour = color; // set the pitchColour to the corresponding color
                ShootBullet(pitch, pitchColour);
            }
            else
            {
                Debug.Log(pitch + " is not in the dictionary");
                //    return;
            }

        } 
        else
        {
            Debug.Log("Message Type error. Need integer or string");
        }
    }
    #endregion

    #region player collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            // display end game UI
            _gameManager.PlayerHit();
        }
    }
    #endregion
}
