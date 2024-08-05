using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Gameplay")]
    [SerializeField]
    private int scoreToIncreaseDifficulty;

    [Header("UI Components")]
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject inGameUI;
    [SerializeField]
    private GameObject gameoverUI;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject newBestScoreUI;

    [Header("Gameover")]
    [SerializeField]
    private LineRenderer gameoverLine;
    [SerializeField]
    private float timeToGameover;

    [Header("To Be Saved")]
    [SerializeField]
    private TextMeshProUGUI bestScoreText;

    private int currentScore;

    public Action OnDifficoultyIncreased;

    public enum GameState
    {
        Game,
        Menu,
        Gameover
    }

    public GameState gameState;

    private void Awake()
    {
        if(instance == null)
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
        SetMenu();
    }

    void Update()
    {
        
    }

    public void SetMenu()
    {
        gameState = GameState.Menu;
        menuUI.SetActive(true);
        inGameUI.SetActive(false);
        gameoverUI.SetActive(false);
        bestScoreText.text = "Best Score: \n" + PlayerPrefs.GetInt("BestScore");
    }

    public void SetGame()
    {
        gameState = GameState.Game;
        menuUI.SetActive(false);
        inGameUI.SetActive(true);
        gameoverUI.SetActive(false);
    }
    public void SetGameover()
    {
        gameState = GameState.Gameover;
        menuUI.SetActive(false);
        inGameUI.SetActive(false);
        gameoverUI.SetActive(true);
        CheckForBestScore();
    }

    public void HandleGameoverTimer(bool startTimer)
    {
        gameoverLine.gameObject.GetComponent<Animator>().SetBool("Gameover", startTimer);
    }

    public float GetTimeToGameover()
    {
        return timeToGameover;
    }
    public float GetGameoverLinePositionY()
    {
        return gameoverLine.transform.position.y;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        scoreText.text = "Score: \n" + currentScore;

        if(currentScore >= scoreToIncreaseDifficulty && GetGameoverLinePositionY() > 1.5f)
        {
            float positionToGive = GetGameoverLinePositionY() - 1.0f;
            positionToGive = Mathf.Max(positionToGive, 1.5f);
            gameoverLine.transform.position = new Vector3(0, positionToGive, 0);
            scoreToIncreaseDifficulty *= 2;
            OnDifficoultyIncreased?.Invoke();
        }
    }

    private void CheckForBestScore()
    {
        if(currentScore >= PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            newBestScoreUI.SetActive(true);
        }
    }

    private void OnApplicationQuit()
    {
        
    }
}
