using UnityEngine;
using System.Collections;

/*
 * SFX found here : http://opengameart.org/content/3-ping-pong-sounds-8-bit-style 
 */

/// <summary>
/// Simple sound manager as a singleton. Handles the sfx
/// </summary>
public class SoundManager : MonoBehaviour
{
    static SoundManager _instance = null;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _sfxWallBounce;

    [SerializeField]
    AudioClip _sfxRacketBounce;

    [SerializeField]
    AudioClip _sfxGoal;

    public static SoundManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    void Awake()
    {
        // Singleton
        if(!_instance)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // No audio source found handled
        if(!_audioSource)
        {
            Debug.LogError("Audio Manager was unable to found an audio source. Please double check properties in Editor");
        }
    }

    /// <summary>
    /// Mute sound
    /// </summary>
    public void Mute()
    {
        if(_audioSource)
        {
            _audioSource.volume = 0f;
        }
    }

    /// <summary>
    /// Unmute sound
    /// </summary>
    public void UnMunte()
    {
        if(_audioSource)
        {
            _audioSource.volume = 1f;
        }
    }

    public void PlayGoalSFX()
    {
        PlayOneShot(_sfxGoal);
    }

    public void PlayWallBounceSFX()
    {
        PlayOneShot(_sfxWallBounce);
    }

    public void PlayRacketBounceSFX()
    {
        PlayOneShot(_sfxRacketBounce);
    }

    /// <summary>
    /// Play an audio clip if the sound manager was found.
    /// </summary>
    /// <param name="clip">the clip to play</param>
    void PlayOneShot(AudioClip clip)
    {
        if(_audioSource)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}
