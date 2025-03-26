using PlayerHealth;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;  // The position the player will respawn at when they die
    public float radius = 5f;  // Radius to trigger the checkpoint activation

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Check if the player is within the checkpoint's radius
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance <= radius)
            {
                // Set the checkpoint as the new respawn point
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.SetCheckpoint(respawnPoint);  // Update the respawn point
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the checkpoint's radius in the Scene view for debugging
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
