using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("��������")]
/// <summary>
/// ͨ���ı������α��localScale.xΪ1/-1.���������ָ�����α�
/// </summary>
public class FaceTo : Action
{
	[TT("Ҫ������α�")]
	public SharedTransform faceT;
    [TT("�Ƿ�Խ��ȡ��")]
    public bool doInvert = false;
	[Min(0.1f)]
    [TT("ת��õ�λ��Ҫ�ﵽ����Сˮƽ����")]
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