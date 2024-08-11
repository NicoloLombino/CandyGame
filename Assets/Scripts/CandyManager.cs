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
    private int nextCandyIndex;

    [Header("Gameplay")]
    [SerializeField]
    private float candySpawnPositionY;
    [SerializeField]
    private float spawnTime;

    [Header("MergePush")]
    [SerializeField]
    private bool mergePushForceActive;
    [SerializeField]
    private float mergeRadius;
    [SerializeField]
    private float pushForce;

    [Header("UI Components")]
    [SerializeField]
    private Image nextCandyImage;
    [SerializeField]
    private Toggle pushForceToggle;

    bool canSpawnCandy = false;


    private void Awake()
    {

    }
    void Start()
    {
        canSpawnCandy = true;
        LoadSave();

        CandyMergeManager.onCandyMerge += HandleCandyMerge;
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Game)
        {
            ReadInputPC();
        }
    }

    private void ReadInputPC()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!canSpawnCandy) return;

            canSpawnCandy = false;
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
        currentCandy.EnableCollider(true);
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

    public void SetRandomNextCandy()
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
        if(mergePushForceActive)
        {
            MakeMergePushForce(spawnPosition);
        }

        foreach (Candy candy in allCandyPrefabs)
        {
            if(candy.candyType == candyMergedType)
            {
                Candy mergedCandy = Instantiate(candy, spawnPosition, Quaternion.identity);
                mergedCandy.EnableRigidbody(true);
                mergedCandy.EnableCollider(true);
                break;
            }
        }
    }

    private void MakeMergePushForce(Vector2 pushPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pushPosition, mergeRadius);

        foreach(Collider2D col in colliders)
        {
            if(col.TryGetComponent(out Candy candy))
            {
                Vector2 push = ((Vector2)candy.transform.position - pushPosition).normalized;
                push *= pushForce;
                candy.GetComponent<Rigidbody2D>().AddForce(push);
            }
        }
    }

    public void TogglePushForce()
    {
        mergePushForceActive = !mergePushForceActive;
    }

    public void HandleCloseSettingsMenu()
    {
        PlayerPrefs.SetInt("PushForceActive", mergePushForceActive ? 1 : 0);
    }


    public void SetCandySpriteMode(int spriteMode)
    {
        foreach(Candy candy in allCandyPrefabs)
        {
            candy.SetSpriteMode(spriteMode);
        }
    }

    private void LoadSave()
    {
        mergePushForceActive = PlayerPrefs.GetInt("PushForceActive") == 1 ? true : false;
        pushForceToggle.isOn = mergePushForceActive;

        // to be sure
        mergePushForceActive = PlayerPrefs.GetInt("PushForceActive") == 1 ? true : false;
    }

    // TO DEBUG
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(-20, candySpawnPositionY, 0), new Vector3(20, candySpawnPositionY, 0));
    }
}
