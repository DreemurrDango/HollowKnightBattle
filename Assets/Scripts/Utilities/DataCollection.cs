using UnityEngine;
using Constant;
using System.Collections.Generic;
using System;

namespace DataCollection
{    
     /// <summary>
     /// 音效注册信息，在音效播放时要设置的数据
     /// </summary>
    [Serializable]
    public struct SoundEffectInfo
    {
        /// <summary>
        /// 当欲播放的音效已有实例在播放时的解决方案
        /// </summary>
        public enum MultiPlaySolution
        {
            [EnumName("继续播放旧实例，无视此次播放任务")]
            playOld,
            [EnumName("打断旧实例的播放，播放新的音效，这会使得音效从零开始播放")]
            playNew,
            [EnumName("总是循环播放音效，无视此次播放任务")] 
            playLoop,
            [EnumName("为新的播放任务创建一个新实例，叠加播放二者")]
            playAll
        }

        [Tooltip("音效使用名")]
        public string name;
        [Tooltip("音效片段")]
        public AudioClip clip;
        [Tooltip("欲播放的音效已在播放时的解决方案")]
        public MultiPlaySolution solution;
        [Tooltip("音量随机范围")]
        public Vector2 volumeRange;
        [Tooltip("音调随机范围")]
        public Vector2 pitchRange;
    }
}