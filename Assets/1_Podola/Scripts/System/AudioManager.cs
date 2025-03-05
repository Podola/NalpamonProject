using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    public AudioSource bgmSource; 
    public AudioSource sfxSource; 

    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // Resources/BGM 폴더 내 모든 AudioClip 읽어오기
        AudioClip[] loadedBgm = Resources.LoadAll<AudioClip>("BGM");
        foreach (var clip in loadedBgm)
        {
            bgmClips[clip.name] = clip;
        }

        // Resources/SFX 폴더 내 모든 AudioClip 읽어오기
        AudioClip[] loadedSfx = Resources.LoadAll<AudioClip>("SFX");
        foreach (var clip in loadedSfx)
        {
            sfxClips[clip.name] = clip;
        }
    }

    /// <summary>
    /// BGM 재생
    /// </summary>
    public void PlayBGM(string clipName)
    {
        if (bgmClips.ContainsKey(clipName))
        {
            bgmSource.clip = bgmClips[clipName];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"[AudioManager] BGM clip '{clipName}' not found!");
        }
    }

    /// <summary>
    /// BGM 정지
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    /// <summary>
    /// SFX 재생
    /// </summary>
    public void PlaySFX(string clipName)
    {
        if (sfxClips.ContainsKey(clipName))
        {
            // OneShot으로 재생하면 현재 재생중인 SFX를 멈추지 않고도 겹쳐서 재생 가능
            sfxSource.PlayOneShot(sfxClips[clipName]);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] SFX clip '{clipName}' not found!");
        }
    }
}
