using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("切换指向目标")]
[TaskCategory("UI/生命值条")]
[TaskDescription("切换生命值条的数据同步目标，注意：这不会更新立即数据")]
public class HealthBarSwitchEnemy : Action
{
    [TT("要设置的生命值显示条所在游戏对象")]
    public SharedGameObject healthBarGO;
    [TT("生命值条新的目标敌人所在的游戏对象，若不指定则从当前对象上寻找")]
    public SharedGameObject enemyGO;

    public override TaskStatus OnUpdate()
    {
        var hb = healthBarGO.Value.GetComponent<BossHealthBar>();
        var e = enemyGO.Value != null ? enemyGO.Value.GetComponent<Enemy>() : GetComponent<Enemy>();
        hb.SwitchTarget(e);
        return TaskStatus.Success;
    }
}