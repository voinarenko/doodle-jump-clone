using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public Transform player;
  public GameObject hud;
  public GameObject gameOverPanel;
  public TextMeshProUGUI scoreText;
  public TextMeshProUGUI resultText;
  public Button restartButton;

  private const float FallDownOffset = 10f;

  private int HighestPoint
  {
    get => _highestPoint;
    set
    {
      if (_highestPoint == value) 
        return;
      
      _highestPoint = value;
      AudioManager.Instance.PlayScore();
    }
  }

  private Camera _camera;
  private int _highestPoint;

  private void Start()
  {
    _camera = Camera.main;
    restartButton.onClick.AddListener(RestartGame);
    DisplayScore();
  }

  private void Update()
  {
    if (!player) 
      return;

    if (player.position.y > HighestPoint)
    {
      HighestPoint = Mathf.FloorToInt(player.position.y);
      DisplayScore();
    }

    if (player.position.y < _camera.transform.position.y - FallDownOffset) 
      GameOver();
  }

  private void OnDestroy() =>
    restartButton.onClick.RemoveListener(RestartGame);

  private void DisplayScore()
  {
    scoreText.text = $"{HighestPoint}";
    resultText.text = $"{HighestPoint}";
  }

  private void GameOver()
  {
    AudioManager.Instance.PlayGameOver();
    Destroy(player.gameObject);
    hud.SetActive(false);
    gameOverPanel.SetActive(true);
  }

  private static void RestartGame() =>
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}