using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    [Header("")]
    [SerializeField]
    private GameObject candyPrefab;
    [SerializeField]
    private LineRenderer candyThrowLine;

    [Header("Components")]
    [SerializeField]
    private float candySpawnPositionY;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReadInputPC();
    }

    private void ReadInputPC()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ShowThrowLine(true);
            MoveThrowLine();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            SpawnCandy();
            ShowThrowLine(false);
        }
        else if(Input.GetMouseButton(0))
        {
            MoveThrowLine();
        }
    }

    private void SpawnCandy()
    {
        Vector2 spawnPosition = GetTouchedPositionFromSpawnPosition();
        Instantiate(candyPrefab, spawnPosition, Quaternion.identity);
    }

    private void ShowThrowLine(bool show)
    {
        candyThrowLine.enabled = show;
    }

    private void MoveThrowLine()
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

    // TO DEBUG
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(-20, candySpawnPositionY, 0), new Vector3(20, candySpawnPositionY, 0));
    }
}
