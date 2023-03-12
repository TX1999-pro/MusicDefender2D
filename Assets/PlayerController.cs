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
    public string address;

    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private OSCReceiver receiver;

    
    void Start()
    {
        address = "/player/position";
        // get the receiver
        receiver = GetComponent<OSCReceiver>();
        receiver.Bind(address, MessageReceived);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPosition(targetPosition);
        
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
        transform.position = Vector2.MoveTowards(transform.position, inBoundPosition, speed * Time.deltaTime);
    }

    void MessageReceived(OSCMessage message)
    {
        // check if the received message is of the right
        Debug.Log("message received from " + address);
        Debug.Log(message);

        if (message.ToFloat(out var value))
        {
            targetPosition = value;
            Debug.Log(value);
        } else
        {
            Debug.Log("Message type error. Please make sure a float is sent.");
        }
    }

}
