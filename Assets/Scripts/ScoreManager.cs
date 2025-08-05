using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    public FruitControlManager fruitControlManager;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI bestScoreTxt;
    
    [SerializeField] GameOverUI gameOverUI;

    private int _score;
    private int _bestScore;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            UpdateScoreTxt();
        }
    }

    public int BestScore
    {
        get => _bestScore;
        set
        {
            _bestScore = value;
            UpdateBestScoreTxt();
        }
    }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        LoadBestScore();
    }

    private void Start()
    {
        UpdateScoreTxt();
        UpdateBestScoreTxt();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DroppedFruit"))
        {
            Debug.Log("게임종료");

            Time.timeScale = 0;
            fruitControlManager.isGameOver = true;

            if (_score > _bestScore)
            {
                _bestScore = _score;
                PlayerPrefs.SetInt("BestScore", _bestScore);
            };
            
            gameOverUI.ActivateGameOverUI(Score);
        }
    }

    private void LoadBestScore()
    {
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    private void UpdateScoreTxt()
    {
        if (scoreTxt != null)
            scoreTxt.text = _score.ToString();
    }
    
    private void UpdateBestScoreTxt()
    {
        if (bestScoreTxt != null) 
            bestScoreTxt.text = _bestScore.ToString();
    }

    public void AddScore(int fruitLvl)
    {
        int points = (fruitLvl + 1) * (fruitLvl + 2) / 2;
        Score += points;
    }
}