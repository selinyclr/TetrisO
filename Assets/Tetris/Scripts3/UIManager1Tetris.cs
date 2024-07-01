using UnityEngine;
using UnityEngine.UI;

public class UIManager1Tetris : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    private void OnEnable()
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.OnScoreUpdated += UpdateScoreText;
            GameManager.Instance.OnHighScoreUpdated += UpdateHighScoreText;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.OnScoreUpdated -= UpdateScoreText;
            GameManager.Instance.OnHighScoreUpdated -= UpdateHighScoreText;
        }
    }

    private void Start()
    {
        GameManager.Instance.OnScoreUpdated += UpdateScoreText;
        GameManager.Instance.OnHighScoreUpdated += UpdateHighScoreText;
        
        UpdateScoreText(GameManager.Instance.Score);
        UpdateHighScoreText(GameManager.Instance.HighScore);
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