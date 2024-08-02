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
        gameoverLinePositionY = GameManager.instance.GetGameoverLinePositionY();
        timeToGameover = GameManager.instance.GetTimeToGameover();
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
