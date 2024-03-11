using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskDescription("复活敌人，主要是在数据上恢复其脚本各项数据至正常值")]
public class Rebrith : Action
{
	[TT("要复活的敌人脚本所在的游戏对象，若置空则从当前行为树所在对象上寻找目标组件")]
	public SharedGameObject enemyGO;
    [TT("复活后的新生命值，若设置为0则回复至原生命值上限")]
    public SharedFloat newHealth;

    /// <summary>
    /// 敌人脚本
    /// </summary>
    private Enemy enemy;

    public override void OnAwake()
    {
		if(enemyGO.Value == null) enemy = GetComponent<Enemy>();
		else enemy = enemyGO.Value.GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate()
	{
		enemy.Rebrith(newHealth.Value);
		return TaskStatus.Success;
	}
}