
using UnityEngine;
using UnityEngine.Audio;


public enum AudioEffectType
{
    Music,
    Sound
}

/// <summary>
/// 暂时可以考虑不用加热更，一般音效文件热更的较少，热更的话只要替换加载方式就可以
/// </summary>
public class AudioManager : SingletonBehaviour<AudioManager>
{
    public static readonly float MaxVolume = 20f;
    public static readonly float MinVolume = -80f;

    private static readonly string MUSIC_BASE_PATH = "Audios/Music/";
    private static readonly string SOUND_BASE_PATH = "Audios/SFX/";

    public static AudioClip LoadAudioClip(AudioEffectType effectType, string clipName)
    {
        string clipPath = "";
        switch (effectType)
        {
            case AudioEffectType.Music:
                clipPath = MUSIC_BASE_PATH + clipName;
                break;

            case AudioEffectType.Sound:
                clipPath = SOUND_BASE_PATH + clipName;
                break;
        }
        return Resources.Load<AudioClip>(clipPath);
    }

    public static void PlayMusic(string clipName)
    {
        PlayMusic(clipName, 0);
    }

    public static void PlayMusic(string clipName, ulong delay)
    {
        AudioClip audioClip = LoadAudioClip(AudioEffectType.Music, clipName);
        PlayMusic(audioClip);
    }

    public static void PlayMusic(AudioClip audioClip)
    {
        PlayMusic(audioClip, 0);
    }

    public static void PlayMusic(AudioClip audioClip, ulong delay)
    {
        GetIns().PlayMusicInternal(audioClip, delay);
    }

    public static void PlayAmbient(string clipName)
    {
        PlayAmbient(clipName, 0);
    }

    public static void PlayAmbient(string clipName, ulong delay)
    {
        AudioClip audioClip = LoadAudioClip(AudioEffectType.Music, clipName);
        PlayAmbient(audioClip);
    }

    public static void PlayAmbient(AudioClip audioClip)
    {
        PlayAmbient(audioClip, 0);
    }

    public static void PlayAmbient(AudioClip audioClip, ulong delay)
    {
        GetIns().PlayAmbientInternal(audioClip, delay);
    }

    public static void PlaySound(string clipName)
    {
        AudioClip audioClip = LoadAudioClip(AudioEffectType.Sound, clipName);
        PlaySound(audioClip);
    }

    public static void PlaySound(AudioClip audioClip)
    {
        GetIns().PlaySoundInternal(audioClip);
    }

    public static void PlaySound(AudioClip audioClip, float pitch)
    {
        GetIns().PlaySoundInternal(audioClip, pitch);
    }

    private static void SetMasterVolume(float volume)
    {
        GetIns().SetMasterVolumeInternal(volume);
    }

    public static void SetMusicVolume(float volume)
    {
        GetIns().SetMusicVolumeInternall(volume);
    }

    public static void SetSoundVolume(float volume)
    {
        GetIns().SetSoundVolumeInternal(volume);
    }

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource ambientSource;
    [SerializeField]
    private AudioSource soundSource;

    void OnEnable()
    {
        musicSource.playOnAwake = false;
        musicSource.loop = true;

        ambientSource.playOnAwake = false;
        ambientSource.loop = true;

        soundSource.playOnAwake = false;
        soundSource.loop = false;
    }

    private void SetMasterVolumeInternal(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    private void SetMusicVolumeInternall(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    private void SetSoundVolumeInternal(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
    }

    private void PlayMusicInternal(AudioClip audioClip, ulong delay)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        if (audioClip != null)
        {
            musicSource.clip = audioClip;
            musicSource.Play();
        }
    }

    private void PlayAmbientInternal(AudioClip audioClip, ulong delay)
    {
        if (ambientSource.isPlaying)
        {
            ambientSource.Stop();
        }
        if (audioClip != null)
        {
            ambientSource.clip = audioClip;
            ambientSource.Play();
        }
    }

    private void PlaySoundInternal(AudioClip audioClip, float pitch = 1f)
    {
        if (audioClip != null)
        {
            soundSource.pitch = pitch;
            soundSource.PlayOneShot(audioClip);
        }
    }

    //循环播放
    public static void PlayLoopAudio(Transform trans, string audioName)
    {
        AudioClip audioClip = LoadAudioClip(AudioEffectType.Sound, audioName);
        if (audioClip == null)
        {
            //LogMgr.Instance.LogError("Not Audio name：" + audioName);
            return;
        }
        AudioSource source = trans.GetComponent<AudioSource>();
        if (source == null)
        {
            source = trans.gameObject.AddComponent<AudioSource>();
        }

        AudioMixerGroup[] groups = GetIns().audioMixer.FindMatchingGroups("Sound");
        source.outputAudioMixerGroup = groups[0];
        source.loop = true;
        source.clip = audioClip;
        source.Play();

    }
    //停止循环播放
    public static void StopLoopAudio(Transform trans, string audioName)
    {
        AudioSource source = trans.GetComponent<AudioSource>();
        if (source != null)
        {
            if (source.clip != null)
            {
                if (source.clip.name == audioName)
                    source.Stop();
            }
        }
    }

}
