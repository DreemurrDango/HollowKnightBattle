using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCollection;

/// <summary>
/// 音效注册表数据库：注册项目中所有使用到的音频资源，设置相关播放数据
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "数据/音效注册表")]
public class SoundEffectDataBaseSO: ScriptableObject
{    
    /// <summary>
    /// 项目中所有使用到的音频资源配置信息
    /// </summary>
    public List<SoundEffectInfo> soundEffectInfos = new List<SoundEffectInfo>();
}
