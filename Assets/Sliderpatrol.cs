using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;  // First patrol point
    public Transform pointB;  // Second patrol point
    public float speed = 2f;  // Movement speed
    private Transform target; // Current target waypoint
    private SpriteRenderer spriteRenderer; // Reference to sprite renderer

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Waypoints are not assigned!");
            return;
        }

        target = pointA; // Start moving to pointA
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (pointA == null || pointB == null) return; // Stop if waypoints are missing
        Patrol();
    }

    void Patrol()
    {
        Debug.Log("Enemy moving to: " + target.name); // Check which point it's moving to

        // Move towards the target
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Check if the enemy reached the waypoint
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            Debug.Log("Reached: " + target.name);
            target = target == pointA ? pointB : pointA; // Switch target
            FlipSprite(); // Flip sprite direction
        }
    }

    void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
