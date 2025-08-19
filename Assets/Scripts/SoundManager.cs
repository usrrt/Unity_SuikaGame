using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _inst;

    public static SoundManager Instance
    {
        // SoundManager�� ���� �ٸ� ������ �����Ҷ� SoundManager�� null�̸� Resources�� �ִ� �������� �ҷ��� �����ϰ� SoundManager�� �Ҵ����ش�
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
            Debug.LogWarning(sfxName + " ȿ���� ����");
            return;
        }
        
        sfxAudio.PlayOneShot(clip);
    }
}
