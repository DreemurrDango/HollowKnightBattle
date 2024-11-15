using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using TT = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskCategory("Unity/Animator")]
[TaskName("�ȴ������������")]
[TaskDescription("�ȴ�ָ������״̬�Ķ������һ�β��ţ�һ�����ڵȴ���ѭ������״̬���Ž���")]
public class WaitForAnimationCompleted : Action
{
    [TT("Ҫ���Ķ�������λ�ڵ���Ϸ��������ָ����Ĭ��Ϊ��Ϊ�����ڶ����ϵĶ��������")]
    public SharedGameObject targetGameObject;
    [TT("Ҫ���Ķ���״̬��������Ϊ�ս��ȴ���ǰ����״̬���н���")]
    public string stateName;
    [TT("��״̬����ָ������ͬʱ�Ƿ�����ȴ�������Ϊfalse����״̬��ͬʱֱ�ӷ���ʧ��")]
    public bool waitOnStateDiffer;

    private Animator animator;
    private int stateHash;

    public override void OnAwake()
    {
        animator = (targetGameObject.Value != null ? targetGameObject.Value : Owner.gameObject).GetComponent<Animator>();
        if (stateName != "")
        {
            stateHash = Animator.StringToHash(stateName);
            if(!animator.HasState(0, Animator.StringToHash(stateName)))
                Debug.LogError("�������ϲ�������Ϊ�� " + stateName + "���Ķ���״̬");
        }
    }

    public override TaskStatus OnUpdate()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateName == "") return stateInfo.normalizedTime >= 1 ? TaskStatus.Success : TaskStatus.Running;
        else if (stateInfo.shortNameHash != stateHash) return waitOnStateDiffer ? TaskStatus.Running : TaskStatus.Failure;
        else return stateInfo.normalizedTime >= 0.85f ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnReset()
    {
        targetGameObject = null;
        stateName = "";
    }
}
