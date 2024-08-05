using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Candy : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField]
    private int scoreOnCollision;
    Rigidbody2D rb;

    [Header("Gameplay")]
    [SerializeField]
    private Sprite spriteCandy;
    [SerializeField]
    private Sprite spriteLollipop;
    [SerializeField]
    private Sprite spriteCake;
    [SerializeField]
    private Sprite spriteIceCream;

    public static Action<Candy, Candy> onCandyCollision;

    public SpriteRenderer candySpriteRenderer;

    private float gameoverLinePositionY;
    private float timeToGameover;
    private float gameoverTimer;
    private bool hasCollide;
    private bool hasStartGameoverTimer;

    public enum CandyType
    {
        candyBase0,
        candyBase1,
        candyBase2,
        candyBase3,
        candyBase4,
        candyBase5,
        candyBase6
    }

    public CandyType candyType;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        GameManager.instance.OnDifficoultyIncreased += HandleDifficultyIncreased;

        gameoverLinePositionY = GameManager.instance.GetGameoverLinePositionY();
        timeToGameover = GameManager.instance.GetTimeToGameover();

        SetSpriteMode(PlayerPrefs.GetInt("CandySpriteMode"));
    }

    void Update()
    {
        if(hasCollide && IsOnGameoverLine() && !hasStartGameoverTimer)
        {
            ManageGameoverTimer(true);
        }

        if(hasStartGameoverTimer)
        {
            CheckGameoverTimer();
        }
    }

    private void OnDestroy()
    {
        if(hasStartGameoverTimer)
        {
            ManageGameoverTimer(false);
        }
    }

    public void EnableRigidbody(bool enable)
    {
        rb.bodyType = enable? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
    }

    public void MoveCandy(Vector2 movePosition)
    {
        transform.position = movePosition;
    }

    public Sprite GetCandySprite()
    {
        return candySpriteRenderer.sprite;
    }

    private bool IsOnGameoverLine()
    {
        return transform.position.y >= gameoverLinePositionY;
    }

    private void ManageGameoverTimer(bool startTimer)
    {
        GameManager.instance.HandleGameoverTimer(startTimer);

        if (startTimer)
        {
            hasStartGameoverTimer = true;
        }
        else
        {
            gameoverTimer = 0;
            hasStartGameoverTimer = false;
        }
    }

    private void CheckGameoverTimer()
    {
        gameoverTimer += Time.deltaTime;
        if(gameoverTimer >= timeToGameover)
        {
            GameManager.instance.SetGameover();
        }

        if(!IsOnGameoverLine())
        {
            ManageGameoverTimer(false);
        }
    }

    public void SetSpriteMode(int spriteMode)
    {
        switch(spriteMode)
        {
            case 0:
                candySpriteRenderer.sprite = spriteCandy;
                break;
            case 1:
                candySpriteRenderer.sprite = spriteLollipop;
                break;
            case 2:
                candySpriteRenderer.sprite = spriteCake;
                break;
            case 3:
                candySpriteRenderer.sprite = spriteIceCream;
                break;
        }

        PlayerPrefs.SetInt("CandySpriteMode", spriteMode);
    }

    private void HandleDifficultyIncreased()
    {
        gameoverLinePositionY = GameManager.instance.GetGameoverLinePositionY();
    }

    private void HandleCollision(Collision2D collision)
    {
        hasCollide = true;

        if (collision.collider.TryGetComponent(out Candy collisionCandy))
        {
            if (collisionCandy.candyType == this.candyType)
            {
                onCandyCollision?.Invoke(this, collisionCandy);
                GameManager.instance.AddScore(scoreOnCollision);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }
}
