using extOSC;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

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
            ShootBullet();
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


    void ShootBullet()
    {

        if (shootTimer > coolDownTime)
        {
            shootTimer = 0f;

            Instantiate(bulletPrefab, speaker.position, Quaternion.identity);
            GameManager._instance.PlaySfx(shooting);
        }
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
