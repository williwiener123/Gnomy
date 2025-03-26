using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float offset;
    public GameObject projectile;   // Bullet prefab
    public GameObject shotEffect;   // Muzzle flash effect
    public Transform shotPoint;     // Where the bullet spawns
    public float startTimeBtwShots; // Time between shots
    private float timeBtwShots;     // Time between shots

    public float knockbackForce = 5f;  // Adjustable knockback strength
    private Rigidbody2D playerRb;      // Player's Rigidbody2D

    // Audio variables
    public AudioClip shootSound; // Sound for shooting
    public AudioClip bulletHitSound; // Sound for bullet hit or destroy
    private AudioSource audioSource; // AudioSource component for playing sounds

    private void Start()
    {
        playerRb = GetComponentInParent<Rigidbody2D>(); // Get player's Rigidbody2D
        audioSource = GetComponent<AudioSource>(); // Get AudioSource component on the weapon object

        if (playerRb == null)
        {
            Debug.LogError("Weapon: No Rigidbody2D found on player!");
        }

        if (audioSource == null)
        {
            Debug.LogError("Weapon: No AudioSource component found on weapon!");
        }
    }

    private void Update()
    {
        // Get direction towards the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Rotate weapon to face the mouse
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0)) // Left mouse button
            {
                Shoot(direction);  // Pass shooting direction
                timeBtwShots = startTimeBtwShots;  // Reset shot cooldown
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime; // Reduce the time until the next shot
        }
    }

    private void Shoot(Vector3 shootDirection)
    {
        // Spawn muzzle effect
        if (shotEffect != null)
        {
            Instantiate(shotEffect, shotPoint.position, Quaternion.identity);
        }

        // Spawn bullet
        if (projectile != null && shotPoint != null)
        {
            GameObject bullet = Instantiate(projectile, shotPoint.position, Quaternion.identity);
            Projectile bulletScript = bullet.GetComponent<Projectile>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(shootDirection); // Pass correct direction
            }

            // Play shooting sound
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound); // Play the shooting sound
            }
        }
        else
        {
            Debug.LogError("Weapon: Missing projectile or shotPoint!");
        }

        ApplyKnockback(shootDirection);
    }

    private void ApplyKnockback(Vector3 shootDirection)
    {
        if (playerRb != null)
        {
            Vector2 knockbackDirection = (-shootDirection).normalized; // Knockback in the opposite direction
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    // Call this method when a bullet hits something or is destroyed
    public void PlayBulletHitSound()
    {
        if (bulletHitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bulletHitSound); // Play bullet hit sound
        }
    }
}
