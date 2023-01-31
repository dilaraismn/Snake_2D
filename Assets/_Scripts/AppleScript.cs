using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AppleScript : MonoBehaviour
{
    public BoxCollider2D gameAreaCollider;

    private void Start()
    {
        if (!UIManager.isGameStarted)
        {
            return;
        }
        
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.gameAreaCollider.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        
        x = Mathf.Round(x);
        y = Mathf.Round(y);

        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       RandomizePosition();
    }
}
