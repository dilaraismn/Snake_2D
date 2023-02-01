using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AppleScript : MonoBehaviour
{
    public BoxCollider2D gameAreaCollider;
    public GameObject snake;
    private float minDistance = 5;
    private float minAppleDistance = 5;
    private List<GameObject> allActiveApples = new List<GameObject>();

    private void Start()
    {
        if (!UIManager.isGameStarted)
        {
            return;
        }
        
        RandomizePosition();
    }
    
    public void RandomizePosition()
    {
        while (Vector2.Distance(this.transform.position, snake.transform.position) < minDistance 
               || IsTooCloseToTail(this.transform.position) 
               || IsApplesTooClose(this.transform.position))
        {
            Bounds bounds = this.gameAreaCollider.bounds;
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            x = Mathf.Round(x);
            y = Mathf.Round(y);

            transform.position = new Vector2(x, y);
        }
    }

    private bool IsTooCloseToTail(Vector2 applePosition)
    {
        foreach (Transform tailSegment in SnakeScript._segments)
        {
            if (Vector2.Distance(applePosition, tailSegment.transform.position) < minDistance)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsApplesTooClose(Vector2 applePos)
    {
        foreach (GameObject apple in allActiveApples)
        {
            if (apple.activeInHierarchy)
            {
                allActiveApples.Add(apple);
            }

            if (Vector3.Distance(applePos, apple.transform.position) < minAppleDistance)
            {
                return true;
            }
        }
        return false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Snake"))
        {
            RandomizePosition();
        }
    }
}
