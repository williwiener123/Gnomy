using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject deathEffect;
    public GameObject GetHit;
    public BossHealthBar bossHealthBar; // Referens till hälsobaren (endast för bossen)
    public bool isBoss = false; // Markera om detta är en boss
    public GameManager gameManager; // Referens till GameManager för att visa GameWinPanel

    private void Start()
    {
        if (isBoss && bossHealthBar != null)
        {
            bossHealthBar.SetMaxHealth(health); // Sätter hälsobaren till max HP
            bossHealthBar.ShowHealthBar(false); // Döljer hälsobaren vid start
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            if (isBoss && bossHealthBar != null)
            {
                bossHealthBar.ShowHealthBar(false); // Dölj hälsobaren när bossen dör
            }

            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);

            if (isBoss)
            {
                gameManager.ShowGameWinPanel(); // Visa Game Win Panel när bossen dör
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Instantiate(GetHit, transform.position, Quaternion.identity);
        health -= damage;

        if (isBoss && bossHealthBar != null)
        {
            bossHealthBar.SetHealth(health); // Uppdatera hälsobaren
        }
    }
}
