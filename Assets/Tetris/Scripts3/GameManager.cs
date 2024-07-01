using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MyBlockController Current { get; set; }

    private const int GridSizeX = 10;
    private const int GridSizeY = 20;

    public bool[,] Grid = new bool[GridSizeX, GridSizeY];

    public float GameSpeed => gameSpeed;
    [SerializeField, Range(.1f, 1f)] private float gameSpeed = 1;

    [SerializeField] private List<MyBlockController> listPrefabs;

    private List<MyBlockController> _listHistory = new List<MyBlockController>();
    
    #region Score
    private int score;
    public int Score
    {
        get => score;
        private set
        {
            score = value;
            Debug.Log("Score updated: " + score); 
            OnScoreUpdated?.Invoke(score);
            
            if (score > HighScore)
            {
                HighScore = score;
                OnHighScoreUpdated?.Invoke(HighScore);
                PlayerPrefs.SetInt("HighScore", HighScore); // HighScore'ı kaydet
            }
        }
    }

    private int highScore;
    public int HighScore
    {
        get => highScore;
        private set
        {
            highScore = value;
            OnHighScoreUpdated?.Invoke(highScore);
        }
    }

    public event Action<int> OnScoreUpdated;
    public event Action<int> OnHighScoreUpdated;
    #endregion

    #region Test

    public bool IsOpenTest;

    [SerializeField] private SpriteRenderer displayDataPrefabs;
    private SpriteRenderer[,] previewDisplay = new SpriteRenderer[GridSizeX, GridSizeY];

    private void UpdateDisplayPreview()
    {
        if (!IsOpenTest) return;

        for (int i = 0; i < GridSizeX; i++)
        {
            for (int j = 0; j < GridSizeY; j++)
            {
                var active = Grid[i, j];
                var sprite = previewDisplay[i, j];

                sprite.color = active ? Color.green : Color.red;

                var color = sprite.color;
                color.a = .5f;
                sprite.color = color;
            }
        }
    }

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (IsOpenTest)
        {
            for (int i = 0; i < GridSizeX; i++)
            {
                for (int j = 0; j < GridSizeY; j++)
                {
                    var sprite = Instantiate(displayDataPrefabs, transform);
                    sprite.transform.position = new Vector3(i, j, 0);
                    previewDisplay[i, j] = sprite;
                }
            }
        }

        HighScore = PlayerPrefs.GetInt("HighScore", 0); // HighScore'ı yükle
    }
    private void Start()
    {
        Score = 0; // Initialize score
        Spawn();
    }

    public bool IsInside(List<Vector2> listCoordinate)
    {
        foreach (var coordinate in listCoordinate)
        {
            int x = Mathf.RoundToInt(coordinate.x);
            int y = Mathf.RoundToInt(coordinate.y);

            if (x < 0 || x >= GridSizeX)
            {
                //Horizontal out
                return false;
            }

            if (y < 0 || y >= GridSizeY)
            {
                //Vertical out
                return false;
            }

            if (Grid[x, y])
            {
                //Hit something
                return false;
            }
        }

        return true;
    }

    public void Spawn()
    {
        var index = Random.Range(0, listPrefabs.Count);
        var blockController = listPrefabs[index];
        var newBlock = Instantiate(blockController);
        Current = newBlock;
        _listHistory.Add(newBlock);

        UpdateDisplayPreview();
    }

    private bool IsFullRow(int index)
    {
        for (int i = 0; i < GridSizeX; i++)
        {
            if (!Grid[i, index])
            {
                Debug.Log("Row " + index + " is not full.");
                return false;
            }
        }
        Debug.Log("Row " + index + " is full.");
        return true;
    }

   public void UpdateRemoveObjectController()
{
    bool rowDeleted = false;

    for (int i = 0; i < GridSizeY; i++)
    {
        if (IsFullRow(i))
        {
            rowDeleted = true;
            Debug.Log("Row " + i + " is full. Clearing row.");

            //Remove
            foreach (var myBlock in _listHistory)
            {
                var willDestroy = new List<Transform>();
                foreach (var piece in myBlock.ListPiece)
                {
                    int y = Mathf.RoundToInt(piece.position.y);
                    if (y == i)
                    {
                        //Add Remove
                        willDestroy.Add(piece);
                    }
                }

                //Remove
                foreach (var item in willDestroy)
                {
                    myBlock.ListPiece.Remove(item);
                    Destroy(item.gameObject);
                }
            }

            //ChangeData
            for (int j = 0; j < GridSizeX; j++)
                Grid[j, i] = false;

            //Move down all rows above the cleared row
            for (int j = i + 1; j < GridSizeY; j++)
            {
                for (int k = 0; k < GridSizeX; k++)
                {
                    Grid[k, j - 1] = Grid[k, j];
                }
            }

            // Clear the top row
            for (int j = 0; j < GridSizeX; j++)
                Grid[j, GridSizeY - 1] = false;

            // Move blocks down visually
            foreach (var myBlock in _listHistory)
            {
                foreach (var piece in myBlock.ListPiece)
                {
                    int y = Mathf.RoundToInt(piece.position.y);
                    if (y > i)
                    {
                        var position = piece.position;
                        position.y--;
                        piece.position = position;
                    }
                }
            }
        }
    }

    if (rowDeleted)
    {
        Debug.Log("Row(s) deleted. Updating score.");
        Score += 150; // Add score for clearing a row
        UpdateRemoveObjectController(); // Satır temizleme işlemini tekrar kontrol et
    }
}

  

    public void OnGameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0;
    }
}
