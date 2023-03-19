using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSystem : MonoBehaviour
{
    public float score;
    public bool onBonus;
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
        onBonus = false;
    }

    // Update is called once per frame

    public void UpdatePlayerScore()
    {
        ScoreDisplay.text = score.ToString("F0");
    }

    #region Score System -> maybe in a new script
    public void AddScore(float point)
    {
        if (onBonus)
        {
            point = WithBonus(point);
        }
        Debug.Log("Added " + point);
        score += point;
        Debug.Log("Score: " + score);
        UpdatePlayerScore();
    }

    private float WithBonus(float point)
    {
        // define bonus equation here
        float newPoint = point*2;
        return newPoint;
    }
    #endregion
}
