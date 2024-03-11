using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("开关对象生成器")]
/// <summary>
/// 开关对象生成动作
/// </summary>
public class SwitchSpawner : Action
{
	[TT("要操作的生成器对象")]
	public SharedGameObject spawnerGO;
    [TT("开启或关闭指令")]
    public bool offOrOn;

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
		if(spawner.InSpawning != offOrOn)
        {
            if (offOrOn) spawner.Play();
            else spawner.Stop();
        }
		return TaskStatus.Success;
	}
}