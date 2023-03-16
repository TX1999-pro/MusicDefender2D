using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSystem : MonoBehaviour
{
    private float score = 0;
    public TextMeshProUGUI ScoreDisplay;
    // Start is called before the first frame update

    public static ScoreSystem instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        score = 0;
    }

    // Update is called once per frame

    public void UpdatePlayerScore()
    {
        ScoreDisplay.text = score.ToString("F0");
    }

    #region Score System -> maybe in a new script
    public void AddScore(float point)
    {
        Debug.Log("Added " + point);
        score += point;
        Debug.Log("Score: " + score);
    }
    #endregion
}
