using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Timer UI
    public GameObject pauseMenu; // Pause menu UI
    public GameObject gameOverPanel; // Game Over UI
    public TextMeshProUGUI survivalTimeText; // Text for survival time on Game Over
    public TextMeshProUGUI longestTimeText; // Text for longest time on Game Over

    private float survivalTime = 0f; // Timer in seconds
    private bool isGameOver = false; // Whether the game is over
    private bool isPaused = false; // Whether the game is paused
    private int difficultyLevel = 0; // Tracks difficulty increases
    private float nextDifficultyIncreaseTime = 180f; // Next difficulty increase at 3 minutes

    public static float globalProjectileSpeedIncrease = 1f; // Base increase for projectile speed
    public static float globalSpawnRateDecrease = 0.5f; // Base decrease for spawn rate

    [Header("Audio Clips")]
    public AudioClip backgroundMusic; // Gameplay music
    public AudioClip gameOverMusic; // Music on player death
    public AudioClip toggleSound; // Sound when pause menu is toggled
    public AudioClip buttonClickSound; // Sound for restart and menu buttons

    private AudioSource audioSource;

    void Start()
    {
        Time.timeScale = 1; // Ensure the game starts unpaused
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);

        // Audio setup
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on GameManager!");
        }
        PlayBackgroundMusic(backgroundMusic);
    }

    void Update()
    {
        if (isGameOver) return;

        // Update timer
        if (!isPaused)
        {
            survivalTime += Time.deltaTime;
            UpdateTimerDisplay();

            // Increase difficulty every 3 minutes
            if (survivalTime >= nextDifficultyIncreaseTime)
            {
                IncreaseDifficulty();
                nextDifficultyIncreaseTime += 180f; // Schedule next increase
            }
        }

        // Toggle pause menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(survivalTime / 60f);
        int seconds = Mathf.FloorToInt(survivalTime % 60f);
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    private void IncreaseDifficulty()
    {
        difficultyLevel++;
        ProjectileBehavior.baseSpeed += globalProjectileSpeedIncrease;
        EnemySpawner.AdjustSpawnInterval(difficultyLevel);
    }

    public int GetDifficultyLevel()
    {
        return difficultyLevel;
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        // Show survival time
        int minutes = Mathf.FloorToInt(survivalTime / 60f);
        int seconds = Mathf.FloorToInt(survivalTime % 60f);
        survivalTimeText.text = $"Survival Time: {minutes:D2}:{seconds:D2}";

        // Save and show longest survival time
        float longestTime = PlayerPrefs.GetFloat("LongestSurvivalTime", 0f);
        if (survivalTime > longestTime)
        {
            PlayerPrefs.SetFloat("LongestSurvivalTime", survivalTime);
            longestTime = survivalTime;
        }
        int longestMinutes = Mathf.FloorToInt(longestTime / 60f);
        int longestSeconds = Mathf.FloorToInt(longestTime % 60f);
        longestTimeText.text = $"Longest Time: {longestMinutes:D2}:{longestSeconds:D2}";

        // Display Game Over panel
        gameOverPanel.SetActive(true);

        // Play game over music
        PlayBackgroundMusic(gameOverMusic);
    }

    public void RestartGame()
    {
        PlayButtonClickSound();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        PlayButtonClickSound();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        // Play toggle sound if available
        if (toggleSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(toggleSound);
        }
        else
        {
            Debug.LogWarning("Toggle sound is missing or AudioSource is not assigned!");
        }
    }

    private void PlayBackgroundMusic(AudioClip music)
    {
        if (audioSource != null && music != null)
        {
            audioSource.clip = music;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (music == null)
        {
            Debug.LogWarning("Background music clip is not assigned!");
        }
    }

    private void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("Button click sound is missing or AudioSource is not assigned!");
        }
    }
}
