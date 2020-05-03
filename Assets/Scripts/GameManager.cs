using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Properties with a private set, meaning it can only be set within GameManager class but accessed anywhere
    public bool IsGameActive { get; private set; } = true;
    public bool IsGamePaused { get; private set; }

    // Properties with only get implemented, meaning its value cannot be assigned outside of this class
    public float Health => _playerHealth.Health;

    [SerializeField]
    private TextMeshProUGUI _gameOverText;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private TextMeshProUGUI _pausedText;

    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private Button _resumeButton;

    private HealthManagement _playerHealth;
    private int _totalScore;

    private void Start()
    {
        _playerHealth = GameObject.Find("Player").GetComponent<HealthManagement>();
    }

    // Game is over - enable appropriate UI and stop the game
    public void EndGame()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        IsGameActive = false;

        StartCoroutine(PauseGameCoroutine());
    }

    IEnumerator PauseGameCoroutine()
    {
        yield return new WaitForSeconds(3);

        // This pauses the game
        Time.timeScale = 0;
    }

    // If the Restart button is selected, the scene is reloaded
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    // If player presses "Esc", the game is paused
    public void PauseGame()
    {
        // This pauses the game
        Time.timeScale = 0;

        _pausedText.gameObject.SetActive(true);
        _resumeButton.gameObject.SetActive(true);

        IsGamePaused = true;
    }

    // If the Resume button is selected, the game is unpaused
    public void ResumeGame()
    {
        //This unpauses the game
        Time.timeScale = 1;

        _pausedText.gameObject.SetActive(false);
        _resumeButton.gameObject.SetActive(false);

        IsGamePaused = false;
    }

    // Updates how many lives are left on the UI
    public void UpdateHealth(int healthToAdd)
    {
        // Only add health if not already at max and don't do anything if health is 0
        if ((_playerHealth.Health != _playerHealth.MaxHealth || healthToAdd < 0) && _playerHealth.Health != 0)
        {
            // We either need to add or minus lives
            _playerHealth.Health += healthToAdd;

            // If there are no lives left, the game is over
            if (_playerHealth.Health == 0)
            {
                EndGame();
            }
        }
    }

    // Update the total score on the UI
    public void UpdateTotalScore(int scoreToAdd)
    {
        _totalScore += scoreToAdd;

        _scoreText.text = $"Score: {_totalScore}";
    }
}
