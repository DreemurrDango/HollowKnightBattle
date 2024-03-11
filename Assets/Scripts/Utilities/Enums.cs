using UnityEngine;

namespace Enums
{
    /// <summary>
    /// ö��ģ��
    /// </summary>
    public class Sample
    {
        //TODO:����ö��
        public enum Enum
        {
            [EnumName("ö��1")]
            enum1,
            [EnumName("ö��2")]
            enum2
        }
        //TODO:��дö�ٵ���ʾ��
        public readonly string[] enumNames = { };

        #region ͨ�÷����������
        public string EnumName(Enum e) => enumNames[(int)e];
        public int Count => enumNames.Length;
        #endregion
    }

    /// <summary>
    /// �ɱ��������еĶ���
    /// </summary>
    public class HitableObject
    {
        /// <summary>
        /// �ɱ��������е���������
        /// </summary>
        public enum Type
        {
            [EnumName("ö��1")]
            enemy,
            [EnumName("ö��2")]
            item,
            [EnumName("ö��2")]
            environment
        }
    }

    /// <summary>
    /// ֵ��С��ϵ�ж�
    /// </summary>
    public class ValueComparison
    {
        /// <summary>
        /// ֵ��С�ȽϷ�ʽ
        /// </summary>
        public enum ComparisonWay
        {
            [EnumName(">")]
            moreThen,
            [EnumName("<")]
            lessThen,
            [EnumName("=")]
            equal
        }

        /// <summary>
        /// ��ֵ�ıȽϽ��
        /// </summary>
        /// <param name="way">ֵ�ȽϷ�ʽ</param>
        /// <param name="value1">Ҫ�Ƚϵ�ֵ1</param>
        /// <param name="value2">Ҫ�Ƚϵ�ֵ2</param>
        /// <returns>������ֵ�Ĵ�С��ϵ</returns>
        public static bool EqualJudge(ComparisonWay way, float value1, float value2)
            => way switch
            {
                ComparisonWay.moreThen => value1 > value2,
                ComparisonWay.lessThen => value1 < value2,
                ComparisonWay.equal => value1 == value2,
            };
    }
}