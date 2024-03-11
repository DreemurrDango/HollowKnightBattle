using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// 条件判断两形变之间的距离
/// </summary>
[TaskName("距离判断")]
public class DistanceToTransform : Conditional
{
	[TT("起点形变，若不指定则为挂载当前行为树组件的形变")]
	public SharedTransform originT;
    [TT("目标形变")]
    public SharedTransform aimT;
	[TT("忽视两形变在X轴上的距离")]
	public bool ignoreXDistance;
    [TT("忽视两形变在Y轴上的距离")]
    public bool ignoreYDistance;
    [TT("忽视两形变在Z轴上的距离")]
    public bool ignoreZDistance;
    [TT("是否使用本地坐标值进行比较")]
    public bool useLocalPosition = false;
    [TT("值比较方式")]
    public ValueComparison.ComparisonWay comparisonWay;
    [TT("要比较的目标值")]
    public SharedFloat value;
    [TT("是否对结果取反")]
    public bool invertResult = false;

    public override void OnAwake()
    {
        if (originT.Value == null) originT.Value = transform;
        if (aimT.Value == null) Debug.LogError("未指定目标形变！");
    }

    public override TaskStatus OnUpdate()
	{
        Vector3 offset;
        if (!useLocalPosition) offset = aimT.Value.transform.position - originT.Value.transform.position;
        else offset = aimT.Value.transform.localPosition - originT.Value.transform.localPosition;
        if (ignoreXDistance) offset.x = 0;
        if (ignoreYDistance) offset.y = 0;
        if (ignoreZDistance) offset.z = 0;
        bool result = ValueComparison.EqualJudge(comparisonWay, offset.magnitude, value.Value);
        if (invertResult) result = !result;
        return result ? TaskStatus.Success : TaskStatus.Failure;
	}
}