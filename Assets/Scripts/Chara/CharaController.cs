using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using BehaviorDesigner.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// 主角控制核心脚本
/// </summary>
public class CharaController : Singleton<CharaController>
{
    /// <summary>
    /// 当前的跳跃状态
    /// </summary>
    public enum JumpState
    {
        [EnumName("未处在跳跃中")]
        noJump,
        [EnumName("正处在跳跃的上升阶段")]
        inRising,
        [EnumName("正处在跳跃的下落阶段")]
        inFalling
    }

    [Header("控制属性")]
    [Header("-移动")]
    [SerializeField]
    [Tooltip("最大速度")]
    private float fullSpeed = 1f;
    [SerializeField]
    [Tooltip("速度从0加速打最大需要的时间")]
    private float t_accelerateToFullSpeed = 0.5f;
    [SerializeField]
    [Range(0f,1f)]
    [Tooltip("摇杆操纵下，角色移动的最小值")]
    private float StrickMoveThreshold = 0.15f;

    [Header("-攻击")]
    [SerializeField]
    [Tooltip("攻击间隔冷却时间")]
    private float attackCoolDown = 0.4f;
    [SerializeField]
    [Tooltip("攻击力系数")]
    private float damageFactor = 1f;
    [SerializeField]
    [Tooltip("攻击特效产生的根形变位置")]
    private Transform attackRootT;
    [SerializeField]
    [Tooltip("奔跑时产生的攻击位置最大水平偏移量")]
    private float maxAttackRootOffset;
    [SerializeField]
    [Tooltip("攻击时要播放的音效信息名")]
    private string attackSEName;

    [Header("-跳跃")]
    [SerializeField]
    [Tooltip("最大跳跃次数")]
    private int jumpTimes = 1;
    [SerializeField]
    [Tooltip("跳跃落地后的冷却时间")]
    private float jumpCoolDown = 0.25f;
    [SerializeField]
    [Tooltip("跳跃的输入按住时间区间，结构为（按住后开始计算额外跳跃高度的最小输入时间，达到最大跳跃高度需要的时间）")]
    private Vector2 jumpInputTime;
    [SerializeField]
    [Tooltip("跳跃最大上升力区间，结构为（短按时的最小跳跃力，长按达到最大时间后的跳跃力）")]
    private Vector2 jumpInputForce;
    [SerializeField]
    [Tooltip("跳跃落地时要播放的音效信息名")]
    private string landSEName;

    [Header("-受伤")]
    [SerializeField]
    [Tooltip("每次攻击命中时会受到的最小伤害")]
    private int minDamageTake = 1;
    [SerializeField]
    [Tooltip("伤害转换为额外伤害的比例")]
    private float damageToTrueDamgeRatio = 50f;
    [SerializeField]
    [Tooltip("受到伤害时的效果力：其中X轴的值需为正数，在发生伤害时由具体伤害方向决定")]
    private Vector2 damageTakeForce;
    [SerializeField]
    [Tooltip("受到伤害后的保护时间")]
    private float damageProtectTime = 1f;
    [SerializeField]
    [Tooltip("受到伤害后的角色频闪效果频率")]
    private int damageProtectEffectTimes = 5;
    [SerializeField]
    [Tooltip("受到伤害后的操作锁定时间")]
    private float ControlLockTimeAfterDamage = 0.25f;
    [SerializeField]
    [Tooltip("受到伤害后要播放的音效信息")]
    private string beHitSEName;
    [HideInInspector]
    [Tooltip("受到伤害后失去生命值事件")]
    public UnityEvent afterLoseHealth;

    [Header("组件信息")]
    [SerializeField]
    [Tooltip("角色的刚体组件")]
    private Rigidbody2D rigidbody;
    [SerializeField]
    [Tooltip("当前正在面对的BOSS敌人")]
    private Enemy bossEnemy;
    [SerializeField]
    [Tooltip("死亡后仍存在作为敌人攻击残影的形变")]
    private Transform staticStayT;
    [SerializeField]
    [Tooltip("角色的精灵渲染器")]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    [Tooltip("角色的动画器组件")]
    private Animator animator;
    [SerializeField]
    [Tooltip("攻击粒子效果的预制体组件列表，应与攻击动画的后缀一致")]
    private List<AttackEffect> attackEffectPrefabs;
    [SerializeField]
    [Tooltip("显示角色剩余生命值的")]
    private List<GameObject> charaLivesShowGO;
    [SerializeField]
    [Tooltip("受到攻击时显示受击动画特效的组件")]
    private CharaBeHitShowUI beHitShowUI;
    [SerializeField]
    [Tooltip("显示效果上的中心位置形变")]
    private Transform showCentreT;


    /// <summary>
    /// 攻击剩余冷却时间
    /// </summary>
    private float t_attackCD = 0f;
    /// <summary>
    /// 剩余可用的跳跃次数
    /// </summary>
    private int availableJumpTimes;
    /// <summary>
    /// 跳跃动作的剩余冷却时间
    /// </summary>
    private float t_jumpCD = 0f;
    /// <summary>
    /// 要播放的攻击动画序号（从0开始）
    /// </summary>
    private int attackAnimIndex = 0;
    
    /// <summary>
    /// 当前是否正按住跳跃键
    /// </summary>
    private bool doJumpButtonHold = false;
    /// <summary>
    /// 当前输入的水平速度，只会为 -1/0/1
    /// </summary>
    private float horizontalInput = 0f;
    /// <summary>
    /// 跳跃键已按住的时间
    /// </summary>
    private float t_jumpInputHold = 0f;
    /// <summary>
    /// 是否收到跳跃命令
    /// </summary>
    private bool doGetJumpInput = false;
    /// <summary>
    /// 是否受到攻击命令
    /// </summary>
    private bool doGetAttackInput = false;

    /// <summary>
    /// 伤害保护计时器
    /// </summary>
    private float t_damageProtect = 0f;
    [SerializeField,DisplayOnly]
    /// <summary>
    /// 受伤后控制锁定计时器
    /// </summary>
    private float t_controlLock = 0f;
    /// <summary>
    /// 角色当前生命数
    /// </summary>
    private int currentLives;
    /// <summary>
    /// 当前正在交战的BOSS敌人的行为树
    /// </summary>
    private BehaviorTree bossEnemyTree;
    /// <summary>
    /// 主角形体主碰撞体
    /// </summary>
    private Collider2D mainCollider2D;

    /// <summary>
    /// 获得主角的中心形变
    /// </summary>
    public Transform GetCentreT => showCentreT;
    /// <summary>
    /// 是否正处于伤害保护中
    /// </summary>
    public bool InDamageProtect => t_damageProtect > 0;
    /// <summary>
    /// 当前是否正处于伤害保护中
    /// </summary>
    public bool InDamageLock => t_controlLock > 0;
    /// <summary>
    /// 当前是否可以进行跳跃
    /// </summary>
    public bool CanJump => availableJumpTimes > 0;
    /// <summary>
    /// 当前是否正在进行攻击
    /// </summary>
    public bool InDoingAttack => t_attackCD > 0;
    /// <summary>
    /// 当前是否可以攻击
    /// </summary>
    public bool CanAttack => !(t_attackCD > 0);

    public void OnHorizontalMove(CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
        if (horizontalInput > StrickMoveThreshold) horizontalInput = 1f;
        else if (horizontalInput < StrickMoveThreshold * -1) horizontalInput = -1f;
        else horizontalInput = 0f;
    }
    public void OnJumpButtonDown(CallbackContext context)
    {
        if (context.started && !doJumpButtonHold)
        {
            t_jumpInputHold = 0f;
            doJumpButtonHold = true;
        }
        else if (context.canceled && doJumpButtonHold)
        {
            doJumpButtonHold = false;
            doGetJumpInput = true;
        }
    }
    public void OnAttackButtonDown(CallbackContext context) { if (context.started) doGetAttackInput = true; }

    private void Start()
    {
        t_attackCD = 0f;
        availableJumpTimes = jumpTimes;
        t_jumpCD = 0f;
        doJumpButtonHold = false;
        horizontalInput = 0f;
        doGetJumpInput = false;
        doGetAttackInput = false;
        t_damageProtect = 0f;
        t_controlLock = 0f;
        currentLives = charaLivesShowGO.Capacity;
        mainCollider2D = rigidbody.GetComponent<Collider2D>();
        bossEnemyTree = bossEnemy.GetComponent<BehaviorTree> ();
    }

    private void Update()
    {
        // 控制锁定计时器
        if (t_controlLock > 0f) t_controlLock -= Time.unscaledDeltaTime;
        // 伤害保护计时器
        if (t_damageProtect > 0f) t_damageProtect -= Time.unscaledDeltaTime;
        // 每帧的水平移动
        if (!InDamageLock && horizontalInput != 0f)
        {
            rigidbody.transform.localScale = new Vector3(horizontalInput,1,1);
            Vector2 speed = rigidbody.velocity;
            speed.x += horizontalInput * Time.deltaTime / t_accelerateToFullSpeed * fullSpeed;
            if (speed.x > fullSpeed) speed.x = fullSpeed;
            else if (speed.x < fullSpeed * -1) speed.x = fullSpeed * -1;
            rigidbody.velocity = speed;
        }
        float horizontalSpeed = Mathf.Abs(rigidbody.velocity.x);
        animator.SetFloat("horizontalSpeed", horizontalSpeed);
        // 每帧的跳跃判断
        //-跳跃输入计时器更新
        if (doJumpButtonHold && !doGetJumpInput)
        {
            //按住跳跃键时间达到最大跳跃高度所需的时长时，自动跳跃
            t_jumpInputHold += Time.deltaTime;
            if(t_jumpInputHold > jumpInputTime.y)
            {
                doJumpButtonHold = false;
                doGetJumpInput = true;
            }
        }
        //-发生跳跃动作输入时，判断是否有效
        if (doGetJumpInput)
        {
            if(!InDamageLock && CanJump)
            {
                float t = Mathf.Clamp(t_jumpInputHold, jumpInputTime.x, jumpInputTime.y);
                t = (t - jumpInputTime.x) / (jumpInputTime.y - jumpInputTime.x);
                Vector2 upForce = Vector2.up * Mathf.Lerp(jumpInputForce.x, jumpInputForce.y, t);
                rigidbody.AddForce(upForce);
                availableJumpTimes--;
            }
            doGetJumpInput = false;
        }
        //-跳跃动作中结算
        animator.SetFloat("verticalSpeed", rigidbody.velocity.y);
        // 每帧的攻击动作判断
        if (t_attackCD > 0) t_attackCD -= Time.deltaTime;
        if (!InDamageLock && doGetAttackInput && CanAttack) 
        {
            var randomNum = Random.Range(0, attackEffectPrefabs.Count);
            //攻击特效产生形变位置根据当前水平速度产生偏移，全速奔跑时会使得攻击位置靠前
            attackRootT.localPosition = new Vector3(horizontalSpeed * maxAttackRootOffset / fullSpeed, 0f, 0f);
            //播放攻击音效
            AudioManager.Instance.PlaySE(attackSEName, transform);
            //生成攻击效果实例
            var ae = Instantiate(attackEffectPrefabs[randomNum].gameObject, attackRootT).GetComponent<AttackEffect>();
            ae.Init(gameObject, damageFactor);
            //循环播放攻击动作动画
            attackAnimIndex = (attackAnimIndex + 1) % 2;
            animator.SetFloat("attackAnimIndex", attackAnimIndex);
            animator.SetTrigger("doAttack");
            //攻击内置CD
            t_attackCD = attackCoolDown;
            doGetAttackInput = false;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;
        var collider = collision.collider;
        switch (go.tag)
        {
            //与可跳跃的平台碰撞时回复跳跃能力
            case "Platform":
                //if (jumpState != JumpState.inFalling) break;
                if (rigidbody.transform.position.y + 0.2f > collider.bounds.max.y)
                    availableJumpTimes = jumpTimes;
                AudioManager.Instance.PlaySE(landSEName, transform);
                break;
        }
    }

    /// <summary>
    /// 玩家角色受到伤害时效果
    /// </summary>
    /// <param name="hitGO">伤害玩家的对象</param>
    /// <param name="damage">受到的伤害量</param>
    public void OnBeHit(GameObject hitGO,float damage)
    {
        if (InDamageProtect) return;
        //受到伤害量结算
        int finalDamage = minDamageTake + (int)(damage / damageToTrueDamgeRatio);
        currentLives -= finalDamage;
        afterLoseHealth?.Invoke();
        //剩余生命值显示
        for (int i = 0; i < charaLivesShowGO.Count; i++) 
            charaLivesShowGO[i].SetActive(i < currentLives);
        //受伤后后退力
        Vector2 finalForce = damageTakeForce;
        finalForce.x *= hitGO.transform.position.x > transform.position.x ? -1 : 1;
        rigidbody.AddForce(finalForce);
        //受到伤害后慢放
        StartCoroutine(TestManager.Instance.TimeSlowDownBriefly(0.5f, 1f));
        //受击UI动画
        beHitShowUI.PlayBeHitEffectUIAnimation();
        //控制锁定
        t_controlLock = ControlLockTimeAfterDamage;
        if (currentLives > 0)
        {
            //伤害保护
            t_damageProtect = damageProtectTime;
            var s = DOTween.Sequence();
            float interval = damageProtectTime / damageProtectEffectTimes / 2;
            for (int i = 0; i < damageProtectEffectTimes; i++)
            {
                s.Append(spriteRenderer.DOFade(0.5f, interval));
                s.Append(spriteRenderer.DOFade(1f, interval));
            }
            s.Play();
            //受伤后短暂地无视与敌人的碰撞
            StartCoroutine(IgnoreCollisonWithEnemyTemp("Enemy", damageProtectTime));
            IEnumerator IgnoreCollisonWithEnemyTemp(string aimLayerName, float ignoreTime)
            {
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(aimLayerName), true);
                yield return new WaitForSeconds(ignoreTime);
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(aimLayerName), false);
            }
            // 被击中时播放受伤音效
            string seName = beHitSEName + (Random.value > 0.5f ? "1" : "2");
            AudioManager.Instance.PlaySE(seName, transform);
            AudioManager.Instance.SwitchSnapShotTemp("MainCharaBeHit", damageProtectTime);
        }
        // 生命值掉完时的特殊处理
        else
        {
            t_controlLock = 99f;
            t_damageProtect = 99f;
            //死亡后可掉出屏幕
            mainCollider2D.isTrigger = true;
            //分离残影，供BOSS敌人继续行为树动作
            staticStayT.SetParent(null, true);
            bossEnemyTree.SetVariableValue("BattleTargetT", staticStayT);
            // 被击中且死亡时播放受伤音效
            string seName = beHitSEName + (Random.value > 0.5f ? "1" : "2");
            AudioManager.Instance.PlaySE(seName, transform);
            AudioManager.Instance.SwitchSnapShot("MainCharaBeHit");
            //游戏失败结算
            GameManager.Instance.GameEnd(false);
            StartCoroutine(GameEnd_DefeatCoroutine(2f));

            IEnumerator GameEnd_DefeatCoroutine(float waitTime)
            {
                yield return new WaitForSeconds(waitTime);
                rigidbody.Sleep();
            }
        }
    }

    /// <summary>
    /// 设置控制锁定，指定时间内角色无法接受控制
    /// </summary>
    /// <param name="t">控制锁定时间</param>
    public void SetControlLock(float t = 999) => t_controlLock = t;
}
