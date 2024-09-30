using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// 判断敌人的生命值是否满足条件
/// </summary>
[TaskName("受击次数判断")]
public class HitCountCondition : Conditional
{
	[TT("携带ENEMY脚本的游戏对象，若不指定则在行为树所在对象上寻找组件")]
	public SharedGameObject enemyGO;
    [TT("值比较方式")]
    public ValueComparison.ComparisonWay comparisonWay;
    [TT("要比较的值")]
    public SharedInt value;
    [TT("若为真则比较连击次数而不是受击次数")]
    public bool combo = false;
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
        float v = combo ? enemy.BeHitComboCount : enemy.BeHitCount;
        bool result = ValueComparison.EqualJudge(comparisonWay, v, value.Value);
        if (invertResult) result = !result;
        return result ? TaskStatus.Success : TaskStatus.Failure;
    }
}