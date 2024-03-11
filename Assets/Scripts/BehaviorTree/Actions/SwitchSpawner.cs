using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskName("���ض���������")]
/// <summary>
/// ���ض������ɶ���
/// </summary>
public class SwitchSpawner : Action
{
	[TT("Ҫ����������������")]
	public SharedGameObject spawnerGO;
    [TT("������ر�ָ��")]
    public bool offOrOn;

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
		if(spawner.InSpawning != offOrOn)
        {
            if (offOrOn) spawner.Play();
            else spawner.Stop();
        }
		return TaskStatus.Success;
	}
}