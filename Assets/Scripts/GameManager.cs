using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    

    [Header("UI Components")]
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject inGameUI;
    [SerializeField]
    private GameObject gameoverUI;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [Header("Gameover")]
    [SerializeField]
    private LineRenderer gameoverLine;
    [SerializeField]
    private float timeToGameover;

    private int score;

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
        score += scoreToAdd;
        scoreText.text = "Score: \n" + score;
    }
}
