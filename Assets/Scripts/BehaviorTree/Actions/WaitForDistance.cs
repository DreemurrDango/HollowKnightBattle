using UnityEngine;
using Enums;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// �ȴ��α�������ָ���α�ľ�������
/// </summary>
[TaskName("�ȴ�������������")]

public class WaitForDistance : Action
{
    /// <summary>
    /// �ȽϷ�ʽ
    /// </summary>
    public enum CompareWay
    {
        [EnumName("ֻ�Ƚ�X��")]
        compareX,
        [EnumName("ֻ�Ƚ�Y��")]
        compareY,
        [EnumName("ֻ�Ƚ�Z��")]
        compareZ,
        [EnumName("�ȽϾ���")]
        compareDistance
    }

    [TT("����α䣬����ָ����Ϊ���ص�ǰ��Ϊ��������α�")]
    public SharedTransform originT;
    [TT("Ŀ���α䣬����ָ����ֱ�ӽ�����α������ֵ�����ж�")]
    public SharedTransform aimT;
    [TT("���α�֮��ıȽϷ�ʽ")]
    public CompareWay compareWay;
    [TT("ֵ�ȽϷ�ʽ")]
    public ValueComparison.ComparisonWay comparisonWay;
    [TT("Ҫ�Ƚϵ�Ŀ��ֵ")]
    public SharedVector3 value;
    [TT("�Ƿ�Խ��ȡ��")]
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