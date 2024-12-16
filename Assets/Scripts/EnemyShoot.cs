using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject redSpellPrefab;    // Red spell prefab (damage)
    public GameObject greenSpellPrefab;  // Green spell prefab (heal)
    public GameObject purpleSpellPrefab; // Purple spell prefab (multi-bounce)
    public Transform player;             // Reference to the player
    public float shootInterval = 2f;     // Time between shots

    private float timer; // Timer to keep track of when to shoot

    void Start()
    {
        // Find the player object
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = shootInterval;
    }

    void Update()
    {
        // Update the timer
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ShootSpell(); // Fire a spell
            timer = shootInterval; // Reset the timer
        }
    }

    void ShootSpell()
    {
        if (player == null) return;

        // Randomly choose a spell type
        int spellType = Random.Range(0, 3); // 0 = Red, 1 = Green, 2 = Purple
        GameObject spellPrefab;

        switch (spellType)
        {
            case 0:
                spellPrefab = redSpellPrefab; // Red Fireball
                break;
            case 1:
                spellPrefab = greenSpellPrefab; // Green Heal Spell
                break;
            case 2:
                spellPrefab = purpleSpellPrefab; // Purple Multi-Bounce Spell
                break;
            default:
                spellPrefab = redSpellPrefab;
                break;
        }

        // Instantiate the spell at the enemy's position
        GameObject spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);

        // Calculate direction toward the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Add velocity to the spell
        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 5f; // Adjust speed as needed
        }
    }
}
