using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public static float baseSpeed = 5f; // Base speed for projectiles, modified dynamically
    public int damage = 1; // Damage dealt by the projectile
    public bool isPlayerProjectile = false; // Whether the projectile is thrown by the player
    private Transform target; // The target for the projectile
    public bool canFollowTarget = false; // Determines if the projectile follows its target

    [Header("Particle Effects")]
    public GameObject hitPlayerEffect; // Particle effect when hitting the player
    public GameObject hitEnemyEffect;  // Particle effect when hitting an enemy
    public float particleLifetime = 1f; // Time in seconds before the particle effect is destroyed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Destroy projectile after 5 seconds if no collision occurs
        Destroy(gameObject, 5f);

        // Determine behavior based on tag
        AssignBehavior();

        // If there’s a target, set the initial direction
        if (target != null)
        {
            SetDirectionToTarget(target);
        }
    }

    private void AssignBehavior()
    {
        if (!isPlayerProjectile)
        {
            if (gameObject.CompareTag("Fireball"))
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                    canFollowTarget = true; // Fireball follows the player
                }
            }
            else if (gameObject.CompareTag("PurpleFlame"))
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                    canFollowTarget = false; // PurpleFlame does not follow, but faces the player
                    SetDirectionToTarget(player.transform);
                }
            }
        }
    }

    public void InitializeProjectile(Vector2 direction, bool isFromPlayer, Transform newTarget = null)
    {
        isPlayerProjectile = isFromPlayer;

        // Set target if provided
        if (newTarget != null)
        {
            target = newTarget;

            if (canFollowTarget)
            {
                SetDirectionToTarget(target);
            }
        }
        else
        {
            // Set velocity for non-following projectiles
            rb.velocity = direction * baseSpeed;
            UpdateRotation(direction);
        }
    }

    private void Update()
    {
        // Continuously adjust direction if the projectile can follow the target
        if (target != null && canFollowTarget)
        {
            SetDirectionToTarget(target);
        }
    }

    private void SetDirectionToTarget(Transform newTarget)
    {
        Vector2 direction = (newTarget.position - transform.position).normalized;
        rb.velocity = direction * baseSpeed;
        UpdateRotation(direction);
    }

    private void UpdateRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerProjectile)
        {
            // Damage the player for enemy projectiles
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Spawn particle effect
            if (hitPlayerEffect != null)
            {
                GameObject particle = Instantiate(hitPlayerEffect, transform.position, Quaternion.identity);
                Destroy(particle, particleLifetime); // Destroy the particle effect after its lifetime
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy") && isPlayerProjectile)
        {
            // Damage the enemy for player projectiles
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Spawn particle effect
            if (hitEnemyEffect != null)
            {
                GameObject particle = Instantiate(hitEnemyEffect, transform.position, Quaternion.identity);
                Destroy(particle, particleLifetime); // Destroy the particle effect after its lifetime
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Environment"))
        {
            // Destroy projectile upon hitting the environment
            Destroy(gameObject);
        }
    }
}
