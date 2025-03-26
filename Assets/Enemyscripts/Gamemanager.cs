using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameWinPanel; // Referens till Game Win Panel
    public string homeScreenSceneName = "HomeScreen"; // Namn p� Home Screen scenen

    private void Start()
    {
        gameWinPanel.SetActive(false); // D�ljer panelen vid start
    }

    // Anropas n�r bossen d�r och GameWinPanel ska visas
    public void ShowGameWinPanel()
    {
        gameWinPanel.SetActive(true); // Visa GameWinPanel
    }

    // Funktion f�r att g� tillbaka till Home Screen
    public void GoToHomeScreen()
    {
        SceneManager.LoadScene(homeScreenSceneName); // Ladda hemmeny scenen
    }

    // Funktion f�r att starta om banan
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Ladda om den aktuella scenen
    }
}
