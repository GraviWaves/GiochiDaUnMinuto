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


        bgmSource = gameObject.AddComponent<AudioSource>();
        seSource = gameObject.AddComponent<AudioSource>();
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

    public void SetSeVolume(float volume)
    {
        SeVolume = Mathf.Clamp(volume, 0, 1);
        seSource.volume = SeVolume;
    }


    public void PlayBgmOneShot(AudioClip clip)
    {
        bgmSource.volume = BgmVolume;
        bgmSource.PlayOneShot(clip);
    }

    public void PlayBgm(AudioClip clip)
    {
        bgmSource.volume = BgmVolume;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySeOneShot(AudioClip clip)
    {
        seSource.volume = SeVolume;
        seSource.PlayOneShot(clip);
    }
}
