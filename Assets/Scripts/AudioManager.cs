using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgAudioSource; 
    public AudioSource voiceOverAudioSource; 
    public CharacterPatrol  character;

    [Header("Audio Clips")]
    public AudioClip[] voiceOverClips; 

    private int currentClipIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (bgAudioSource == null || voiceOverAudioSource == null)
        {
            Debug.LogError("AudioSources are not assigned!");
            return;
        }
    }

    public void PlayNextVoiceOverClip()
    {
        if (currentClipIndex >= voiceOverClips.Length)
        {
            Debug.Log("All voice-over clips have been played.");
            return;
        }

        
        AudioClip currentClip = voiceOverClips[character.broadcastIndex];
        voiceOverAudioSource.clip = currentClip;
        FadeBGMusic();
        voiceOverAudioSource.Play();

        //StartCoroutine(WaitForClipToEnd(currentClip.length)); 
        //currentClipIndex++;
    }

    public void ClipToEnd()
    {
       // yield return new WaitForSeconds(clipLength);
        voiceOverAudioSource.Stop();
        Debug.Log("Voice-over clip has finished playing.");
        LoudBGMusic();
    }

    public void LoudBGMusic()
    {
        bgAudioSource.volume = 1f;
    }

    public void FadeBGMusic()
    {
        bgAudioSource.volume = 0.25f;
    }
}
