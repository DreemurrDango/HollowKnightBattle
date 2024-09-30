using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

/// <summary>
/// 大黄蜂扔针攻击中针的行为
/// </summary>
public class HornetNeedle : MonoBehaviour
{
    [HideInInspector,Tooltip("针返回至所发出角色时的事件回调")]
    public UnityEvent onNeedleBack;
    [Tooltip("被扔出后的回旋加速度，应为正值，方向总是向着大黄蜂方向")]
    public float boomerangForce = 10f;
    [SerializeField, Tooltip("针后连接线的精灵渲染器，用于控制精灵长度")]
    private SpriteRenderer threadRender;
    [SerializeField, Tooltip("针的动画器，用于控制针的动画状态")]
    private Animator needleAnimator;
    [SerializeField, Tooltip("针后连接线的动画器，用于控制线的动画状态")]
    private Animator threadAnimator;

    /// <summary>
    /// 所有者的形变
    /// </summary>
    private Transform ownerT;
    /// <summary>
    /// 所属生成器组件
    /// </summary>
    private Spawner spawner;
    /// <summary>
    /// 2D刚体组件
    /// </summary>
    private Rigidbody2D rigidbody2D;
    /// <summary>
    /// 回旋力
    /// </summary>
    private Vector2 backForce;
    /// <summary>
    /// 初始力方向
    /// </summary>
    private Vector2 originDirection;

    /// <summary>
    /// 当前针是否处于返程过程中
    /// </summary>
    public bool InBacking => rigidbody2D.velocity.x * originDirection.x < 0;

    /// <summary>
    /// 生成时初始化
    /// </summary>
    /// <param name="s">生成该对象的生成器组件</param>
    /// <param name="owner">所属敌人对象的形变</param>
    /// <param name="originForce">初始力</param>
    public void Init(Spawner s,Transform owner,Vector2 originForce)
    {
        spawner = s;
        ownerT = owner;
        originDirection = originForce.normalized;
        backForce = originDirection * boomerangForce * -1f;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(originForce);
    }

    private void Update()
    {
        //调整针后线的长度
        Vector2 s = threadRender.size;
        s.x = Mathf.Abs(transform.position.x - spawner.transform.position.x);
        threadRender.size = s;
        threadAnimator.SetFloat("forwardVelocity", rigidbody2D.velocity.x * originDirection.x);
        needleAnimator.SetFloat("horizontalVelocity", Mathf.Abs(rigidbody2D.velocity.x));
    }

    private void FixedUpdate()
    {
        //持续施加力，模拟类似回旋至原拥有者的效果
        rigidbody2D.AddForce(backForce * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //回旋过程中触碰到发出者时，扔针动作完成，销毁此对象并通知拥有者
        if(collision.gameObject.transform == ownerT && InBacking)
        {
            onNeedleBack?.Invoke();
            Destroy(gameObject);
        }
    }
}