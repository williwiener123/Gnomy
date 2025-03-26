using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;       // Speed of the bullet
    public float lifeTime = 2f;     // Bullet lifespan before self-destruct
    public int damage = 1;          // Damage to enemies
    public LayerMask whatIsSolid;   // Layers that can stop the bullet
    public GameObject destroyEffect;

    private Vector2 moveDirection;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime); // Destroy after lifeTime seconds
    }

    private void Update()
    {
        if (moveDirection != Vector2.zero)
        {
            transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, moveDirection, 0.1f, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            DestroyProjectile();
        }
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    void DestroyProjectile()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
