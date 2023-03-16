using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    internal static GameManager _instance;
    internal void PlaySfx(AudioClip clip) => sfx.PlayOneShot(clip);

    [SerializeField] private AudioSource sfx;
   
    public PlayerController playerController;
    public ScoreSystem m_ScoreSystem;
    public Transform bonusRegion;
    private float bonusRegionTop;
    private float bonusRegionBottom;
    private float regionHeight;

    public bool isGameOver = false;
    public delegate void GameEndEvent();
    public static event GameEndEvent OnGameEnd;

    #region UI
    public GameObject gameEndWhenHit;
    public GameObject gameEndWhenLand;
    public GameObject levelCompletedUI;
    
    public TextMeshProUGUI invaderCountDisplay;
    public TextMeshProUGUI ScoreDisplay;
    private int invaderLeft;
    #endregion

    #region enemy
    [SerializeField]private List<GameObject> enemies;  

    public delegate void AllEnemiesKilled(); // template
    public static event AllEnemiesKilled OnAllEnemiesKilled; // instance
    [SerializeField] private float destroyEnemybelowHeight = 9.4f;
    //public GameObject[] enemyPrefabs;
    #endregion

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
        // turn off UI
        gameEndWhenHit.SetActive(false);
        gameEndWhenLand.SetActive(false);
        levelCompletedUI.SetActive(false);

        regionHeight = 2f;
        bonusRegionTop = bonusRegion.position.y + regionHeight / 2;
        bonusRegionBottom = bonusRegion.position.y - regionHeight / 2;

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

    #region Game End

    public void EndGame()
    {
        isGameOver = true;
        OnGameEnd?.Invoke();
    }

    //Update In-game UI display
    private void UpdateInvaderCounter()
    {
        invaderCountDisplay.text = invaderLeft.ToString();
    }

    #endregion

    #region enemy kill count
    // maybe have an enemy controller as a separate class to define enemy features
    public void EnemyKilled(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            invaderLeft = enemies.Count;
            float enemyTransformY = enemy.transform.position.y;
            float point = 100 + (12f - enemyTransformY) * 10;

            if (EnemyInBonusRegion(enemyTransformY))
            {
                point *= 2;
            }
            m_ScoreSystem.AddScore(point);
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

    private bool EnemyInBonusRegion(float enemyPosition)
    {
        Debug.Log("Hit enemy in bonus region.");
        return enemyPosition >= bonusRegionBottom && enemyPosition <= bonusRegionTop;

    }
    #endregion

    #region Game End UI
    public void LevelComplete()
    {
        EndGame();
        Debug.Log("Cool. All Cleared.");
        levelCompletedUI.gameObject.SetActive(true);
        playerController.enabled = false;
    }

    public void PlayerHit()
    {
        EndGame();
        Time.timeScale = 0; // freeze the game
        Debug.Log("Player Hit! Try again?");
        gameEndWhenHit.SetActive(true);
        playerController.gameObject.SetActive(false);
        playerController.enabled = false;
    }

    public void EnemyLanded()
    {
        isGameOver = true;
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
