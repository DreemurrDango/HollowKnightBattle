using DataCollection;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

/// <summary>
/// 音频播放控制管理者：管理BGM与音效的播放
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    /// <summary>
    /// 音效混音效果枚举
    /// </summary>
    public enum MixerSnapshot
    {
        [EnumName("正常状态")]
        Normal,
        [EnumName("战斗中")]
        InFight,
        [EnumName("主角受伤")]
        MainCharaBeHit
    }

    /// <summary>
    /// 音效混合器快照配置
    /// </summary>
    [Serializable]
    public struct MixerSnapshotConfig
    {
        [Tooltip("快照显示名称，应与实例名称相同")]
        public string name;
        [Tooltip("切换至该快照点需要经过的时间")]
        public float reachTime;
        [Tooltip("快照实例")]
        public AudioMixerSnapshot snapshot;
    }

    [SerializeField]
    [Tooltip("主调音台效果器")]
    private AudioMixer mainMixer;
    [SerializeField]
    [Tooltip("背景音乐播放声音源")]
    private AudioSource BGMSource;
    [SerializeField]
    [Tooltip("背景音乐播放声音源")]
    private AudioSource AmbientSource;
    //TEMP:未使用的以项目数据形式保存的注册表，当前使用的是挂载于AudioManager实例的列表配置
    [SerializeField]
    [Tooltip("音效源数据库")]
    private SoundEffectDataBaseSO soundEffectDB;
    [SerializeField]
    [Tooltip("音效播放实例预制体")]
    private SoundEffectPlayer soundEffectPlayerPrefab;
    [SerializeField]
    [Tooltip("快照信息数据")]
    private List<MixerSnapshotConfig> mixerSnapshotConfigs = new List<MixerSnapshotConfig>();
    [SerializeField]
    [Tooltip("项目中所有使用到的音频资源配置信息")]
    public List<SoundEffectInfo> soundEffectInfos = new List<SoundEffectInfo>();

    /// <summary>
    /// 当前正在播放的快照效果信息
    /// </summary>
    private MixerSnapshotConfig currentSnapshot;
    [DisplayOnly]
    /// <summary>
    /// 当前正在播放的音效实例
    /// </summary>
    public List<SoundEffectPlayer> SEPlayerLists = new List<SoundEffectPlayer>();
    /// <summary>
    /// 根据音效名获得对应的音效播放数据
    /// </summary>
    /// <param name="SEName">音效名</param>
    /// <returns>对应的音效播放数据</returns>
    public SoundEffectInfo GetSEInfo(string SEName) => soundEffectInfos.Find(i => i.name == SEName);

    /// <summary>
    /// 播放新的BGM
    /// </summary>
    /// <param name="bgmClip">要播放的BGM片段</param>
    /// <param name="fadeInTime">音量淡入的时间</param>
    public void PlayBGM(AudioClip bgmClip, float fadeInTime = 0.2f)
    {
        BGMSource.clip = bgmClip;
        if (fadeInTime > 0f) BGMSource.DOFade(1f, fadeInTime);
        else BGMSource.volume = 1f;
        BGMSource.Play();
    }

    /// <summary>
    /// 播放环境音
    /// </summary>
    /// <param name="ambientClip">要播放的环境音片段</param>
    public void PlayAmbient(AudioClip ambientClip)
    {
        AmbientSource.clip = ambientClip;
        AmbientSource.Play();
    }

    /// <summary>
    /// 背景音乐淡出：音量逐渐降低至0
    /// </summary>
    /// <param name="fadeOutTime">淡出延时</param>
    public void BGMFadeOut(float fadeOutTime = 1f)
    {
        if (fadeOutTime > 0f) BGMSource.DOFade(0f, fadeOutTime);
        else BGMSource.volume = 0f;
    }

    /// <summary>
    /// 播放游戏音效
    /// </summary>
    /// <param name="seInfoName">要播放的音效信息注册名</param>
    /// <param name="followT">要跟随的形变位置，若不指定则放在AudioManager下</param>
    public void PlaySE(string seInfoName, Transform followT = null)
    {
        var info = GetSEInfo(seInfoName);
        if (followT == null) followT = transform;
        //寻找是否已存在正在播放的同音效实例
        var sep = SEPlayerLists.Find(s => s.SEName == seInfoName);
        if(sep != null)
        {
            //根据该音效注册信息的多重播放解决方案决定如何播放
            switch (info.solution)
            {
                case SoundEffectInfo.MultiPlaySolution.playOld:
                case SoundEffectInfo.MultiPlaySolution.playLoop:
                    return;
                case SoundEffectInfo.MultiPlaySolution.playNew:
                    sep.Play();
                    return;
                case SoundEffectInfo.MultiPlaySolution.playAll:
                    break;
            }
        }
        var player = Instantiate(soundEffectPlayerPrefab.gameObject,followT).GetComponent<SoundEffectPlayer>();
        SEPlayerLists.Add(player);
        //Debug.Log("PlaySE " + info.name);
        player.Init(info);
        player.Play();
    }
    //WORKFLOW:由于此函数未被实际使用，未实装音效多重播放时的处理
    /// <summary>
    /// 固定位置播放游戏音效
    /// </summary>
    /// <param name="seInfoName">要播放的音效信息注册名</param>
    /// <param name="pos">音效播放固定位置</param>
    public void PlaySE(string seInfoName, Vector3 pos)
    {
        var info = soundEffectInfos.Find(i => i.name == seInfoName);
        var player = Instantiate(soundEffectPlayerPrefab.gameObject, pos, Quaternion.identity).GetComponent<SoundEffectPlayer>();
        player.Init(info);
        player.Play();
    }

    /// <summary>
    /// 根据要切换的快照配置进行切换
    /// </summary>
    /// <param name="config">混音器快照配置数据</param>
    public void SwitchSnapShot(MixerSnapshotConfig config)
    {
        currentSnapshot = config;
        currentSnapshot.snapshot.TransitionTo(currentSnapshot.reachTime);
    }
    /// <summary>
    /// 切换至指定的调音快照
    /// </summary>
    /// <param name="aimSnapshotName">要切换的调音快照名称</param>
    public void SwitchSnapShot(string aimSnapshotName) => 
        SwitchSnapShot(mixerSnapshotConfigs.Find(c => c.name == aimSnapshotName));

    /// <summary>
    /// 暂时切换至指定的调音快照，并在一段时间后复原
    /// </summary>
    /// <param name="aimSnapshotName">目标快照信息</param>
    /// <param name="lastTime">持续时间</param>
    public void SwitchSnapShotTemp(string aimSnapshotName,float lastTime)
    {
        var prevSnapshot = currentSnapshot;
        SwitchSnapShot(aimSnapshotName);
        StartCoroutine(SwitchBackToPrevRoutine());

        IEnumerator SwitchBackToPrevRoutine()
        {
            yield return new WaitForSeconds(lastTime);
            SwitchSnapShot(prevSnapshot);
        }
    }
}
