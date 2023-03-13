using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteDisplay : MonoBehaviour
{
    public GameObject LevelCompleteUI;

    private void OnEnable()
    {
        GameManager.OnAllEnemiesKilled += DisplayPanel;
    }
    private void OnDisable()
    {
        GameManager.OnAllEnemiesKilled -= DisplayPanel;
    }
    void DisplayPanel()
    {
        LevelCompleteUI.gameObject.SetActive(true);
    }
}
