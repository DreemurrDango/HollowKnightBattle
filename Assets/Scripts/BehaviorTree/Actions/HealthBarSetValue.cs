using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("设置值")]
[TaskCategory("UI/生命值条")]
[TaskDescription("直接设置生命值条的显示值")]
public class HealthBarSetValue : Action
{
    [TT("要设置的生命值显示条所在游戏对象")]
    public SharedGameObject healthBarGO;
    [TT("要显示的数值")]
    public float value;

    /// <summary>
    /// 指向的生命值显示条
    /// </summary>
    private BossHealthBar healthBar;

    public override void OnStart()
    {
        if (healthBarGO.Value == null) healthBar = GetComponent<BossHealthBar>();
        else healthBar = healthBarGO.Value.GetComponent<BossHealthBar>();
    }

    public override TaskStatus OnUpdate()
    {
        healthBar.Value = value;
        return TaskStatus.Success;
    }
}