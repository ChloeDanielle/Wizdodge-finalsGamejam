using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject aboutPanel; // Reference to the About panel
    [Header("Audio Clips")]
    public AudioClip backgroundMusic; // Background music for the main menu

    private AudioSource audioSource;

    void Start()
    {
        // Ensure the About panel is hidden when the menu starts
        if (aboutPanel != null)
        {
            aboutPanel.SetActive(false);
        }

        // Initialize and play background music
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on MainMenuManager!");
        }
        else if (backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }

    // Called when the Game Start button is clicked
    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene("MainArena"); // Replace "MainArena" with your game's main scene name
    }

    // Called when the About button is clicked
    public void ShowAboutPanel()
    {
        if (aboutPanel != null)
        {
            aboutPanel.SetActive(true); // Show the About panel
        }
    }

    // Called when the Close/About Back button is clicked
    public void HideAboutPanel()
    {
        if (aboutPanel != null)
        {
            aboutPanel.SetActive(false); // Hide the About panel
        }
    }

    // Called when the Back to Main Menu button in the About panel is clicked
    public void BackToMainMenu()
    {
        Debug.Log("Returning to Main Menu from About panel...");
        HideAboutPanel(); // Hides the About panel to show the Main Menu
    }

    // Called when the Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void PlayBackgroundMusic()
    {
        if (audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
