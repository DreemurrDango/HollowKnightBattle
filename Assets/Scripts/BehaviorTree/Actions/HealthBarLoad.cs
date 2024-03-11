using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("载入数据")]
[TaskCategory("UI/生命值条")]
[TaskDescription("读取BOSS的生命值并进行显示")]
public class HealthBarLoad : Action
{
	[TT("要设置的生命值显示条所在游戏对象")]
	public SharedGameObject healthBarGO;
    [TT("是否要显示过渡动画")]
    public bool showTranstion = false;

    /// <summary>
    /// 指向的生命值显示条
    /// </summary>
    private BossHealthBar healthBar;

    public override void OnStart()
	{
		if(healthBarGO.Value == null) healthBar = GetComponent<BossHealthBar>();
		else healthBar = healthBarGO.Value.GetComponent<BossHealthBar>();
	}

	public override TaskStatus OnUpdate()
	{
		healthBar.LoadData(showTranstion);
		return TaskStatus.Success;
	}
}