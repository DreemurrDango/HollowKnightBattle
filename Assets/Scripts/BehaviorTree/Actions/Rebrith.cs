using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskDescription("������ˣ���Ҫ���������ϻָ���ű���������������ֵ")]
public class Rebrith : Action
{
	[TT("Ҫ����ĵ��˽ű����ڵ���Ϸ�������ÿ���ӵ�ǰ��Ϊ�����ڶ�����Ѱ��Ŀ�����")]
	public SharedGameObject enemyGO;
    [TT("������������ֵ��������Ϊ0��ظ���ԭ����ֵ����")]
    public SharedFloat newHealth;

    /// <summary>
    /// ���˽ű�
    /// </summary>
    private Enemy enemy;

    public override void OnAwake()
    {
		if(enemyGO.Value == null) enemy = GetComponent<Enemy>();
		else enemy = enemyGO.Value.GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate()
	{
		enemy.Rebrith(newHealth.Value);
		return TaskStatus.Success;
	}
}