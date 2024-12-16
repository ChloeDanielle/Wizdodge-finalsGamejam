using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar; // Reference to the health bar UI
    public int maxHealth = 5; // Maximum health
    private int currentHealth;

    private GameManager gameManager; // Reference to the GameManager

    [Header("Audio Clips")]
    public AudioClip hurtSound; // Sound when the player gets hurt
    public AudioClip healSound; // Sound when the player is healed

    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        // Get the GameManager instance
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on the Player object!");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        // Play hurt sound
        if (hurtSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        healthBar.value = currentHealth;

        // Play heal sound
        if (healSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(healSound);
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        // Trigger Game Over in the GameManager
        if (gameManager != null)
        {
            gameManager.GameOver();
        }

        // Disable player controls or other necessary components
        gameObject.SetActive(false); // Example: Disable the player GameObject
    }
}
