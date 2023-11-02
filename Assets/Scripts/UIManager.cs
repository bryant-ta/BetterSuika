using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI _scoreText;

    [SerializeField] TextMeshProUGUI _firstPlaceScoreText;
    [SerializeField] TextMeshProUGUI _secondPlaceScoreText;
    [SerializeField] TextMeshProUGUI _thirdPlaceScoreText;

    [SerializeField] GameObject _pauseMenuPanel;
    [SerializeField] GameObject _settingsPanel;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    public void UpdateScoreText(int val) { _scoreText.text = val.ToString(); }

    public void UpdateHighScoresText(List<int> highScores) {
        if (highScores.Count != 3) Debug.LogError("High scores list does not have 3 elements.");

        _firstPlaceScoreText.text = highScores[0].ToString();
        _secondPlaceScoreText.text = highScores[1].ToString();
        _thirdPlaceScoreText.text = highScores[2].ToString();
    }

    // returns active state of pause menu
    public void TogglePauseMenu() {
        _pauseMenuPanel.SetActive(!_pauseMenuPanel.activeSelf);
        GameManager.Instance.Player.CanInput = !_pauseMenuPanel.activeSelf;
    }
    
    public void OnReturnButtonClicked() {
        TogglePauseMenu();
    }
    public void OnRestartButtonClicked() {
        _pauseMenuPanel.SetActive(false);
        GameManager.Instance.ResetGame();
    }
    public void OnSettingsButtonClicked() {
        _pauseMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }
    public void OnBackButtonClicked() { // for now ok... until more menus or sup
        _settingsPanel.SetActive(false);
        _pauseMenuPanel.SetActive(true);
    }

    public void OnExitButtonClicked() { Application.Quit(); }
}