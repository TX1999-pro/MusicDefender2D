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
    public BulletManager _bulletManager;

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

    private RaycastSelectButton buttonSelector;

    void Start()
    {

        _gameManager = FindObjectOfType<GameManager>();
        buttonSelector = FindObjectOfType<RaycastSelectButton>();

        address_1 = "/player/position";
        address_2 = "/player/audio/instruction"; // true/false
        address_3 = "/player/audio/pitch"; // pitch code name e.g. C3
        // get the receiver
        receiver = GetComponent<OSCReceiver>();
        receiver.Bind(address_1, PositionMessageReceived);
        receiver.Bind(address_2, InstructionMessageReceived);
        receiver.Bind(address_3, MusicMessageReceived);
        pitchCodeBook = FindObjectOfType<PitchCode>().CodeBook;


    }

    void Update()
    {
        UpdatePlayerPosition(targetPosition);

        // use a timer to control the wait time between shots
        shootTimer += Time.deltaTime;

    }

    #region player control
    private void UpdatePlayerPosition(float targetPosition)
    {
        Vector3 inBoundPosition = new Vector3(targetPosition, 0, 0);

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

    public void SwitchAppearanceByName(string currentChildName, string newChildName)
    {
        Transform currentChild = transform.Find(currentChildName);
        Transform newChild = transform.Find(newChildName);

        if (currentChild == null || newChild == null)
        {
            // Check if the given name is valid
            Debug.LogError("Child with the given name not found.");
            return;
        }

        currentChild.gameObject.SetActive(false);
        newChild.gameObject.SetActive(true);
    }

    public void SwitchAppearanceByIndex(int currentChildIndex, int newChildIndex)
    {
        // Check if the given indexes are valid
        if (currentChildIndex >= transform.childCount || newChildIndex >= transform.childCount)
        {
            Debug.LogError("Invalid child index provided.");
            return;
        }

        // Set the current child inactive
        transform.GetChild(currentChildIndex).gameObject.SetActive(false);

        // Set the new child active
        transform.GetChild(newChildIndex).gameObject.SetActive(true);
    }

    #endregion

    #region Fire bullets
    void ShootBullet(String pitch_code,Color m_colour)
    {

        if (shootTimer > coolDownTime)
        {
            shootTimer = 0f;
            InstantiateBullet(pitch_code);
            //GenerateBulletOfColour(pitch_code, m_colour);
        }
    }

    void InstantiateBullet(String pitch_code)
    {
        Vector3 spawnPosition = speaker.position;
        Quaternion spawnRotation = Quaternion.identity;

        GameObject newBullet = _bulletManager.InstantiateBullet(pitch_code, spawnPosition, spawnRotation);
        //if (newBullet != null) // play the sound effect if there is a valid bullet returned
        //{
        //    GameManager._instance.PlaySfx(shooting);
        //}

    }
    void GenerateBulletOfColour(String pitch_code, Color m_colour)
    // obsolete code
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
        //Debug.Log("message received from " + address_1);
        //Debug.Log(message);

        if (message.ToFloat(out var value))
        {
            targetPosition = value;
            //Debug.Log(value);
        } else
        {
            Debug.Log("Message type error. Float required.");
        }
    }

    void InstructionMessageReceived(OSCMessage message)
    {
        // check if the received message is of the right

        if (message.ToString(out var instruction))
        {
            if (instruction == "click" )
            {
                // invoke the button.onClick of the select button
                TriggerSelectedButton();
            }
            if (instruction == "transform")
            {
                // update the player sprite,transform to lizard
                SwitchAppearanceByIndex(1, 2);

            }
            if (instruction == "back")
            {
                // update the player sprite, transform to human
                SwitchAppearanceByIndex(2, 1);
            }

        }
        else
        {
            Debug.Log("Message type error. String required.");
        }
    }

    void MusicMessageReceived(OSCMessage message)
    {
        // control whether the sound is received (as if shoot button was pressed), used for debugging
        
        //if (message.ToInt(out var bo))
        //{
        //    // Toggle shooting on and off
        //    if (bo == 0) isMusicDetected = false;
        //    if (bo == 1)
        //    {
        //        isMusicDetected = true;
        //    }
        //    /////###############################//////
        //    ShootBullet("NA", new Color32(0,0,0,255));
        //    /////###############################//////
        //} 
        if (message.ToString(out var pitch))
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
        if (other.collider.CompareTag("Coin"))
        {
            // Do something when the player collides with the coin, e.g., add points
            Debug.Log("hit coin!");
            ScoreSystem.instance.AddScore(10);
            Destroy(other.gameObject);
        }
    }

    #endregion

    #region instruction handler
    private void TriggerSelectedButton()
    {
        // Ensure the selected button is not null
        if (buttonSelector.currentSelectedButton != null)
        {
            buttonSelector.currentSelectedButton.onClick.Invoke();
        }
    }
    #endregion
}
