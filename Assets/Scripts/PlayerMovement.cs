using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Player movement speed
    public float catchRadius = 2f; // Radius for catching spells

    private Rigidbody2D rb;
    private Vector2 movement;

    private SpriteRenderer spriteRenderer;

    // Sprites for different directions
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    private GameObject heldSpell = null; // Spell currently held by the player
    private bool isCatching = false; // Whether the player is holding the button to catch

    [Header("Audio Clips")]
    public AudioClip throwSound; // Sound for throwing a spell
    public AudioClip catchSound; // Sound for catching a spell

    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on the Player object!");
        }
    }

    void Update()
    {
        // Movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;
        UpdateSpriteDirection();
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    void UpdateSpriteDirection()
    {
        if (movement.x > 0) spriteRenderer.sprite = spriteRight;
        else if (movement.x < 0) spriteRenderer.sprite = spriteLeft;
        else if (movement.y > 0) spriteRenderer.sprite = spriteUp;
        else if (movement.y < 0) spriteRenderer.sprite = spriteDown;
    }

    public void OnCatchThrowButtonPressed()
    {
        isCatching = true;
        TryCatchSpell();
    }

    public void OnCatchThrowButtonReleased()
    {
        isCatching = false;
        ThrowSpell();
    }

    private void TryCatchSpell()
    {
        if (heldSpell != null)
        {
            Debug.Log("Already holding a spell. Throw it first.");
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, catchRadius);
        foreach (var collider in colliders)
        {
            // Ignore PurpleFlame
            if (collider.CompareTag("PurpleFlame"))
            {
                Debug.Log("PurpleFlame cannot be caught.");
                continue;
            }

            // Catch Fireball
            if (collider.CompareTag("Fireball"))
            {
                heldSpell = collider.gameObject;
                heldSpell.transform.SetParent(transform);
                heldSpell.transform.localPosition = Vector3.zero;
                Rigidbody2D spellRb = heldSpell.GetComponent<Rigidbody2D>();
                spellRb.velocity = Vector2.zero;
                spellRb.isKinematic = true;

                // Play catch sound
                if (catchSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(catchSound);
                }

                Debug.Log("Caught a Fireball!");
                return;
            }

            // Heal with GreenSpell
            if (collider.CompareTag("GreenSpell"))
            {
                PlayerHealth playerHealth = GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(1);
                    Destroy(collider.gameObject); // Destroy the GreenSpell after healing
                }

                // Play catch sound for GreenSpell
                if (catchSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(catchSound);
                }

                Debug.Log("Caught and healed with GreenSpell!");
                return;
            }
        }

        Debug.Log("No spell within catch radius!");
    }

    private void ThrowSpell()
    {
        if (heldSpell == null)
        {
            Debug.Log("No spell to throw!");
            return;
        }

        GameObject targetEnemy = FindNearestEnemy();
        if (targetEnemy == null)
        {
            Debug.Log("No enemy to target!");
            return;
        }

        // Throw the held spell at the target
        Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;
        Rigidbody2D spellRb = heldSpell.GetComponent<Rigidbody2D>();
        spellRb.isKinematic = false;
        heldSpell.GetComponent<ProjectileBehavior>().InitializeProjectile(direction, true, targetEnemy.transform);

        // Play throw sound
        if (throwSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(throwSound);
        }
        else
        {
            Debug.LogWarning("Throw sound is missing or AudioSource is not assigned!");
        }

        Debug.Log($"Threw a {heldSpell.tag}!");

        // Detach and clear the held spell
        heldSpell.transform.SetParent(null);
        heldSpell = null;
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).FirstOrDefault();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
    }
}
