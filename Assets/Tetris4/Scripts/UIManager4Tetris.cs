using UnityEngine;
using UnityEngine.UI;

public class UIManager4Tetris : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    private void OnEnable()
    {
        if (GameManager4.Instance != null) 
        {
            GameManager4.Instance.OnScoreUpdated += UpdateScoreText;
            GameManager4.Instance.OnHighScoreUpdated += UpdateHighScoreText;
        }
    }

    private void OnDisable()
    {
        if (GameManager4.Instance != null) 
        {
            GameManager4.Instance.OnScoreUpdated -= UpdateScoreText;
            GameManager4.Instance.OnHighScoreUpdated -= UpdateHighScoreText;
        }
    }

    private void Start()
    {
        GameManager4.Instance.OnScoreUpdated += UpdateScoreText;
        GameManager4.Instance.OnHighScoreUpdated += UpdateHighScoreText;
        
        UpdateScoreText(GameManager4.Instance.Score);
        UpdateHighScoreText(GameManager4.Instance.HighScore);
    }

    private void UpdateScoreText(int newScore)
    {
        Debug.Log("Updating score text: " + newScore);
        scoreText.text = "Skor: " + newScore;
    }

    private void UpdateHighScoreText(int newHighScore)
    {
        Debug.Log("Updating high score text: " + newHighScore);
        highScoreText.text = "H" +
                             "" +
                             "" +
                             "igh Skor: " + newHighScore;
    }
}