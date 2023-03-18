using Platformer;
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
        menuCanvas.SetActive(false);
        level1_Object.SetActive(true);
        _gameManager.StartSelectedLevel();
    }

    private void OnLevel2ButtonClick()
    {
        menuCanvas.SetActive(false);
        level2_Object.SetActive(true);
        _gameManager.StartSelectedLevel();
    }

    private void OnLevel3ButtonClick()
    {
        menuCanvas.SetActive(false);
        level3_Object.SetActive(true);
        _gameManager.StartSelectedLevel();
    }
}