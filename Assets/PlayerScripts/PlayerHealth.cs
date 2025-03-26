using UnityEngine;
using UnityEngine.UI;

namespace PlayerHealth
{
    public class Health : MonoBehaviour
    {
        public float maxHealth = 100f;
        public float CurrentHealth;

        public Image healthBar;
        private Transform respawnPoint;  // The current checkpoint respawn point

        public static event System.Action OnPlayerDeath;

        private void Start()
        {
            CurrentHealth = maxHealth;

            if (healthBar == null)
            {
                Debug.LogError("Health bar is not assigned!");
            }

            if (respawnPoint == null)
            {
                Debug.LogError("Respawn point is not assigned!");
            }
        }

        private void Update()
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = Mathf.Clamp(CurrentHealth / maxHealth, 0f, 1f);
            }
        }

        public void TakeDamage(float damageAmount)
        {
            CurrentHealth -= damageAmount;
            Debug.Log("Player took damage: " + damageAmount);

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
        }

        private void Die()
        {
            // Trigger the death event
            TriggerPlayerDeathEvent();

            // Reset health and respawn at the most recent checkpoint if available
            CurrentHealth = maxHealth;

            if (respawnPoint != null)
            {
                transform.position = respawnPoint.position;  // Respawn player at the checkpoint
            }
            else
            {
                Debug.Log("No respawn point set. Implement respawn logic.");
            }
        }

        private void TriggerPlayerDeathEvent()
        {
            OnPlayerDeath?.Invoke();  // Fire the event if there are any listeners
        }

        // Method to update the checkpoint when the player reaches it
        public void SetCheckpoint(Transform newCheckpoint)
        {
            respawnPoint = newCheckpoint;  // Update the respawn point to the new checkpoint
            Debug.Log("New checkpoint set: " + newCheckpoint.name);
        }
    }
}
