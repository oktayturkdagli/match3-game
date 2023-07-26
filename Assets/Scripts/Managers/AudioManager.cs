using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource blockExplosionAudio;
    [SerializeField] private AudioSource blockCollectAudio;
    
    public void PlayBlockExplosionAudio()
    {
        blockExplosionAudio.Play();
    }
    
    public void PlayBlockCollectAudio()
    {
        blockCollectAudio.Play();
    }
}