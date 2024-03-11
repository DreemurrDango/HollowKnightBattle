using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("播放音效")]
[TaskCategory("音频")]
[TaskDescription("播放音效")]
public class PlaySEAction : Action
{
	[TT("要播放的音效注册名")]
	public string SEName;
	public override TaskStatus OnUpdate()
	{
		AudioManager.Instance.PlaySE(SEName, transform);
		return TaskStatus.Success;
	}
}