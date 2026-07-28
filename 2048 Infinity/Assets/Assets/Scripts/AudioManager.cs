using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip tileSpawn;
    [SerializeField] private AudioClip tileMove;
    [SerializeField] private AudioClip tileMerge;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip victory;

    private void Awake()
    {
        // Ensure there is only one AudioManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    #region Music
    public void PlayMusic()
    {
        if (backgroundMusic == null)
            return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    #endregion

    #region Sound Effects
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }

    public void PlayTileSpawn()
    {
        PlaySFX(tileSpawn);
    }

    public void PlayTileMove()
    {
        PlaySFX(tileMove);
    }

    public void PlayTileMerge()
    {
        PlaySFX(tileMerge);
    }

    public void PlayGameOver()
    {
        PlaySFX(gameOver);
    }

    #endregion
}