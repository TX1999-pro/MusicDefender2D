using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private float score = 0;
    public TextMeshProUGUI ScoreDisplay;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdatePlayerScore()
    {
        ScoreDisplay.text = score.ToString("F0");
    }

    #region Score System -> maybe in a new script
    public void playerScore(float point)
    {
        score += point;
    }
    #endregion
}
