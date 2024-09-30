using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("单次生成对象")]
/// <summary>
/// 对象生成器生成一次对象
/// </summary>
public class SpawnerPlayOnce : Action
{
	[TT("要操作的生成器对象，若不指定则从行为树所在对象上寻找")]
	public SharedGameObject spawnerGO;

    /// <summary>
    /// 生成器组件
    /// </summary>
    private Spawner spawner;

    public override void OnAwake()
    {
        spawner = spawnerGO.Value?.GetComponent<Spawner>();
        if (spawner == null) Debug.LogWarning("未指定生成器组件！");
    }

    public override TaskStatus OnUpdate()
	{
		spawner.PlayOnce();
		return TaskStatus.Success;
	}
}