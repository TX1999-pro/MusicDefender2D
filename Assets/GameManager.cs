using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class GameManager : MonoBehaviour
{
    internal static GameManager _instance;
    internal void PlaySfx(AudioClip clip) => sfx.PlayOneShot(clip);

    [SerializeField] private AudioSource sfx;
    
    private bool gameHasEnded = false;
    
    public GameObject gameEndWhenHit;
    public GameObject gameEndWhenLand;
    public GameObject levelCompletedUI;
    public PlayerController playerController;

    private int invaderLeft;
    [SerializeField]private List<GameObject> enemies;

    
    public delegate void AllEnemiesKilled(); // template
    public static event AllEnemiesKilled OnAllEnemiesKilled; // instance
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
        Debug.Log("No. of Enemy Spawn: " + invaderLeft);
    }


    #region testing with enemy kill count
    public void EnemyKilled(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            invaderLeft = enemies.Count;
            Debug.Log("No. of Enemy Spawn: " + invaderLeft);
            if (invaderLeft == 0 && OnAllEnemiesKilled != null)
            {
                Debug.Log("All enemy killed!");
                OnAllEnemiesKilled?.Invoke(); // broadcast the message
            }

        }
    }
    #endregion
    public void LevelComplete()
    {
        gameHasEnded = true;
        Debug.Log("Cool. All Cleared.");
        levelCompletedUI.SetActive(true);
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

}
