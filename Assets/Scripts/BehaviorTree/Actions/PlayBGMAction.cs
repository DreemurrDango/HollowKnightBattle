using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("���ű�������")]
[TaskCategory("��Ƶ")]
[TaskDescription("���ű�������")]
public class PlayBGMAction : Action
{
	[TT("Ҫ���ŵ���Чע����")]
	public AudioClip bgmClip;
    [TT("���ŵ����ӳ�")]
    public float fadeInTime = 0.2f;
    public override TaskStatus OnUpdate()
	{
		AudioManager.Instance.PlayBGM(bgmClip, fadeInTime);
		return TaskStatus.Success;
	}
}