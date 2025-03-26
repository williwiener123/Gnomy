using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image healthBarFill; // Bilden som fylls
    public GameObject healthBarUI; // UI-objektet att aktivera/inaktivera

    private float maxHealth;

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        healthBarFill.fillAmount = 1f; // Full hälsa från start
    }

    public void SetHealth(float currentHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth; // Uppdaterar baren
    }

    public void ShowHealthBar(bool show)
    {
        healthBarUI.SetActive(show);
    }
}
