using UnityEngine;
using Constant;
using System.Collections.Generic;
using System;

namespace DataCollection
{
    /// <summary>
    /// 模板类
    /// </summary>
    [System.Serializable]
    public class Template
    {
        //填入类属性
    }
    
    /// <summary>
     /// 音效注册信息，在音效播放时要设置的数据
     /// </summary>
    [Serializable]
    public struct SoundEffectInfo
    {
        [Tooltip("音效使用名")]
        public string name;
        [Tooltip("音效片段")]
        public AudioClip clip;
        [Tooltip("音量随机范围")]
        public Vector2 volumeRange;
        [Tooltip("音调随机范围")]
        public Vector2 pitchRange;
    }
}