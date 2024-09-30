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


    /// <summary>
    /// ֵ��С��ϵ�ж�
    /// </summary>
    public class RelativePostionComparison
    {
        /// <summary>
        /// ��Է�λö��
        /// </summary>
        public enum RelativePostion
        {
            [EnumName("�غ�")]
            coincide,
            [EnumName("�Ϸ�")]
            up,
            [EnumName("�·�")]
            down,
            [EnumName("��")]
            left,
            [EnumName("�ҷ�")]
            right
        }

        /// <summary>
        /// �Ƚ�Ŀ���������ԭ����㣬��Unity2D����ϵ�е����λ��
        /// </summary>
        /// <param name="way">ֵ�ȽϷ�ʽ</param>
        /// <param name="value1">Ҫ�Ƚϵ�ֵ1</param>
        /// <param name="value2">Ҫ�Ƚϵ�ֵ2</param>
        /// <returns>������ֵ�Ĵ�С��ϵ</returns>
        public static bool InRelativePostion(Vector2 aimPos, Vector2 sourcePos, RelativePostion rp, float coincideDistance = 0.1f)
            => rp switch
            {
                RelativePostion.coincide => (aimPos -  sourcePos).magnitude < coincideDistance,
                RelativePostion.up => aimPos.y > sourcePos.y,
                RelativePostion.down => aimPos.y < sourcePos.y,
                RelativePostion.left => aimPos.x < sourcePos.x,
                RelativePostion.right => aimPos.x > sourcePos.x
            };
    }


    /// <summary>
    /// ���ɷ�Χ����״ö��
    /// </summary>
    public enum RangeShape
    {
        [EnumName("����")]
        point,
        [EnumName("����")]
        box2D,
        [EnumName("����")]
        box,
        [EnumName("Բ��")]
        circle,
        [EnumName("����")]
        fan,
        [EnumName("����")]
        sphere,
    }
}