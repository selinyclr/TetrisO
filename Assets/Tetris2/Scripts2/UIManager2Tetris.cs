using UnityEngine;
using UnityEngine.UI;

public class UIManager2Tetris : MonoBehaviour
{
    public static UIManager2Tetris Instance; 

    public Text scoreText; 
    public Text highScoreText; // En yüksek skoru göstermek için yeni bir Text bileşeni
    private int score; 
    private int highScore; // En yüksek skoru tutmak için yeni bir değişken

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of UIManager already exists. Destroying this one.");
            Destroy(this);
        }
    }

    private void Start()
    {
        score = 0; 
        highScore = PlayerPrefs.GetInt("HighScore", 0); // En yüksek skoru yükle
        UpdateScoreUI();
        UpdateHighScoreUI(); // Oyunun başında yüksek skoru güncelle
    }

    public void AddScore(int amount)
    {
        score += amount; 
        UpdateScoreUI();
        CheckHighScore();
    }

    private void CheckHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // En yüksek skoru sakla
            UpdateHighScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString(); 
        }
    }
    
    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Skor: " + highScore.ToString(); 
        }
    }

    public void OnLineDeleted(int linesCleared)
    {
        AddScore(linesCleared * 15); 
    }
}