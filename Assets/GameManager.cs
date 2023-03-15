using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class GameManager : MonoBehaviour
{
    internal static GameManager _instance;
    internal void PlaySfx(AudioClip clip) => sfx.PlayOneShot(clip);

    [SerializeField] private AudioSource sfx;
    
    private bool gameHasEnded = false;
    public PlayerController playerController;
    public ScoreSystem m_ScoreSystem;

    //UI
    public GameObject gameEndWhenHit;
    public GameObject gameEndWhenLand;
    public GameObject levelCompletedUI;
    
    public TextMeshProUGUI invaderCountDisplay;
    public TextMeshProUGUI ScoreDisplay;
    private int invaderLeft;
    
    [SerializeField]private List<GameObject> enemies;
       
    public delegate void AllEnemiesKilled(); // template
    public static event AllEnemiesKilled OnAllEnemiesKilled; // instance
    [SerializeField] private float destroyEnemybelowHeight = 9.4f;
    //public GameObject[] enemyPrefabs;


    private void OnEnable()
    {
        OnAllEnemiesKilled += LevelComplete;
    }

    private void OnDisable()
    {
        OnAllEnemiesKilled -= LevelComplete;
    }

    private void Start()
    {
        #region Singleton
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        #endregion
        gameEndWhenHit.SetActive(false);
        gameEndWhenLand.SetActive(false);
        levelCompletedUI.SetActive(false);

        // count the number of enemy in the scene
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        invaderLeft = enemies.Count;
        UpdateInvaderCounter();
        Debug.Log("Total No. of Enemy Left: " + invaderLeft);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Hey cheating!");
                // D and A are pressed in sequence
                DestoryAllEnemyOnScreen();
            }
        }
    }

    #region cheat
    private void DestoryAllEnemyOnScreen()
    {
        // loop through the enemy list
        // determine if each enemy in the list is visible
        // remove the enemy that is visible
        int count = 0;
        // create a copy first
        //List<GameObject> copylist = enemies.ToList();

        foreach (GameObject enemy in enemies.ToList())
        {
            if (enemy.transform.position.y <= destroyEnemybelowHeight) // check screen position
            // not the best way but it (might) work
            {
                Destroy(enemy);
                count++;
            }
        }

        Debug.Log("Destroying " + count + " enemies visible from the screen.");
    }
    #endregion

    #region Update In-game UI display
    private void UpdateInvaderCounter()
    {
        invaderCountDisplay.text = invaderLeft.ToString();
    }

    #endregion

    #region enemy kill count
    public void EnemyKilled(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            invaderLeft = enemies.Count;

            m_ScoreSystem.playerScore(100f);
            UpdateInvaderCounter();
            m_ScoreSystem.UpdatePlayerScore();

            Debug.Log("No. of Enemy Left: " + invaderLeft);

            if (invaderLeft == 0 && OnAllEnemiesKilled != null)
            {
                Debug.Log("All enemy killed!");
                OnAllEnemiesKilled?.Invoke(); // broadcast the message
            }

        }
    }
    #endregion

    #region Game End UI
    public void LevelComplete()
    {
        gameHasEnded = true;
        Debug.Log("Cool. All Cleared.");
        levelCompletedUI.gameObject.SetActive(true);
        playerController.enabled = false;
    }

    public void PlayerHit()
    {
        gameHasEnded = true;
        Time.timeScale = 0;
        Debug.Log("Player Hit! Try again?");
        gameEndWhenHit.SetActive(true);
        playerController.gameObject.SetActive(false);
        playerController.enabled = false;
    }

    public void EnemyLanded()
    {
        gameHasEnded = true;
        Time.timeScale = 0;
        Debug.Log("Enemy Landed! Try again?");
        gameEndWhenLand.SetActive(true);
        playerController.gameObject.SetActive(false);
        playerController.enabled = false;
    }
    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion


}
