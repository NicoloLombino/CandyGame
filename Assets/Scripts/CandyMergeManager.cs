using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CandyMergeManager : MonoBehaviour
{
    Candy lastCandyCaller;

    public static Action<Candy.CandyType, Vector2> onCandyMerge;

    void Start()
    {
        Candy.onCandyCollision += HandleCandyCollision;
    }

    void Update()
    {
        
    }

    private void HandleCandyCollision(Candy candyCaller, Candy collisionCandy)
    {
        if (lastCandyCaller) return;

        Candy.CandyType candyCallerType = candyCaller.candyType;
        candyCallerType += 1;

        Vector2 candySpawnPosition =
            (candyCaller.transform.position + collisionCandy.transform.position) / 2;

        onCandyMerge?.Invoke(candyCallerType, candySpawnPosition);

        lastCandyCaller = candyCaller;
        MergeCandy(candyCaller, collisionCandy);
    }

    private void MergeCandy(Candy candy1, Candy candy2)
    {
        Destroy(candy1.gameObject);
        Destroy(candy2.gameObject);
        StartCoroutine(ResetLastCandyCaller());
    }

    private IEnumerator ResetLastCandyCaller()
    {
        yield return new WaitForEndOfFrame();
        lastCandyCaller = null;
    }
}
