using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("������Ч")]
[TaskCategory("��Ƶ")]
[TaskDescription("������Ч")]
public class PlaySEAction : Action
{
	[TT("Ҫ���ŵ���Чע����")]
	public string SEName;
	public override TaskStatus OnUpdate()
	{
		AudioManager.Instance.PlaySE(SEName, transform);
		return TaskStatus.Success;
	}
}