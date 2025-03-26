using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 200; // Max-hälsa för bossen
    private int currentHealth;
    public GameObject deathEffect;
    public GameObject GetHit;
    public BossHealthBar healthBar; // Referens till hälsobaren

    private void Start()
    {
        currentHealth = maxHealth; // Sätter bossens start-hälsa till max
        healthBar.SetMaxHealth(maxHealth); // Uppdaterar hälsobaren till max
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Instantiate(GetHit, transform.position, Quaternion.identity);
        healthBar.SetHealth(currentHealth); // Uppdaterar hälsobaren

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject); // Förstör bossen när den dör
    }
}
