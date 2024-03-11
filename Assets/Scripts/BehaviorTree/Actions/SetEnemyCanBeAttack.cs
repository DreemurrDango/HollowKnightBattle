using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("设置无敌状态")]
[TaskCategory("敌人")]
[TaskDescription("设置敌人的无敌状态，使其可/不可被攻击命中")]
public class SetEnemyCanBeAttack : Action
{
    [TT("挂载有敌人组件的游戏对象，若置空则在该行为树所在对象上寻找")]
    public SharedGameObject enemyGO;
    [TT("要设置该敌人是否可被攻击的值")]
    public bool canBeAttack;

    /// <summary>
    /// 敌人脚本
    /// </summary>
    private Enemy enemy;

    public override void OnAwake()
    {
        if (enemyGO.Value == null) enemy = GetComponent<Enemy>();
        else enemy = enemyGO.Value.GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate()
    {
        enemy.CanBeHit = canBeAttack;
        return TaskStatus.Success;
    }
}