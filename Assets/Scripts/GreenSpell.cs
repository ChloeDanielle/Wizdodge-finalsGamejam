using UnityEngine;

public class GreenSpell : MonoBehaviour
{
    public int healAmount = 1; // Amount of health to restore to the player
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Destroy GreenSpell after 5 seconds if no collision occurs
        Destroy(gameObject, 5f);

        // Rotate to face the player at the moment it is created
        FacePlayer();
    }

    private void FacePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Heal the player
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject); // Destroy the GreenSpell after healing
            }
        }
        else if (collision.CompareTag("Environment"))
        {
            // Destroy the GreenSpell if it collides with the environment
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector2 direction, float speed)
    {
        if (rb != null)
        {
            // Set the velocity for the GreenSpell
            rb.velocity = direction.normalized * speed;
        }
    }
}
