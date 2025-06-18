using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Background Music")]
    [SerializeField] private List<AudioClip> backgroundAudioClips;
    private AudioClip backgroundMusic;
    public AudioSource backgroundAudioSource;

    [Header("Lobby Music")]
    [SerializeField] private List<AudioClip> lobbyAudioClips;
    private AudioClip lobbyMusic;
    public AudioSource lobbyAudioSource;

    [Header("Ball Hit Music")]
    [SerializeField] private AudioClip ballHitMusic;
    public AudioSource ballHitAudioSource;

    [Header("Bomb Tick Music")]
    [SerializeField] private AudioClip bombTickMusic;
    public AudioSource bombTickAudioSource;

    [Header("Dash Splash Music")]
    [SerializeField] private AudioClip dashSplashMusic;
    public AudioSource dashSplashAudioSource;

    [Header("Eat Music")]
    [SerializeField] private AudioClip eatMusic;
    public AudioSource eatAudioSource;

    [Header("Goal Music")]
    [SerializeField] private AudioClip goalMusic;
    public AudioSource goalAudioSource;

    [Header("Inflate Music")]
    [SerializeField] private AudioClip inflateMusic;
    public AudioSource inflateAudioSource;

    [Header("Underwater Explosion Music")]
    [SerializeField] private AudioClip underwaterExplosionMusic;
    public AudioSource underwaterExplosionAudioSource;

    private int currentSceneIndex = -1; // Initialize to an invalid index

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe here
        }
        else
        {
            Destroy(this);
        }
        lobbyAudioSource = GetComponent<AudioSource>();
        backgroundAudioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy() // Use OnDestroy for DontDestroyOnLoad objects to unsubscribe reliably
    {
         SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only proceed if it's a new scene load (not just additive or scene being reloaded)
        if (scene.buildIndex != currentSceneIndex)
        {
            currentSceneIndex = scene.buildIndex;
            Debug.Log($"Scene loaded: {scene.name}, Index: {currentSceneIndex}. Checking for music change.");

            // Check if the loaded scene's index is one of the lobby indexes
            if (currentSceneIndex >= 0 && currentSceneIndex <= 2) // Lobby indexes are 0, 1, 2
            {
                ChangeLobbyMusic();
                // Optionally stop other music if it was playing, e.g., backgroundAudioSource.Stop();
            }
            else // Any other scene index is considered a game level
            {
                ChangeBackgroundMusic();
                // Optionally stop other music if it was playing, e.g., lobbyAudioSource.Stop();
            }
        }
        else
        {
            Debug.Log($"Scene loaded: {scene.name}, Index: {currentSceneIndex}. Music already set (same scene index).");
        }
    }

    public void ChangeBackgroundMusic()
    {
        if (backgroundAudioClips.Count > 0)
        {
            // Stop lobby music if it's playing and this is starting
            if (lobbyAudioSource.isPlaying) lobbyAudioSource.Stop();

            backgroundMusic = backgroundAudioClips[Random.Range(0, backgroundAudioClips.Count)];
            backgroundAudioSource.clip = backgroundMusic;
            backgroundAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No background music clips assigned to AudioManager!");
            backgroundAudioSource.Stop(); // Ensure it stops if no clips
        }
    }

    public void ChangeLobbyMusic()
    {
        if (lobbyAudioClips.Count > 0)
        {
            // Stop background music if it's playing and this is starting
            if (backgroundAudioSource.isPlaying) backgroundAudioSource.Stop();
            
            lobbyMusic = lobbyAudioClips[Random.Range(0, lobbyAudioClips.Count)];
            lobbyAudioSource.clip = lobbyMusic;
            lobbyAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No lobby music clips assigned to AudioManager!");
            lobbyAudioSource.Stop(); // Ensure it stops if no clips
        }
    }

    // --- Other audio methods remain the same ---
    public void BallHitMusic()
    {
        if (ballHitMusic != null) ballHitAudioSource.PlayOneShot(ballHitMusic);
        else Debug.LogWarning("Ball hit music clip is not assigned!");
    }

    public void BombTickMusic()
    {
        if (bombTickMusic != null) bombTickAudioSource.PlayOneShot(bombTickMusic);
        else Debug.LogWarning("Bomb tick music clip is not assigned!");
    }

    public void DashSplashMusic()
    {
        if (dashSplashMusic != null) dashSplashAudioSource.PlayOneShot(dashSplashMusic);
        else Debug.LogWarning("Dash splash music clip is not assigned!");
    }

    public void EatMusic()
    {
        if (eatMusic != null) eatAudioSource.PlayOneShot(eatMusic);
        else Debug.LogWarning("Eat music clip is not assigned!");
    }

    public void GoalMusic()
    {
        if (goalMusic != null) goalAudioSource.PlayOneShot(goalMusic);
        else Debug.LogWarning("Goal music clip is not assigned!");
    }

    public void InflateMusic()
    {
        if (inflateMusic != null) inflateAudioSource.PlayOneShot(inflateMusic);
        else Debug.LogWarning("Inflate music clip is not assigned!");
    }

    public void UnderwaterExplosionMusic()
    {
        if (underwaterExplosionMusic != null) underwaterExplosionAudioSource.PlayOneShot(underwaterExplosionMusic);
        else Debug.LogWarning("Underwater explosion music clip is not assigned!");
    }
}