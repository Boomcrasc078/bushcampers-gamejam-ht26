using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundDefinition", menuName = "Audio/SoundDefinition", order = 0)]
public class Sound : ScriptableObject
{
    public AudioClip audioClip;
    [Range(0f, 1f)]
    public float defaultVolume = 1f;
    public bool loop = false;
}