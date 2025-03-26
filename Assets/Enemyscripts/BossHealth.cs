using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 200; // Max-h�lsa f�r bossen
    private int currentHealth;
    public GameObject deathEffect;
    public GameObject GetHit;
    public BossHealthBar healthBar; // Referens till h�lsobaren

    private void Start()
    {
        currentHealth = maxHealth; // S�tter bossens start-h�lsa till max
        healthBar.SetMaxHealth(maxHealth); // Uppdaterar h�lsobaren till max
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Instantiate(GetHit, transform.position, Quaternion.identity);
        healthBar.SetHealth(currentHealth); // Uppdaterar h�lsobaren

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject); // F�rst�r bossen n�r den d�r
    }
}
