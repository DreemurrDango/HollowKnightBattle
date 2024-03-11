using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// �жϵ��˵�����ֵ�Ƿ���������
/// </summary>
[TaskName("����ֵ�ж�")]
public class HealthCondition : Conditional
{
	[TT("Я��ENEMY�ű�����Ϸ��������ָ��������Ϊ�����ڶ�����Ѱ�����")]
	public SharedGameObject enemyGO;
    [TT("ֵ�ȽϷ�ʽ")]
    public ValueComparison.ComparisonWay comparisonWay;
    [TT("Ҫ�Ƚϵ�Ŀ��ֵ")]
    public SharedFloat value;
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
        bool result = ValueComparison.EqualJudge(comparisonWay, enemy.Health, value.Value);
        if (invertResult) result = !result;
        return result ? TaskStatus.Success : TaskStatus.Failure;
    }
}