using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Cinemachine;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("�������")]
/// <summary>
/// ������Դ�ķ������������������
/// </summary>
public class ShakeCamera : Action
{
    [TT("��Դ������ڵ���Ϸ����")]
    public SharedGameObject impulseSourceGO;
    [TT("�𶯷���")]
	public Vector2 direction;
    [TT("������")]
    public float force;
    /// <summary>
    /// ���ս�ʹ�õ���Դ���
    /// </summary>
    private CinemachineImpulseSource impulseSource;

    public override void OnAwake()
    {
        impulseSource = impulseSourceGO.Value?.GetComponent<CinemachineImpulseSource>();
        if (impulseSource == null) Debug.LogWarning("δָ����Դ�����");
    }

    public override TaskStatus OnUpdate()
	{
        impulseSource.GenerateImpulse(force * direction);
		return TaskStatus.Success;
	}
}