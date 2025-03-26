using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameWinPanel; // Referens till Game Win Panel
    public string homeScreenSceneName = "HomeScreen"; // Namn på Home Screen scenen

    private void Start()
    {
        gameWinPanel.SetActive(false); // Döljer panelen vid start
    }

    // Anropas när bossen dör och GameWinPanel ska visas
    public void ShowGameWinPanel()
    {
        gameWinPanel.SetActive(true); // Visa GameWinPanel
    }

    // Funktion för att gå tillbaka till Home Screen
    public void GoToHomeScreen()
    {
        SceneManager.LoadScene(homeScreenSceneName); // Ladda hemmeny scenen
    }

    // Funktion för att starta om banan
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Ladda om den aktuella scenen
    }
}
