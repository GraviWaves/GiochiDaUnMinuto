using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    #region SINGLETON

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("AudioManager");
                instance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        bgmSource = InstantiateAudioSource();
        seSource = InstantiateAudioSource();

        BgmVolume = 1f;
        SeVolume = 1f;
    }

    #endregion

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    
    public float BgmVolume { get; private set; }

    public float SeVolume { get; private set; }

    void Start()
    {
        if(bgmSource == null)
        {
            bgmSource = gameObject.GetComponents<AudioSource>()[0];
        }

        if (seSource == null)
        {
            seSource = gameObject.GetComponents<AudioSource>()[1];
        }

        bgmSource.volume = Mathf.Clamp(BgmVolume, 0, 1);
        seSource.volume = Mathf.Clamp(SeVolume, 0, 1);
    }



    public void SetBgmVolume(float volume)
    {
        BgmVolume = Mathf.Clamp(volume, 0, 1);
        bgmSource.volume = BgmVolume;
    }

    public void FadeBgmVolume(float targetVolume, float fadeSpeed)
    {
        StartCoroutine(Fade(bgmSource, targetVolume, fadeSpeed));
    }

    public void SetSeVolume(float volume)
    {
        SeVolume = Mathf.Clamp(volume, 0, 1);
        seSource.volume = SeVolume;
    }

    public void FadeSeVolume(float targetVolume, float fadeSpeed)
    {
        StartCoroutine(Fade(seSource, targetVolume, fadeSpeed));
    }

    private IEnumerator Fade(AudioSource source, float targetVolume, float fadeTime)
    {
        float timer = 0f;
        while(timer < fadeTime)
        {
            float progress = timer / fadeTime;
            source.volume = Mathf.Lerp(source.volume, targetVolume, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        source.Stop();
    }


    public void PlayBgmOneShot(AudioClip clip)
    {
        bgmSource.volume = BgmVolume;
        bgmSource.PlayOneShot(clip);
    }

    public void PlayBgm(AudioClip clip, bool loop = false)
    {
        bgmSource.volume = BgmVolume;
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlaySeOneShot(AudioClip clip)
    {
        seSource.volume = SeVolume;
        seSource.PlayOneShot(clip);
    }


    private AudioSource InstantiateAudioSource()
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        return source;
    }
}
