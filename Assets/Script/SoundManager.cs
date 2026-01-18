using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("오디오 소스")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayBgm(AudioClip clip)
    {
        if (bgmSource == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        if(clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
