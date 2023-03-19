using Platformer;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    GameManager _gameManager;
    public Button level1_Button;
    public Button level2_Button;
    public Button level3_Button;
    public GameObject menuCanvas;
    public GameObject level1_Object;
    public GameObject level2_Object;
    public GameObject level3_Object;


    private void OnEnable()
    {
        level1_Button.onClick.AddListener(OnLevel1ButtonClick);
        level2_Button.onClick.AddListener(OnLevel2ButtonClick);
        level3_Button.onClick.AddListener(OnLevel3ButtonClick);
    }

    private void OnDisable()
    {
        level1_Button.onClick.RemoveListener(OnLevel1ButtonClick);
        level2_Button.onClick.RemoveListener(OnLevel2ButtonClick);
        level3_Button.onClick.RemoveListener(OnLevel3ButtonClick);
    }
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }


    private void OnLevel1ButtonClick()
    {

        StartCoroutine(Countdown(level1_Object));
    }

    private void OnLevel2ButtonClick()
    {
        StartCoroutine(Countdown(level2_Object));
    }

    private void OnLevel3ButtonClick()
    {
        StartCoroutine(Countdown(level3_Object));
    }

    private IEnumerator Countdown(GameObject levelObject)
    {
        // 3-2-1-GO!
        menuCanvas.SetActive(false);
        _gameManager.backgroundAudio.Stop();
        // set the gameObject to be true
        _gameManager.countdownText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        // Display "3"
        _gameManager.countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);

        // Display "2"
        _gameManager.countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);

        // Display "1"
        _gameManager.countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);

        // Display "Go!"
        _gameManager.countdownText.text = "Go!";
        yield return new WaitForSeconds(1.0f);

        // Hide the countdown text
        _gameManager.countdownText.gameObject.SetActive(false);

        // Enable the desired behavior for your game objects (e.g., dropping down)
        levelObject.SetActive(true);
        _gameManager.StartSelectedLevel();
    }
}