using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource cubeExplosionAudio;
    [SerializeField] private AudioSource cubeCollectAudio;
    
    public void PlayCubeExplosionAudio()
    {
        cubeExplosionAudio.Play();
    }
    
    public void PlayCubeCollectAudio()
    {
        cubeCollectAudio.Play();
    }
}