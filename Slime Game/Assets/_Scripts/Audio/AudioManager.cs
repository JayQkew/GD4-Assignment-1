using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Background Music")]
    [SerializeField] private List<AudioClip> backgroundAudioClips;
    private AudioClip backgroundMusic;
    [SerializeField] private AudioSource backgroundAudioSource;

    [Header("Lobby Music")] 
    [SerializeField] private List<AudioClip> lobbyAudioClips;
    private AudioClip lobbyMusic;
    [SerializeField] private AudioSource lobbyAudioSource;
    
    [Header("Ball Hit Music")] 
    [SerializeField] private AudioClip ballHitMusic;
    [SerializeField] private AudioSource ballHitAudioSource;
    
    [Header("Bomb Tick Music")] 
    [SerializeField] private AudioClip bombTickMusic;
    [SerializeField] private AudioSource bombTickAudioSource;
    
    [Header("Dash Splash Music")] 
    [SerializeField] private AudioClip dashSplashMusic;
    [SerializeField] private AudioSource dashSplashAudioSource;
    
    [Header("Eat Music")] 
    [SerializeField] private AudioClip eatMusic;
    [SerializeField] private AudioSource eatAudioSource;
    
    [Header("Goal Music")] 
    [SerializeField] private AudioClip goalMusic;
    [SerializeField] private AudioSource goalAudioSource;
    
    [Header("Inflate Music")] 
    [SerializeField] private AudioClip inflateMusic;
    [SerializeField] private AudioSource inflateAudioSource;
    
    [Header("Underwater Explosion Music")] 
    [SerializeField] private AudioClip underwaterExplosionMusic;
    [SerializeField] private AudioSource underwaterExplosionAudioSource;
    
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }

    }

    public void ChangeBackgroundMusic()
    {
        backgroundMusic = backgroundAudioClips[Random.Range(0, backgroundAudioClips.Count)];
        backgroundAudioSource.clip = backgroundMusic;
        backgroundAudioSource.Play();
    }
    
    public void ChangeLobbyMusic()
    {
        lobbyMusic = lobbyAudioClips[Random.Range(0, lobbyAudioClips.Count)];
        lobbyAudioSource.clip = lobbyMusic;
        lobbyAudioSource.Play();
    }
    
    public void BallHitMusic()
    {
        ballHitAudioSource.clip = ballHitMusic;
        ballHitAudioSource.Play();
    }
    
    public void BombTickMusic()
    {
        bombTickAudioSource.clip = bombTickMusic;
        bombTickAudioSource.Play();
    }
    
    public void DashSplashMusic()
    {
        dashSplashAudioSource.clip = dashSplashMusic;
        dashSplashAudioSource.Play();
    }
    
    public void EatMusic()
    {
        eatAudioSource.clip = eatMusic;
        eatAudioSource.Play();
    }
    
    public void GoalMusic()
    {
        goalAudioSource.clip = goalMusic;
        goalAudioSource.Play();
    }
    
    public void InflateMusic()
    {
        inflateAudioSource.clip = inflateMusic;
        inflateAudioSource.Play();
    }
    
    public void UnderwaterExplosionMusic()
    {
        underwaterExplosionAudioSource.clip = underwaterExplosionMusic;
        underwaterExplosionAudioSource.Play();
    }
}
