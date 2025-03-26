using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the pause menu UI panel
    public GameObject pauseButton; // Reference to the pause button
    public AudioManager audioManager; // Reference to the AudioManager
    public Toggle musicToggle; // Toggle for music
    public Slider volumeSlider; // Slider for volume control

    private bool isPaused = false; // Track if the game is paused

    private void Start()
    {
        // Ensure the volume slider starts with the current volume
        if (volumeSlider != null)
        {
            volumeSlider.value = audioManager.audioSource.volume;
        }

        // Initialize music toggle based on the current state of music
        if (musicToggle != null)
        {
            musicToggle.isOn = audioManager.isMusicOn;
        }

        // Hide the pause menu initially
        pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        // Check if the player presses the "Escape" key to pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Freeze the game time (pause)
        pauseMenuPanel.SetActive(true); // Show the pause menu
        isPaused = true; // Set the game state to paused
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game time
        pauseMenuPanel.SetActive(false); // Hide the pause menu
        isPaused = false; // Set the game state to unpaused
    }

    public void ToggleMusic(bool isOn)
    {
        if (audioManager != null)
        {
            audioManager.isMusicOn = isOn; // Toggle music on/off

            if (isOn)
            {
                audioManager.PlayMusic(); // Play music if toggled on
            }
            else
            {
                audioManager.StopMusic(); // Stop music (mute) if toggled off
            }
        }
    }

    public void ChangeVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume); // Adjust the music volume
        }
    }
}
