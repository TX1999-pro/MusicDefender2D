using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{


    [SerializeField] private AudioSource sfx;
    [SerializeField] private Transform speaker;
    [SerializeField] private AudioClip shooting;
    [SerializeField] private float coolDownTime = 0.5f;
    [SerializeField] private Bullet bulletPrefab;
    private float shootTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > coolDownTime && Input.GetKey(KeyCode.Space))
        {
            shootTimer = 0f;

            Instantiate(bulletPrefab, speaker.position, Quaternion.identity);
            GameManager._instance.PlaySfx(shooting);
        }

    }
}
