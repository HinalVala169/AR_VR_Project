using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgAudioSource;
    public AudioSource voiceOverAudioSource;

    [Header("Audio Clips")]
    public AudioClip[] voiceOverClips;
    public AudioClip visitAgainClip;

    public int currentClipIndex = 0;
    public CharacterPatrol character;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (bgAudioSource == null || voiceOverAudioSource == null)
        {
            Debug.LogError("AudioSources are not assigned!");
            return;
        }

        // Only play automatically if no external script is controlling playback
        PlayNextVoiceOverClip();
    }

    public void PlayNextVoiceOverClip()
    {
        // Prevent playing a new clip if one is still playing
        if (voiceOverAudioSource.isPlaying)
        {
            Debug.Log("A voice-over clip is already playing. Waiting for it to finish.");
            return;
        }

        // Ensure we are within the clip array bounds
        // if (currentClipIndex >= voiceOverClips.Length)
        // {
        //     Debug.Log("All voice-over clips have been played. Resetting.");
        //     currentClipIndex = 1;
        //     return;  // Prevents accessing an invalid index
        // }

        // Play the current clip
        AudioClip currentClip = voiceOverClips[currentClipIndex];
        voiceOverAudioSource.clip = currentClip;

        Debug.Log($"Playing voice-over clip {currentClipIndex}: {currentClip.name}");

        FadeBGMusic();
        voiceOverAudioSource.Play();

        StartCoroutine(CheckClipEnd());
    }

    IEnumerator CheckClipEnd()
    {
        while (voiceOverAudioSource.isPlaying)
        {
            yield return null;
        }

       // Debug.Log($"Voice-over clip {currentClipIndex} finished.");
     

        // Move character after the first clip finishes
        if (currentClipIndex == 0)
        {
           // Debug.Log("The intro clip has finished playing.");
            character?.MovePlayer();
        }

        // Move to the next clip, but don't go out of bounds
        if (currentClipIndex < voiceOverClips.Length - 1)
        {
            currentClipIndex++;
        }
         else
        {
            
            Debug.Log("All voice-over clips finished. Resetting.");
            currentClipIndex = 1;
            PlayNextVoiceOverClip();
        }
       
    }

    public void LoudBGMusic()
    {
        bgAudioSource.volume = 1f;
    }

    public void FadeBGMusic()
    {
        bgAudioSource.volume = 0.15f;
    }

    public bool IsVoiceOverPlaying()
    {
        return voiceOverAudioSource.isPlaying;
    }

    public bool ClipToEnd()
    {
        return !voiceOverAudioSource.isPlaying;
    }
}
