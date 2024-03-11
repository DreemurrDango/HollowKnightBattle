using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("结束游戏")]
[TaskCategory("流程控制")]
[TaskDescription("游戏结束，进入结算流程")]
public class GameEnd : Action
{
	[TT("玩家结果：玩家是否获得了胜利")]
	public bool doPlayerWin = true;
    [TT("在显示结算UI前的延迟时间")]
    public float delayTimeBeforeShowGameResultUI = 2f;

	public override TaskStatus OnUpdate()
	{
		GameManager.Instance.GameEnd(doPlayerWin, delayTimeBeforeShowGameResultUI);
		return TaskStatus.Success;
	}
}