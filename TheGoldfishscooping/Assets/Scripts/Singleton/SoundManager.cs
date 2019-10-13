using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public float MIN_VOLUME_SE = 0.4f;
    public float MAX_VOLUME_SE = 1.0f;

    [SerializeField]
    private AudioSource m_BgmAudioSource;

    [SerializeField]
    private AudioSource m_SeAudioSource;

    // [SerializeField]
    // private AudioSource m_SeGloveCatchAudioSource;

    private AudioClip[] m_SEClip;
    private AudioClip[] m_BgmClip;

    private static readonly string SE_PATH = "Sound/SE";
    private static readonly string BGM_PATH = "Sound/BGM";

    private float m_BaseSeVolume = 0f;

    public enum SE
    {
        NONE = -1,
        BUTTON_YES = 0,
        CATCH_SUCCESS = 1,
        THROW_END = 2,
        MAX,
    }

    public enum BGM
    {
        NONE = -1,
        NATSUMATSURI = 0,
    }

    public override void OnAwake()
    {
        m_SEClip = Resources.LoadAll<AudioClip>(SE_PATH);
        m_BgmClip = Resources.LoadAll<AudioClip>(BGM_PATH);
        m_BaseSeVolume = m_SeAudioSource.volume;
        PlayBgm(SoundManager.BGM.NATSUMATSURI);
    }

    public void PlayButtonSE()
    {
        m_SeAudioSource.volume = m_BaseSeVolume * 0.2f;
        PlaySE(SE.THROW_END);
    }

    private void PlaySE(SE playSE)
    {
        AudioClip playClip = GetSeAudioClip(playSE);
        m_SeAudioSource.PlayOneShot(playClip);
    }

    public void PlaySE(SE playSE, float volume = 1.0f)
    {
        m_SeAudioSource.volume = volume * m_BaseSeVolume;
        PlaySE(playSE);
    }

    // private void PlayGloveCatchSE(SE playSE)
    // {
    //     AudioClip playClip = GetSeAudioClip(playSE);
    //     m_SeGloveCatchAudioSource.PlayOneShot(playClip);
    // }

    // public void PlayGloveCatchSE(SE playSE, float volume = 1.0f)
    // {
    //     m_SeGloveCatchAudioSource.volume = volume * m_BaseSeVolume;
    //     PlayGloveCatchSE(playSE);
    // }

    private AudioClip GetSeAudioClip(SE playSE)
    {
        AudioClip clip = m_SEClip[(int)playSE];
        return clip;
    }

    public void PlayBgm(BGM playBgm)
    {
        m_BgmAudioSource.clip = GetBgmAudioClip(playBgm);
        m_BgmAudioSource.Play();
    }

    private AudioClip GetBgmAudioClip(BGM playBgm)
    {
        AudioClip clip = m_BgmClip[(int)playBgm];
        return clip;
    }

    public float GetBgmVolume()
    {
        return m_BgmAudioSource.volume;
    }

    public float GetSeVolume()
    {
        return m_BaseSeVolume;
    }

    public void SetBgmVolume(float value)
    {
        m_BgmAudioSource.volume = value;
    }

    // public void SetSeVolume(float value)
    // {
    //     m_SeAudioSource.volume = value;
    // }

    public void SetBaseSeVolume(float value)
    {
        m_BaseSeVolume = value;
    }
}
