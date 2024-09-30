using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// �ж����������Ƿ�����ִ��ĳһ��������ǰ���й�����
/// </summary>
[TaskName("�����Ƿ����ڹ���")]
public class IsDoingAttack : Conditional
{
    /// <summary>
    /// Ҫ��ȡ�ĵ������
    /// </summary>
    private CharaController charaController;

    public override void OnAwake()
    {
        charaController = CharaController.Instance;
    }

    public override TaskStatus OnUpdate()
    {
        return charaController.InDoingAttack ? TaskStatus.Success : TaskStatus.Failure;
    }
}