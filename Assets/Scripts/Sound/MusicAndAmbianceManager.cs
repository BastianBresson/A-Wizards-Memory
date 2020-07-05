using UnityEngine;

public class MusicAndAmbianceManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource = default;
    [SerializeField] private AudioSource ambianceAudioSource = default;

    [SerializeField] private AudioClip musicClip = default;
    [SerializeField] private AudioClip ambianceClip = default; 

    

    private void Awake()
    {
        musicAudioSource.loop = true;
        ambianceAudioSource.loop = true;

        musicAudioSource.clip = musicClip;
        ambianceAudioSource.clip = ambianceClip;
    }


    private void Start()
    {

        Play(musicAudioSource);
        Play(ambianceAudioSource);
    }


    public void Play(AudioSource audioSource)
    {
        if (audioSource == null || audioSource.clip == null) return;
        audioSource.Play();
    }

    public void StopPlaying(string name)
    {

    }
}
