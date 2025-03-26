using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to the AudioSource component
    public AudioClip backgroundMusic;  // Background music clip
    public bool isMusicOn = true; // Whether music is enabled or not

    private void Start()
    {
        // Check if we have an AudioSource component on the same GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Play music when the game starts if the music is on
        if (isMusicOn)
        {
            PlayMusic();
        }
        else
        {
            audioSource.volume = 0f; // Mute the music if it's off
        }
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;  // Set the background music clip
            audioSource.loop = true;  // Loop the music
            audioSource.Play();  // Play the music
        }
    }

    public void StopMusic()
    {
        // Mute the music by setting the volume to 0
        audioSource.volume = 0f;
    }

    public void SetMusicVolume(float volume)
    {
        // If music is on, adjust the volume
        if (isMusicOn)
        {
            audioSource.volume = volume;
        }
    }
}
