using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // Main menu buttons
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button mapSelectButton;

    // Settings panel and UI elements
    public GameObject settingsPanel;
    public Toggle musicToggle;
    public Slider volumeSlider;

    private void Awake()
    {
        // Add listeners to buttons for main menu
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        mapSelectButton.onClick.AddListener(OpenMapSelect);

        // Initialize settings (optional)
        InitializeSettings();
    }

    // Method to start the game when Play is clicked
    public void PlayGame()
    {
        // You can load your game scene here
        LoadScene("GameScene"); // Replace "GameScene" with your gameplay scene name
    }

    // Open the settings menu (can be for music, volume, etc.)
    public void OpenSettings()
    {
        settingsPanel.SetActive(true); // Show settings panel
    }

    // Close settings panel (can be called from the "Close" button in settings)
    public void CloseSettings()
    {
        settingsPanel.SetActive(false); // Hide settings panel
    }

    // Change music toggle (on/off)
    public void ToggleMusic(bool isOn)
    {
        // Handle music toggle here (you can add your own code for enabling/disabling music)
        if (isOn)
        {
            // Enable music or play background music
        }
        else
        {
            // Disable music or stop background music
        }
    }

    // Change volume using the slider
    public void ChangeVolume(float volume)
    {
        // Adjust volume (you can link this to audio management if you later decide to have music)
    }

    // Quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Stop play mode in editor
#else
        Application.Quit();  // Quit the game if running as a build
#endif
    }

    // Load a new scene (used for transitioning to the game scene or map selection scene)
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Make sure this scene is in your Build Settings
    }

    // Open map selection scene (optional)
    private void OpenMapSelect()
    {
        LoadScene("MapSelectScene"); // Change this to your map select scene name
    }

    // Initialize settings such as volume and music state
    private void InitializeSettings()
    {
        // Set volume slider to a default value (0.5 as an example)
        volumeSlider.value = 0.5f; // Set to a default volume level, this can be saved/loaded as needed.

        // Set music toggle (if you have music on/off functionality)
        musicToggle.isOn = true; // Assume music is enabled by default
    }
}
