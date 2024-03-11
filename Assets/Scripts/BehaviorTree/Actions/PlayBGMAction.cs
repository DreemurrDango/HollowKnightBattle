using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("²¥·Å±³¾°ÒôÀÖ")]
[TaskCategory("ÒôÆµ")]
[TaskDescription("²¥·Å±³¾°ÒôÀÖ")]
public class PlayBGMAction : Action
{
	[TT("Òª²¥·ÅµÄÒôĞ§×¢²áÃû")]
	public AudioClip bgmClip;
    [TT("²¥·Åµ­ÈëÑÓ³Ù")]
    public float fadeInTime = 0.2f;
    public override TaskStatus OnUpdate()
	{
		AudioManager.Instance.PlayBGM(bgmClip, fadeInTime);
		return TaskStatus.Success;
	}
}