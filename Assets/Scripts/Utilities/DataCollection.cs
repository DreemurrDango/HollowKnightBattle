using UnityEngine;
using Constant;
using System.Collections.Generic;
using System;

namespace DataCollection
{
    /// <summary>
    /// ģ����
    /// </summary>
    [System.Serializable]
    public class Template
    {
        //����������
    }
    
    /// <summary>
     /// ��Чע����Ϣ������Ч����ʱҪ���õ�����
     /// </summary>
    [Serializable]
    public struct SoundEffectInfo
    {
        [Tooltip("��Чʹ����")]
        public string name;
        [Tooltip("��ЧƬ��")]
        public AudioClip clip;
        [Tooltip("���������Χ")]
        public Vector2 volumeRange;
        [Tooltip("���������Χ")]
        public Vector2 pitchRange;
    }
}