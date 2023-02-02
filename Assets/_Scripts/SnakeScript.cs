using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SnakeScript : MonoBehaviour
{
   private Vector2 _direction = Vector2.right;
   public static List<Transform> _segments = new List<Transform>();
   public Transform segmentPrefab;
   private AudioSource _audioSource;
   public AudioClip sfx_Movement, sfx_Apple, sfx_Hit;
   private int appleCount, highScore;
   public TMP_Text text_appleCount, text_highScore;
   public GameObject startUI, muteImage, pauseUI;
   private string currentScene;
   [SerializeField] private string sceneName, key;
   public static bool isGamePaused;
   
   private void Start()
   {
      currentScene = SceneManager.GetActiveScene().name;
      _audioSource = GetComponent<AudioSource>();
      _audioSource.Play(); //game music
      ResetSnake();
   }

   private void SetPlayPrefs(string sceneName, string key, int value)
   {
      if (currentScene == sceneName)
      {
         PlayerPrefs.SetInt(key, value);
      }
   }
   
   private void GetPlayPrefs(string sceneName, string key, int value)
   {
      string currentScene = SceneManager.GetActiveScene().name;
      
      if (currentScene == sceneName)
      {
         PlayerPrefs.SetInt(key, value);
      }
   }
   
   private void Update()
   {
      if (UIManager.isMute)
      {
         _audioSource.mute = true;
         muteImage.SetActive(true);
      }
      else
      {
         _audioSource.mute = false;
         muteImage.SetActive(false);
      }
      
      text_appleCount.text = appleCount.ToString();

      if (!UIManager.isGameStarted) return;

      if (_direction.x != 0f)
      {
         if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
         {
            _direction = Vector2.up;
            _audioSource.PlayOneShot(sfx_Movement);
            transform.eulerAngles = new Vector3 (0, 0, 180);
         } 
         else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
         {
            _direction = Vector2.down;
            _audioSource.PlayOneShot(sfx_Movement);
            transform.eulerAngles = new Vector3 (0, 0,0);
         }
      }

      else if (_direction.y != 0f)
      {
         if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
         {
            _direction = Vector2.right;
            _audioSource.PlayOneShot(sfx_Movement);
            transform.eulerAngles = new Vector3 (0, 0,90);
         } 
         else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
         {
            _direction = Vector2.left;
            _audioSource.PlayOneShot(sfx_Movement);
            transform.eulerAngles = new Vector3 (0, 0,-90);
         }
      }

      if (Input.GetKeyDown(KeyCode.Escape))
      {
         isGamePaused = !isGamePaused;
      }

      if (isGamePaused)
      {
         pauseUI.SetActive(true);
         Time.timeScale = 0f;
      }
      else
      {
         pauseUI.SetActive(false);
         Time.timeScale = 1;
      }
   }

   private void FixedUpdate()
   {
      if (UIManager.isGameStarted)
      {
         for (int i = _segments.Count -1; i > 0; i--)
         {
            _segments[i].position = _segments[i - 1].position;
         } 
      
         float x = Mathf.Round(transform.position.x) + _direction.x;
         float y = Mathf.Round(transform.position.y) + _direction.y;

         transform.position = new Vector2(x, y);
      }
   }
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Apple"))
      {
         _audioSource.PlayOneShot(sfx_Apple);
         appleCount += 1;
         Grow();
      }

      if (other.CompareTag("Obstacle"))
      {
         _audioSource.PlayOneShot(sfx_Hit);
         _audioSource.Stop();
         Time.timeScale = 0f;
         startUI.SetActive(true);

         if (appleCount > PlayerPrefs.GetInt(key))
         {
            highScore = appleCount;
            SetPlayPrefs(sceneName, key, highScore);
            PlayerPrefs.Save();
         }
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
         ResetSnake();
      }

      #region WallTransition
      
      if (other.CompareTag("RightWall"))
      {
         transform.position = new Vector3(-26, transform.position.y,0);
         transform.eulerAngles = new Vector3 (0, 0,90);
      }

      if (other.CompareTag("LeftWall"))
      {
         transform.position = new Vector3(26, transform.position.y, 0);
         transform.eulerAngles = new Vector3 (0, 0,-90);
      }

      if (other.CompareTag("UpWall"))
      {
         transform.position = new Vector3(transform.position.x, -15, 0);
         transform.eulerAngles = new Vector3 (0, 0, 180);
      }

      if (other.CompareTag("DownWall"))
      {
         transform.position = new Vector3(transform.position.x, 15, 0);
         transform.eulerAngles = new Vector3 (0, 0,0);
      }
      #endregion

   }
   
   private void Grow()
   {
      Transform segment = Instantiate(segmentPrefab, transform.parent);
      segment.position = _segments[_segments.Count - 1].position;
      _segments.Add(segment);
   }

   private void ResetSnake()
   {
      text_highScore.text = PlayerPrefs.GetInt(key).ToString();
      appleCount = 0;
      _direction = Vector2.right;
      transform.position = Vector3.zero;
      transform.eulerAngles = new Vector3 (0, 0,90);

      for (int i = 1; i < _segments.Count; i++)
      {
         Destroy(_segments[i].gameObject);
      }
      
      _segments.Clear();
      _segments.Add(this.transform);
      _audioSource.Play();
   }
}
