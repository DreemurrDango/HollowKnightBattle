using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("敌人僵直状态")]
[TaskCategory("敌人")]
[TaskDescription("设置敌人进入/退出僵直状态，在僵直状态下不再计数受击次数/连击次数")]
public class SetEnemyLimpState : Action
{
    [TT("挂载有敌人组件的游戏对象，若置空则在该行为树所在对象上寻找")]
    public SharedGameObject enemyGO;
    [TT("T：进入僵直状态;F：退出僵直状态")]
    public bool exitOrEnter;

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
        enemy.SetLimpState(exitOrEnter);
        return TaskStatus.Success;
    }
}