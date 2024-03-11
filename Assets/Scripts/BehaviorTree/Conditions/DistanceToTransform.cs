using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// �����ж����α�֮��ľ���
/// </summary>
[TaskName("�����ж�")]
public class DistanceToTransform : Conditional
{
	[TT("����α䣬����ָ����Ϊ���ص�ǰ��Ϊ��������α�")]
	public SharedTransform originT;
    [TT("Ŀ���α�")]
    public SharedTransform aimT;
	[TT("�������α���X���ϵľ���")]
	public bool ignoreXDistance;
    [TT("�������α���Y���ϵľ���")]
    public bool ignoreYDistance;
    [TT("�������α���Z���ϵľ���")]
    public bool ignoreZDistance;
    [TT("�Ƿ�ʹ�ñ�������ֵ���бȽ�")]
    public bool useLocalPosition = false;
    [TT("ֵ�ȽϷ�ʽ")]
    public ValueComparison.ComparisonWay comparisonWay;
    [TT("Ҫ�Ƚϵ�Ŀ��ֵ")]
    public SharedFloat value;
    [TT("�Ƿ�Խ��ȡ��")]
    public bool invertResult = false;

    public override void OnAwake()
    {
        if (originT.Value == null) originT.Value = transform;
        if (aimT.Value == null) Debug.LogError("δָ��Ŀ���α䣡");
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