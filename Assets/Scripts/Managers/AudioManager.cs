using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private Sound musicTest;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    void Start()
    {
        PlaySound(musicTest, gameObject);
    }

    public AudioSource PlaySound(Sound sound, GameObject sourceGameObject)
    {
        if (sound == null)
        {
            return null;
        }

        var existingAudioSources = sourceGameObject.GetComponents<AudioSource>();

        var audioSource = existingAudioSources.FirstOrDefault(source => source.isPlaying == false);
        if (audioSource == null)
        {
            audioSource = sourceGameObject.AddComponent<AudioSource>();
        }
        SetAudioSourceAttributes(audioSource, sound);
        audioSource.Play();
        return audioSource;
    }

    private void SetAudioSourceAttributes(AudioSource audioSource, Sound sound)
    {
        audioSource.clip = sound.audioClip;
        audioSource.playOnAwake = false;
        audioSource.volume = sound.defaultVolume;
        audioSource.loop = sound.loop;
    }
}
