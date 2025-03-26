using UnityEngine;
using TMPro;  // For TextMeshPro components
using UnityEngine.SceneManagement;  // For scene management

public class LifeCounter : MonoBehaviour
{
    public TextMeshProUGUI lifeCounterText;  // Reference to the TextMeshPro UI element
    private int lives = 3;                   // Starting number of lives

    private void OnEnable()
    {
        PlayerHealth.Health.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.Health.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        lives--;  // Decrease lives
        Debug.Log("Lives remaining: " + lives);  // Debug the lives count
        UpdateLifeCounterText();

        // Check if lives are zero, and restart the scene if so
        if (lives <= 0)
        {
            RestartScene();
        }
    }

    private void UpdateLifeCounterText()
    {
        if (lifeCounterText != null)
        {
            lifeCounterText.text = lives.ToString();  // Display only the number of lives
        }
    }

    private void RestartScene()
    {
        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

