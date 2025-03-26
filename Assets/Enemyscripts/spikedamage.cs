using UnityEngine;
using PlayerHealth;  // Make sure to reference the namespace if using Health script

public class DamageObject : MonoBehaviour
{
    public float damageAmount = 10;  // Amount of damage this object will deal (optional, if you want to deal damage before death)

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Health component of the player
            PlayerHealth.Health playerHealth = collision.gameObject.GetComponent<PlayerHealth.Health>();

            // If the player has a Health component, apply damage and kill the player
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerHealth.CurrentHealth);  // Apply enough damage to kill the player
                Debug.Log("Player has died!");
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on the player!");
            }

            // Optional: Add any effects like death animations or sounds
            // Destroy(gameObject);  // You can optionally destroy this object if needed
        }
    }
}
