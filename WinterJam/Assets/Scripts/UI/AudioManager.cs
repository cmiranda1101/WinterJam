using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] playerHurt;
    [SerializeField] private AudioClip[] playerMelee;
    [SerializeField] private AudioClip[] enemyHurtMale;
    [SerializeField] private AudioClip[] EnemyHurtFemale;
    [SerializeField] private AudioClip[] snowballHits;
    [SerializeField] private AudioClip[] playerMovement;
    [SerializeField] private AudioClip[] enemyMovement;
    [SerializeField] private AudioClip[] playerAcrobatics;
    [SerializeField] private AudioClip[] Freezing;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip weaponSwap;
    [SerializeField] private AudioClip weaponPickup;
    [SerializeField] private AudioClip menuSound;

    [SerializeField] private AudioSource musicSource;

    public float musicVolume { get; private set; }
    public float sfxVolume { get; private set; }
    public float inMenuVolume {  get; private set; }

    private void Start()
    {
        GameManager.instance.RegisterAudioManager(this);

        musicVolume = 0.5f;
        inMenuVolume = musicVolume * 0.5f;
        sfxVolume = 0.5f;
    }

    // Music
    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetMenuMusic(bool inMenu)
    {
        musicSource.volume = inMenu ? inMenuVolume : musicVolume;
    }

    // SFX
    public void PlayHurt(AudioSource source)
    {
        if (source.gameObject.CompareTag("Player"))
        {
            PlayRandom(source, playerHurt);
        }
        else
        {
            GenderMarker marker = source.GetComponent<GenderMarker>();
            if (marker != null) return;
            AudioClip[] clips = marker.male ? enemyHurtMale : EnemyHurtFemale;
            PlayRandom(source, clips);
        }
    }

    public void PlayMelee(AudioSource source)
    {
        PlayRandom(source, playerMelee);
    }

    public void PlayShoot(AudioSource source)
    {
        PlayRandom(source, snowballHits);
    }

    public void PlayMovement(AudioSource source)
    {
        AudioClip[] clips = source.gameObject.CompareTag("Player") ? playerMovement : enemyMovement;
        PlayRandom(source, clips);
    }

    public void PlayAcrobatics(AudioSource source)
    {
        PlayRandom(source, playerAcrobatics);
    }

    public void PlayFreezing(AudioSource source)
    {
        PlayRandom(source, Freezing);
    }

    public void PlayWeaponSwap(AudioSource source)
    {
        source.PlayOneShot(weaponSwap, sfxVolume);
    }

    public void PlayWeaponPickup(AudioSource source)
    {
        source.PlayOneShot(weaponPickup, sfxVolume);
    }

    public void PlayMenuClick(AudioSource source)
    {
        source.PlayOneShot(menuSound, sfxVolume);
    }

    // Helpers
    private void PlayRandom(AudioSource source, AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0 || source == null) return;
        source.PlayOneShot(clips[Random.Range(0, clips.Length)], sfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }
}
