using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
   private Vector2 _direction = Vector2.right;
   private List<Transform> _segments = new List<Transform>();
   public Transform segmentPrefab;

   private void Start()
   {
      ResetSnake();
   }

   private void Update()
   {
      if (_direction.x != 0f)
      {
         if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
         {
            _direction = Vector2.up;
         } 
         else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
         {
            _direction = Vector2.down;
         }
      }

      else if (_direction.y != 0f)
      {
         if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
         {
            _direction = Vector2.right;
         } 
         else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
         {
            _direction = Vector2.left;
         }
      }
      
    /*if (Input.GetKeyDown(KeyCode.W))
      {
         _direction = Vector2.up;
      }
      else if (Input.GetKeyDown(KeyCode.S))
      {
         _direction = Vector2.down;
      }
      else if (Input.GetKeyDown(KeyCode.A))
      {
         _direction = Vector2.left;
      }
      else if (Input.GetKeyDown(KeyCode.D))
      {
         _direction = Vector2.right;
      }*/
   }

   private void FixedUpdate()
   {
      for (int i = _segments.Count -1; i > 0; i--)
      {
         _segments[i].position = _segments[i - 1].position;
      } 
      
      float x = Mathf.Round(transform.position.x) + _direction.x;
      float y = Mathf.Round(transform.position.y) + _direction.y;

      transform.position = new Vector2(x, y);
   }
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Apple"))
      {
         Grow();
      }

      if (other.CompareTag("Obstacle"))
      {
         ResetSnake();
      }
   }
   
   private void Grow()
   {
      Transform segment = Instantiate(segmentPrefab, transform.parent);
      segment.position = _segments[_segments.Count - 1].position;
      _segments.Add(segment);
   }

   private void ResetSnake()
   {
      _direction = Vector2.right;
      transform.position = Vector3.zero;
      
      for (int i = 1; i < _segments.Count; i++)
      {
         Destroy(_segments[i].gameObject);
      }
      
      _segments.Clear();
      _segments.Add(this.transform);
      
   }
   
}
