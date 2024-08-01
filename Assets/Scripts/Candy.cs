using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Candy : MonoBehaviour
{
    Rigidbody2D rb;
    public static Action<Candy, Candy> onCandyCollision;

    public SpriteRenderer candySpriteRenderer;

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
        
    }

    void Update()
    {
        
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Candy collisionCandy))
        {
            if(collisionCandy.candyType == this.candyType)
            {
                onCandyCollision?.Invoke(this, collisionCandy);
            }
        }
    }
}
