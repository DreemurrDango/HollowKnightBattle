using UnityEngine;
using Enums;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// 等待形变满足与指定形变的距离条件
/// </summary>
[TaskName("等待距离满足条件")]

public class WaitForDistance : Action
{
    /// <summary>
    /// 比较方式
    /// </summary>
    public enum CompareWay
    {
        [EnumName("只比较X轴")]
        compareX,
        [EnumName("只比较Y轴")]
        compareY,
        [EnumName("只比较Z轴")]
        compareZ,
        [EnumName("比较距离")]
        compareDistance
    }

    [TT("起点形变，若不指定则为挂载当前行为树组件的形变")]
    public SharedTransform originT;
    [TT("目标形变，若不指定则直接将起点形变的坐标值用于判断")]
    public SharedTransform aimT;
    [TT("两形变之间的比较方式")]
    public CompareWay compareWay;
    [TT("值比较方式")]
    public ValueComparison.ComparisonWay comparisonWay;
    [TT("要比较的目标值")]
    public SharedVector3 value;
    [TT("是否对结果取反")]
    public bool invertResult = false;

    public override void OnAwake()
    {
        if (originT.Value == null) originT.Value = transform;
    }

	public override TaskStatus OnUpdate()
	{
        Vector3 offset, aimpos;
        if (aimT.Value == null) offset = originT.Value.position;
        else offset = aimT.Value.position - originT.Value.position;
        bool result = compareWay switch
        {
            CompareWay.compareX => ValueComparison.EqualJudge(comparisonWay, offset.x, value.Value.x),
            CompareWay.compareY => ValueComparison.EqualJudge(comparisonWay, offset.y, value.Value.y),
            CompareWay.compareZ => ValueComparison.EqualJudge(comparisonWay, offset.z, value.Value.z),
            CompareWay.compareDistance => ValueComparison.EqualJudge(comparisonWay, offset.magnitude, value.Value.magnitude)
        };
        if (invertResult) result = !result;
        return result ? TaskStatus.Success : TaskStatus.Running;
    }
}