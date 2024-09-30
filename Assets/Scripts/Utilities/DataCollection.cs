using UnityEngine;
using Constant;
using System.Collections.Generic;
using System;

namespace DataCollection
{    
     /// <summary>
     /// ��Чע����Ϣ������Ч����ʱҪ���õ�����
     /// </summary>
    [Serializable]
    public struct SoundEffectInfo
    {
        /// <summary>
        /// �������ŵ���Ч����ʵ���ڲ���ʱ�Ľ������
        /// </summary>
        public enum MultiPlaySolution
        {
            [EnumName("�������ž�ʵ�������Ӵ˴β�������")]
            playOld,
            [EnumName("��Ͼ�ʵ���Ĳ��ţ������µ���Ч�����ʹ����Ч���㿪ʼ����")]
            playNew,
            [EnumName("����ѭ��������Ч�����Ӵ˴β�������")] 
            playLoop,
            [EnumName("Ϊ�µĲ������񴴽�һ����ʵ�������Ӳ��Ŷ���")]
            playAll
        }

        [Tooltip("��Чʹ����")]
        public string name;
        [Tooltip("��ЧƬ��")]
        public AudioClip clip;
        [Tooltip("�����ŵ���Ч���ڲ���ʱ�Ľ������")]
        public MultiPlaySolution solution;
        [Tooltip("���������Χ")]
        public Vector2 volumeRange;
        [Tooltip("���������Χ")]
        public Vector2 pitchRange;
    }
}