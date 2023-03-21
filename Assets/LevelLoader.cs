using Platformer;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    // this script is attached to the Canvas
    GameManager _gameManager;
    public Button level1_Button;
    public Button level2_Button;
    public Button level3_Button;
    public GameObject menuCanvas;
    public GameObject level1_Object;
    public GameObject level2_Object;
    public GameObject level3_Object;
    public SpriteRenderer level_1_bg;
    public SpriteRenderer level_2_bg;
    public SpriteRenderer level_3_bg;


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

        StartCoroutine(Countdown(level1_Object, level_1_bg));
    }

    private void OnLevel2ButtonClick()
    {
        StartCoroutine(Countdown(level2_Object, level_2_bg));
    }

    private void OnLevel3ButtonClick()
    {
        StartCoroutine(Countdown(level3_Object, level_3_bg));
    }

    private IEnumerator Countdown(GameObject levelObject, SpriteRenderer bg)
    {
        // 3-2-1-GO!
        menuCanvas.SetActive(false);
        bg.gameObject.SetActive(true);
        _gameManager.backgroundAudio.Stop();

        // turn on the background object

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

        // Enable the level objects (it will drop down)
        levelObject.SetActive(true);
        _gameManager.StartSelectedLevel();
    }
}