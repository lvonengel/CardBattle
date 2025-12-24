using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the main menu buttons and UI.
/// </summary>
public class MainMenu : MonoBehaviour {
    
    [SerializeField] private Button playButton, quitGameButton;
    private const string BATTLE_SCENE = "Battle";

    private void Awake() {
        playButton.onClick.AddListener(() => {
            StartGame();
        });
        quitGameButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void StartGame() {
        SceneManager.LoadScene(BATTLE_SCENE);
    }

}
