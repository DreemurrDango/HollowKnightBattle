using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("调整面向")]
/// <summary>
/// 通过改变所属形变的localScale.x为1/-1.让其变面向指定的形变
/// </summary>
public class FaceTo : Action
{
	[TT("要面向的形变")]
	public SharedTransform faceT;
    [TT("是否对结果取反")]
    public bool doInvert = false;
	[Min(0.1f)]
    [TT("转向该单位需要达到的最小水平距离")]
    public float minDistanceToFilp = 0.1f;

	public override TaskStatus OnUpdate()
	{
		var scale = transform.localScale;
		float xDistance = faceT.Value.position.x - transform.position.x;
		if (Mathf.Abs(xDistance) > minDistanceToFilp)
		{
			scale.x = xDistance > 0 ? 1 : -1;
			transform.localScale = scale;
		}
		return TaskStatus.Success;
	}
}