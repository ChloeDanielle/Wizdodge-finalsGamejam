using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3; // Maximum health for the enemy
    private int currentHealth;
    private Animator animator; // Reference to the Animator
    private bool isDead = false; // To prevent multiple death triggers

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // If the enemy is already dead, ignore further damage

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log(gameObject.name + " has died!");

        // Trigger the death animation
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Disable the enemy's collider and movement
        GetComponent<Collider2D>().enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop movement
            rb.isKinematic = true; // Disable physics interactions
        }

        // Destroy the enemy after the animation finishes
        float deathAnimationDuration = GetAnimationClipLength("Enemy_death");
        Destroy(gameObject, deathAnimationDuration);
    }

    private float GetAnimationClipLength(string clipName)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return 0f;

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length; // Return the length of the specified animation
            }
        }

        return 0f; // Default to 0 if no matching clip is found
    }
}
