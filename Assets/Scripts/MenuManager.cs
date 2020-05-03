using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _difficultyButtons;

    [SerializeField]
    private Button _startButton;

    [SerializeField]
    private Button _exitButton;

    private GameDifficulty _gameDifficulty;

    private void Start()
    {
        _gameDifficulty = GameDifficulty.Instance;
    }

    // If the Start button is selected, display the difficulties
    public void StartGame()
    {
        _difficultyButtons.SetActive(true);
        _startButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
    }

    // If the Exit button is selected, the application is closed
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetDifficulty(string difficulty)
    {
        _gameDifficulty.Difficulty = difficulty;

        // Load the Game scene
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}
