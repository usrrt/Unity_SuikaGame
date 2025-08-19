using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _inst;

    public static SoundManager Instance
    {
        // SoundManager가 없는 다른 씬에서 시작할때 SoundManager가 null이면 Resources에 있는 프리팹을 불러와 생성하고 SoundManager를 할당해준다
        get
        {
            if (_inst == null)
            {
                GameObject pf = Resources.Load<GameObject>("SoundManager");
                if (pf != null)
                {
                    GameObject obj = Instantiate(pf);
                    _inst = obj.GetComponent<SoundManager>();
                }
            }

            return _inst;
        }
    }

    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource sfxAudio;

    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip[] sfxClips;

    private void Awake()
    {
        if (_inst == null)
            _inst = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        bgmAudio.clip = bgmClip;
        bgmAudio.loop = true;
        bgmAudio.Play();
    }

    public void PlaySFX(string sfxName)
    {
        AudioClip clip = Array.Find(sfxClips, x => x.name == sfxName);
        
        if(clip == null)
        {
            Debug.LogWarning(sfxName + " 효과음 없음");
            return;
        }
        
        sfxAudio.PlayOneShot(clip);
    }
}
