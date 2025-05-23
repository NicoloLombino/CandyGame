using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Candy : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D circleCollider2D;

    [Header("Gameplay")]
    [SerializeField]
    private int scoreOnCollision;

    [Header("Components")]
    [SerializeField]
    private Sprite spriteCandy;
    [SerializeField]
    private Sprite spriteLollipop;
    [SerializeField]
    private Sprite spriteCake;
    [SerializeField]
    private Sprite spriteIceCream;
    public GameObject mergeParticles;

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
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        GameManager.instance.OnDifficoultyIncreased += HandleDifficultyIncreased;

        gameoverLinePositionY = GameManager.instance.GetGameoverLinePositionY();
        timeToGameover = GameManager.instance.GetTimeToGameover();

        SetSpriteMode(PlayerPrefs.GetInt("CandySpriteMode", 0));
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

    public void EnableCollider(bool enable)
    {
        circleCollider2D.enabled = enable;
    }

    public void MoveCandy(Vector2 movePosition)
    {
        transform.position = movePosition;
    }

    public Sprite GetCandySprite()
    {
        return candySpriteRenderer.sprite;
    }

    public Sprite Fix_SpriteNextCandy()
    {
        Sprite tempSprite = null;
        switch (GameManager.instance.FIX_SPRITE_NEXT_CANDY)
        {
            case 0:
                tempSprite = spriteCandy;
                break;
            case 1:
                tempSprite = spriteLollipop;
                break;
            case 2:
                tempSprite = spriteCake;
                break;
            case 3:
                tempSprite = spriteIceCream;
                break;
        }

        Debug.Log(GameManager.instance.FIX_SPRITE_NEXT_CANDY);
        return tempSprite;
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
        GameManager.instance.FIX_SPRITE_NEXT_CANDY = spriteMode;

        switch (spriteMode)
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
        if(collision.gameObject.tag == "Floor")
        {
            hasCollide = true;
        }

        if (collision.collider.TryGetComponent(out Candy collisionCandy))
        {
            hasCollide = true;
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
