using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// 判断敌人的生命值是否满足条件
/// </summary>
[TaskName("生命值判断")]
public class HealthCondition : Conditional
{
	[TT("携带ENEMY脚本的游戏对象，若不指定则在行为树所在对象上寻找组件")]
	public SharedGameObject enemyGO;
    [TT("值比较方式")]
    public ValueComparison.ComparisonWay comparisonWay;
    [TT("要比较的目标值")]
    public SharedFloat value;
    [TT("是否对结果取反")]
    public bool invertResult = false;

    /// <summary>
    /// 要获取的敌人组件
    /// </summary>
    private Enemy enemy;

    public override void OnAwake()
    {
        if(enemyGO.Value == null )enemy = GetComponent<Enemy>();
        else enemy = enemyGO.Value.GetComponent<Enemy>();
        if (enemy == null) Debug.LogError("未指定敌人组件");
    }

    public override TaskStatus OnUpdate()
    {
        bool result = ValueComparison.EqualJudge(comparisonWay, enemy.Health, value.Value);
        if (invertResult) result = !result;
        return result ? TaskStatus.Success : TaskStatus.Failure;
    }
}