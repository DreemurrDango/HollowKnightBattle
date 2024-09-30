using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("�������ɶ���")]
/// <summary>
/// ��������������һ�ζ���
/// </summary>
public class SpawnerPlayOnce : Action
{
	[TT("Ҫ��������������������ָ�������Ϊ�����ڶ�����Ѱ��")]
	public SharedGameObject spawnerGO;

    /// <summary>
    /// ���������
    /// </summary>
    private Spawner spawner;

    public override void OnAwake()
    {
        spawner = spawnerGO.Value?.GetComponent<Spawner>();
        if (spawner == null) Debug.LogWarning("δָ�������������");
    }

    public override TaskStatus OnUpdate()
	{
		spawner.PlayOnce();
		return TaskStatus.Success;
	}
}