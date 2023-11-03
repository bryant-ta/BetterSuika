using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] GameObject _mainMenuSettingsPanel;
    
    public void OnStartGameButtonClicked() {
        SceneManager.LoadScene("MainScene");
    }
    public void OnSettingsButtonClicked() {
        _mainMenuSettingsPanel.SetActive(!_mainMenuSettingsPanel.activeSelf);
    }

    public void OnExitButtonClicked() { Application.Quit(); }
}