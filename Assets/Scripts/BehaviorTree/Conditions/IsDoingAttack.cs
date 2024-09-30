using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 判断主角现在是否正在执行某一动作（当前仅有攻击）
/// </summary>
[TaskName("主角是否正在攻击")]
public class IsDoingAttack : Conditional
{
    /// <summary>
    /// 要获取的敌人组件
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