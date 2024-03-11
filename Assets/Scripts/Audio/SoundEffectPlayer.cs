using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCollection;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectPlayer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("要播放的音效注册名")]
    private string SEName;
    [SerializeField]
    [Tooltip("是否在启用时自动播放")]
    private bool playOnEnable;
    [SerializeField]
    [Tooltip("播放完成后执行完成动作前额外等待时间：设为0时则在播放完成后直接执行结束动作，如销毁、禁用等")]
    private float delayOnCompleted;
    [SerializeField]
    [Tooltip("是否在播放完成后自动销毁")]
    private bool destroyOnCompleted;

    /// <summary>
    /// 要播放的目标音效信息
    /// </summary>
    private SoundEffectInfo info;
    /// <summary>
    /// 该脚本所在对象的音频源组件
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// 该播放器要播放的音效
    /// </summary>
    public SoundEffectInfo Info => info;
    /// <summary>
    /// 要使用的音频源组件
    /// </summary>
    public AudioSource GetAudioSource
    {
        get
        {
            if(audioSource == null)audioSource = GetComponent<AudioSource>();
            return audioSource;
        }
    }

    private void Awake()
    {
        if (SEName != "") info = AudioManager.Instance.GetSEInfo(SEName);
    }

    private void OnEnable()
    {
        if (playOnEnable && SEName != "") Play();
    }

    /// <summary>
    /// 载入源数据
    /// </summary>
    /// <param name="info">要读取的音效信息</param>
    public void Init(SoundEffectInfo info)
    {
        this.info = info;
        this.SEName = info.name;
    }

    /// <summary>
    /// 播放音效，并在播放完成后自动销毁
    /// </summary>
    public void Play()
    {
        GetAudioSource.clip = info.clip;
        GetAudioSource.volume = Random.Range(info.volumeRange.x, info.volumeRange.y);
        GetAudioSource.pitch = Random.Range(info.pitchRange.x, info.pitchRange.y);
        GetAudioSource.Play();
        if (destroyOnCompleted) Destroy(gameObject, info.clip.length + delayOnCompleted);
    }
}
