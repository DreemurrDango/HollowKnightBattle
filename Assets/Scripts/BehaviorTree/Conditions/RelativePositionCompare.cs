using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enums;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// �ж�Ŀ���α������Դ�α��λ���Ƿ�Ϊ�غ�/�Ϸ�/�·�/���/�Ҳ�
/// </summary>
[TaskName("���λ���ж�")]
public class RelativePositionCompare : Conditional
{
    [TT("Ŀ���α�")]
    public SharedTransform aimT;
    [TT("Ŀ�����λ�ã���ָ����Ŀ���α����ֵ��Ϊƫ��ֵ")]
    public SharedVector2 aimPos;
    [TT("����α䣬����ָ����Ϊ���ص�ǰ��Ϊ��������α�")]
	public SharedTransform originT;
    [TT("���λ���жϣ��ж�Ŀ���α�/λ���Ƿ������λ�õĶ�Ӧ��λ")]
    public RelativePostionComparison.RelativePostion relativePostion;
    [TT("�Ƿ�Խ��ȡ��")]
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