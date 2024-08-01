using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CandyManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Candy[] candySpawnablePrefabs;
    [SerializeField]
    private Candy[] allCandyPrefabs;
    private Candy currentCandy;
    [SerializeField]
    private LineRenderer candyThrowLine;
    [SerializeField]
    private float candySpawnPositionY;
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private int nextCandyIndex;

    [Header("UI Components")]
    [SerializeField]
    private Image nextCandyImage;


    bool canSpawnCandy = false;

    void Start()
    {
        canSpawnCandy = true;
        CandyMergeManager.onCandyMerge += HandleCandyMerge;
        SetRandomNextCandy();
    }

    void Update()
    {
        ReadInputPC();
    }

    private void ReadInputPC()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!canSpawnCandy) return;

            SpawnRandomCandy();
            ShowThrowLine(true);
            MoveThrowLineInPressedPosition();
            SetRandomNextCandy();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            ThrowCandy();
        }
        else if(Input.GetMouseButton(0))
        {
            MoveThrowLineInPressedPosition();
            if (currentCandy)
            {
                currentCandy.MoveCandy(GetTouchedPositionFromSpawnPosition());
            }
        }
    }

    private void SpawnRandomCandy()
    {
        Vector2 spawnPosition = GetTouchedPositionFromSpawnPosition();
        currentCandy = Instantiate(candySpawnablePrefabs[nextCandyIndex], spawnPosition, Quaternion.identity);
    }

    private void ShowThrowLine(bool show)
    {
        candyThrowLine.enabled = show;
    }

    private void ThrowCandy()
    {
        if (!currentCandy) return;

        ShowThrowLine(false);
        currentCandy.EnableRigidbody(true);
        canSpawnCandy = false;
        StartCoroutine(SpawnTimer(spawnTime));
        currentCandy = null;
    }

    private void MoveThrowLineInPressedPosition()
    {
        Vector2 lineStartPosition = GetTouchedPositionFromSpawnPosition();
        candyThrowLine.SetPosition(0, lineStartPosition);
        candyThrowLine.SetPosition(1, lineStartPosition + Vector2.down * 10);
    }

    private Vector2 GetTouchedPositionFromSpawnPosition()
    {
        Vector2 touchedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchedPosition.y = candySpawnPositionY;
        return touchedPosition;
    }

    private void SetRandomNextCandy()
    {
        nextCandyIndex = Random.Range(0, candySpawnablePrefabs.Length);
        nextCandyImage.sprite = candySpawnablePrefabs[nextCandyIndex].GetCandySprite();
    }

    private IEnumerator SpawnTimer(float spawnTime)
    {
        yield return new WaitForSecondsRealtime(spawnTime);
        canSpawnCandy = true;
    }

    private void HandleCandyMerge(Candy.CandyType candyMergedType, Vector2 spawnPosition)
    {
        foreach(Candy candy in allCandyPrefabs)
        {
            if(candy.candyType == candyMergedType)
            {
                Candy mergedCandy = Instantiate(candy, spawnPosition, Quaternion.identity);
                mergedCandy.EnableRigidbody(true);
                break;
            }
        }
    }

    // TO DEBUG
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(-20, candySpawnPositionY, 0), new Vector3(20, candySpawnPositionY, 0));
    }
}
