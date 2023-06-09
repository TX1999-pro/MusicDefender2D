using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [System.Serializable]
    public struct BulletPrefab
    {
        public string MIDI_id;
        public GameObject prefab;
    }

    public List<BulletPrefab> bulletPrefabs;

    private Dictionary<string, GameObject> bulletDictionary;
    public GameManager gameManager;
    public PlayerController playerController;

    private void Awake()
    {
        bulletDictionary = new Dictionary<string, GameObject>();

        foreach (var bulletPrefab in bulletPrefabs)
        {
            bulletDictionary.Add(bulletPrefab.MIDI_id, bulletPrefab.prefab);
        }
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public GameObject InstantiateBullet(string bulletID, Vector3 position, Quaternion rotation)
    {
        if (bulletDictionary.ContainsKey(bulletID))
        {
            Debug.Log("Bullet MIDI: " + bulletID);
            GameObject bulletObject = Instantiate(bulletDictionary[bulletID].gameObject, position, rotation);

            // send out position

            gameManager.SendOutBulletPosition(bulletID, playerController.transform.position);
            return bulletObject;
        }
        else
        {
            Debug.LogError("Bullet ID not found: " + bulletID);
            return null;
        }
    }
}