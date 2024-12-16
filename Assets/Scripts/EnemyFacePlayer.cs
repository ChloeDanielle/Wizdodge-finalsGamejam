using UnityEngine;

public class EnemyFacePlayer : MonoBehaviour
{
    private Transform player; // Reference to the player
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    void Start()
    {
        // Find the player in the scene by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player != null)
        {
            FacePlayer();
        }
    }

    void FacePlayer()
    {
        // Calculate the direction to the player
        Vector2 direction = player.position - transform.position;

        // Flip the sprite based on horizontal direction
        if (direction.x > 0) // Player is to the right
        {
            spriteRenderer.flipX = false; // Face right
        }
        else if (direction.x < 0) // Player is to the left
        {
            spriteRenderer.flipX = true; // Face left
        }
    }
}
