using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// 判断目标形变相对于源形变的位置是否为重合/上方/下方/左侧/右侧
/// </summary>
[TaskName("相对位置判断")]
public class RelativePositionCompare : Conditional
{
    [TT("目标形变")]
    public SharedTransform aimT;
    [TT("目标绝对位置，若指定了目标形变则该值作为偏移值")]
    public SharedVector2 aimPos;
    [TT("起点形变，若不指定则为挂载当前行为树组件的形变")]
	public SharedTransform originT;
    [TT("相对位置判断，判断目标形变/位置是否在起点位置的对应方位")]
    public RelativePostionComparison.RelativePostion relativePostion;
    [TT("是否对结果取反")]
    public bool invertResult = false;

    public override void OnAwake()
    {
        if (originT.Value == null) originT.Value = transform;
    }

    public override TaskStatus OnUpdate()
	{
        Vector2 comparePos = aimT.Value != null ? (Vector2)aimT.Value.position + aimPos.Value : aimPos.Value;
        bool result = RelativePostionComparison.InRelativePostion(comparePos, originT.Value.position, relativePostion);
        if (invertResult) result = !result;
        return result ? TaskStatus.Success : TaskStatus.Failure;
	}
}