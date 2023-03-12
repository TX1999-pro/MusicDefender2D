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

    void Start()
    {
        address_1 = "/player/position";
        address_2 = "/player/audio/status"; // true/false
        address_3 = "/player/audio/code"; // code name e.g. C3
        // get the receiver
        receiver = GetComponent<OSCReceiver>();
        receiver.Bind(address_1, PositionMessageReceived);
        receiver.Bind(address_2, MusicMessageReceived);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPosition(targetPosition);

        // use a timer to control the wait time between shots
        shootTimer += Time.deltaTime;         
        if (isMusicDetected)
        {
            ShootBullet(pitchColour); // ShootBulletOfColour(Color pitchColour);
        }



    }

    private void UpdatePlayerPosition(float targetPosition)
    {
        Vector3 inBoundPosition = new Vector3(targetPosition, 0, 0);
        if (targetPosition < leftBound)
        {
            inBoundPosition.x = leftBound;
        }
        if (targetPosition > rightBound)
        {
            inBoundPosition.x = rightBound;
        }
        transform.position = Vector2.MoveTowards(transform.position, inBoundPosition, playerSpeed * Time.deltaTime);
    }


    void ShootBullet(Color m_colour)
    {

        if (shootTimer > coolDownTime)
        {
            shootTimer = 0f;
            // InstantiateBullet()
            GenerateBulletOfColour(m_colour);
            GameManager._instance.PlaySfx(shooting);
        }
    }

    void GenerateBulletOfColour(Color m_colour)
    {
        Bullet newBullet = Instantiate(bulletPrefab, speaker.position, Quaternion.identity);
        SpriteRenderer rend = newBullet.GetComponent<SpriteRenderer>();
        rend.color = m_colour;  //  change the color after it is instantiated
    }

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
            //Debug.Log(bo);
            if (bo == 0) isMusicDetected = false;
            if (bo == 1)
            {
                isMusicDetected = true;
            }
        }
        else
        {
            Debug.Log("Message type error. Integer required");
        }
    }

}
