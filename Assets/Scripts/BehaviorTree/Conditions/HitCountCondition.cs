using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// �жϵ��˵�����ֵ�Ƿ���������
/// </summary>
[TaskName("�ܻ������ж�")]
public class HitCountCondition : Conditional
{
	[TT("Я��ENEMY�ű�����Ϸ��������ָ��������Ϊ�����ڶ�����Ѱ�����")]
	public SharedGameObject enemyGO;
    [TT("ֵ�ȽϷ�ʽ")]
    public ValueComparison.ComparisonWay comparisonWay;
    [TT("Ҫ�Ƚϵ�ֵ")]
    public SharedInt value;
    [TT("��Ϊ����Ƚ����������������ܻ�����")]
    public bool combo = false;
    [TT("�Ƿ�Խ��ȡ��")]
    public bool invertResult = false;

    /// <summary>
    /// Ҫ��ȡ�ĵ������
    /// </summary>
    private Enemy enemy;

    public override void OnAwake()
    {
        if(enemyGO.Value == null )enemy = GetComponent<Enemy>();
        else enemy = enemyGO.Value.GetComponent<Enemy>();
        if (enemy == null) Debug.LogError("δָ���������");
    }

    public override TaskStatus OnUpdate()
    {
        float v = combo ? enemy.BeHitComboCount : enemy.BeHitCount;
        bool result = ValueComparison.EqualJudge(comparisonWay, v, value.Value);
        if (invertResult) result = !result;
        return result ? TaskStatus.Success : TaskStatus.Failure;
    }
}