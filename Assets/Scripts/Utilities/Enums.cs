using UnityEngine;

namespace Enums
{
    /// <summary>
    /// 枚举模板
    /// </summary>
    public class Sample
    {
        //TODO:定义枚举
        public enum Enum
        {
            [EnumName("枚举1")]
            enum1,
            [EnumName("枚举2")]
            enum2
        }
        //TODO:填写枚举的显示名
        public readonly string[] enumNames = { };

        #region 通用方法无需更改
        public string EnumName(Enum e) => enumNames[(int)e];
        public int Count => enumNames.Length;
        #endregion
    }

    /// <summary>
    /// 可被攻击命中的对象
    /// </summary>
    public class HitableObject
    {
        /// <summary>
        /// 可被攻击命中的物体类型
        /// </summary>
        public enum Type
        {
            [EnumName("枚举1")]
            enemy,
            [EnumName("枚举2")]
            item,
            [EnumName("枚举2")]
            environment
        }
    }

    /// <summary>
    /// 值大小关系判断
    /// </summary>
    public class ValueComparison
    {
        /// <summary>
        /// 值大小比较方式
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
        /// 两值的比较结果
        /// </summary>
        /// <param name="way">值比较方式</param>
        /// <param name="value1">要比较的值1</param>
        /// <param name="value2">要比较的值2</param>
        /// <returns>返回两值的大小关系</returns>
        public static bool EqualJudge(ComparisonWay way, float value1, float value2)
            => way switch
            {
                ComparisonWay.moreThen => value1 > value2,
                ComparisonWay.lessThen => value1 < value2,
                ComparisonWay.equal => value1 == value2,
            };
    }


    /// <summary>
    /// 值大小关系判断
    /// </summary>
    public class RelativePostionComparison
    {
        /// <summary>
        /// 相对方位枚举
        /// </summary>
        public enum RelativePostion
        {
            [EnumName("重合")]
            coincide,
            [EnumName("上方")]
            up,
            [EnumName("下方")]
            down,
            [EnumName("左方")]
            left,
            [EnumName("右方")]
            right
        }

        /// <summary>
        /// 比较目标坐标对于原坐标点，在Unity2D坐标系中的相对位置
        /// </summary>
        /// <param name="way">值比较方式</param>
        /// <param name="value1">要比较的值1</param>
        /// <param name="value2">要比较的值2</param>
        /// <returns>返回两值的大小关系</returns>
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
    /// 生成范围的形状枚举
    /// </summary>
    public enum RangeShape
    {
        [EnumName("定点")]
        point,
        [EnumName("矩形")]
        box2D,
        [EnumName("盒形")]
        box,
        [EnumName("圆形")]
        circle,
        [EnumName("扇形")]
        fan,
        [EnumName("球形")]
        sphere,
    }
}