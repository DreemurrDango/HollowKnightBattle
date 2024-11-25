using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using BehaviorDesigner.Runtime;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("初始生命值")]
    private float originHealth = 100f;
    [SerializeField]
    [Tooltip("设置是否可被攻击命中")]
    private bool canBeHit = true;
    [SerializeField,DisplayOnly]
    [Tooltip("是否正处于僵直状态，此状态下的敌人无攻击能力且可被攻击，但攻击不再计为连击")]
    private bool inLimpState = false;
    [SerializeField, DisplayOnly]
    [Tooltip("生命值")]
    private float health;
    [HideInInspector]
    [Tooltip("失去生命值时事件")]
    public UnityEvent afterLoseHealth;
    [DisplayOnly,SerializeField]
    [Tooltip("当前是否在地面上")]
    private bool onGround;

    [Header("相关组件")]
    [SerializeField]
    [Tooltip("动画器组件")]
    private Animator animator;
    [SerializeField]
    [Tooltip("精灵渲染器")]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    [Tooltip("该敌人的行为树组件")]
    private BehaviorTree behaviorTree;
    [SerializeField]
    [Tooltip("该敌人的碰撞伤害器")]
    private AttackEffect colliderAttackEffect;

    [Header("-被击中时效果")]
    [SerializeField]
    [Tooltip("被攻击时的粒子特效，若不设置则不进行显示")]
    private ParticleSystem hitParticleEffect;
    [SerializeField]
    [Tooltip("该敌人受伤时受到的击退力效果")]
    private Vector2 hitBackForce = Vector2.zero;
    [SerializeField]
    [Tooltip("被攻击时动画触发词，若置空则不进行显示")]
    private string beHitTrigger;
    [SerializeField]
    [Tooltip("被攻击时播放音效信息名，若置空则不进行显示")]
    private string beHitSEName;
    [DisplayOnly, SerializeField]
    [Tooltip("自上一次战斗僵直后累计被击中的次数")]
    private int beHitCount;
    [Header("--连击")]
    [SerializeField]
    [Tooltip("连击时间间隔，超过该时间受击连击中断")]
    private float t_beHitComboInterval = 2f;
    [DisplayOnly, SerializeField]
    [Tooltip("已连击次数")]
    private int beHitComboCount;

    [Header("-死亡时效果")]
    [SerializeField]
    [Tooltip("是否在该敌人死亡时清除水平速度")]
    private bool clearXVelocity;
    [SerializeField]
    [Tooltip("是否在该敌人死亡时清除垂直速度")]
    private bool clearYVelocity;
    [SerializeField]
    [Tooltip("该敌人死亡时受到力效果")]
    private Vector2 dieForce = Vector2.zero;
    [SerializeField]
    [Tooltip("被攻击时播放音效信息名，若置空则不进行显示")]
    private string onDiedSEName;
    [Range(0.1f,1f)]
    [SerializeField]
    [Tooltip("该敌人死亡时的慢镜头时间倍率")]
    private float onDiedTimeScale = 0.5f;
    [SerializeField]
    [Tooltip("该敌人死亡时的慢镜头效果持续时间，为0时无慢镜头效果")]
    private float onDiedSlowDownLast = 0.4f;

    /// <summary>
    /// 刚体组件
    /// </summary>
    private Rigidbody2D rigidbody2D;
    /// <summary>
    /// 近期伤害了该单位的攻击特效
    /// </summary>
    private AttackEffect hitEffect;
    /// <summary>
    /// 上一次受击的时间戳
    /// </summary>
    private float t_lastHitTime;
    /// <summary>
    /// 特殊的精灵渲染器
    /// </summary>
    private Material material;

    /// <summary>
    /// 该对象当前是否可被攻击命中
    /// </summary>
    public bool CanBeHit { get => canBeHit; set => canBeHit = value; }
    /// <summary>
    /// 敌人的生命值
    /// </summary>
    public int Health => (int)health;
    /// <summary>
    /// 敌人的生命值上限
    /// </summary>
    public int FullHealth => (int)originHealth;
    /// <summary>
    /// 当前敌人是否仍存活
    /// </summary>
    public bool IsAlive => health > 0;
    /// <summary>
    /// 该敌人当前是否面向右侧
    /// </summary>
    public bool DoFaceRight => transform.localScale.x > 0;
    /// <summary>
    /// 上一次僵直后被攻击的总次数，用于进行判断
    /// </summary>
    public int BeHitCount { get => beHitCount; set => beHitCount = value; }
    /// <summary>
    /// 被连击次数
    /// </summary>
    public int BeHitComboCount => beHitComboCount;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        health = originHealth;
        beHitCount = 0;
        beHitComboCount = 0;
        material = spriteRenderer.material;
    }

    private void Update()
    {
        if(rigidbody2D != null)
        {
            animator.SetFloat("horizontalSpeed", Mathf.Abs(rigidbody2D.velocity.x));
            animator.SetFloat("verticalSpeed", rigidbody2D.velocity.y);
            animator.SetBool("onGround", onGround);
        }
    }

    /// <summary>
    /// 受到伤害效果结算
    /// </summary>
    /// <param name="damage">受到的伤害量</param>
    public void OnBeHit(AttackEffect effect)
    {
        behaviorTree.SendEvent("BeHit");
        if (!IsAlive) return;
        if (!CanBeHit) return;
        //非硬直状态下受击次数统计
        if (!inLimpState) beHitCount++;
        //非硬直状态下连击次数统计
        if(!inLimpState)
        {
            if (Time.time - t_lastHitTime < t_beHitComboInterval) beHitComboCount++;
            else beHitComboCount = 1;
            t_lastHitTime = Time.time;
        }
        //受击后退力
        if (hitBackForce != Vector2.zero)
        {
            Vector2 v = effect.transform.position - transform.position;
            rigidbody2D.AddForce( v * -1 * hitBackForce);
        }
        //计算伤害
        health -= effect.FinalDamage;
        behaviorTree.SendEvent<float>("BeHurt",effect.FinalDamage);
        afterLoseHealth?.Invoke();
        hitEffect = effect;
        ShowHitColorEffectBriefly(0.1f);
        //受伤动画机更新
        if (beHitTrigger != "") animator.SetTrigger(beHitTrigger);
        if (health <= 0) OnDied();
        else if(beHitSEName != "") AudioManager.Instance.PlaySE(beHitSEName, transform);


        //受击变色特效线程
        void ShowHitColorEffectBriefly(float t)
        {
            Sequence sequence = DOTween.Sequence();
            int sid = Shader.PropertyToID("_HitEffectBlend");
            sequence.Append(material.DOFloat(1f, sid, 0.1f))
                .AppendInterval(t)
                .Append(material.DOFloat(0f, sid, 0.1f));
            sequence.Play();
        }
    }

    /// <summary>
    /// 死亡时动作
    /// </summary>
    private void OnDied()
    {
        health = 0;
        if(colliderAttackEffect != null) colliderAttackEffect.enabled = false;
        if (onDiedSEName != "") AudioManager.Instance.PlaySE(onDiedSEName, transform);
        //慢放
        if (onDiedSlowDownLast > 0f && onDiedTimeScale !=  1)
            StartCoroutine(GameManager.TimeSlowDownBriefly(onDiedTimeScale, onDiedSlowDownLast));
        // 若设置掐停则清楚当前速度
        Vector2 v = rigidbody2D.velocity;
        if (clearXVelocity) v.x = 0;
        if (clearYVelocity) v.y = 0;
        rigidbody2D.velocity = v;
        // 若设置死亡力则进行施力
        Vector2 f = dieForce;
        if (dieForce.x != 0 && hitEffect.transform.position.x > transform.position.x)
            f.x *= -1;
        if (f != Vector2.zero) rigidbody2D.AddForce(f);
    }

    /// <summary>
    /// 恢复生命值
    /// </summary>
    /// <param name="value">要恢复的生命值量，若不指定则恢复为满生命值</param>
    public void RecoverHealth(float value = 0)
    {
        if (value == 0) value = originHealth - health;
        health += value;
    }

    /// <summary>
    /// 该敌人复活，回复当前生命值至新的生命值上限
    /// </summary>
    /// <param name="newOriginHealth">新的生命值上限</param>
    public void Rebrith(float newOriginHealth = 0)
    {
        if (newOriginHealth == 0) newOriginHealth = originHealth;
        originHealth = newOriginHealth;
        health = newOriginHealth;
        CanBeHit = true;
    }

    /// <summary>
    /// 设置可被攻击属性的值
    /// </summary>
    /// <param name="value"></param>
    public void SetCanBeHit(float value) => CanBeHit = value != 0;

    /// <summary>
    /// 设置硬直状态变化
    /// </summary>
    /// <param name="exitOrEnter">F：退出硬直状态，T：进入</param>
    public void SetLimpState(bool exitOrEnter)
    {
        if(inLimpState == exitOrEnter) return;
        if (exitOrEnter) beHitCount = beHitComboCount = 0;
        inLimpState = exitOrEnter;
    }

    /// <summary>
    /// 转面向至另一侧
    /// </summary>
    public void TurnAround()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) onGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) onGround = false;
    }
}
