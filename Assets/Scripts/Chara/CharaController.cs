using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using BehaviorDesigner.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// ���ǿ��ƺ��Ľű�
/// </summary>
public class CharaController : Singleton<CharaController>
{
    /// <summary>
    /// ��ǰ����Ծ״̬
    /// </summary>
    public enum JumpState
    {
        [EnumName("δ������Ծ��")]
        noJump,
        [EnumName("��������Ծ�������׶�")]
        inRising,
        [EnumName("��������Ծ������׶�")]
        inFalling
    }

    [Header("��������")]
    [Header("-�ƶ�")]
    [SerializeField]
    [Tooltip("����ٶ�")]
    private float fullSpeed = 1f;
    [SerializeField]
    [Tooltip("�ٶȴ�0���ٴ������Ҫ��ʱ��")]
    private float t_accelerateToFullSpeed = 0.5f;
    [SerializeField]
    [Range(0f,1f)]
    [Tooltip("ҡ�˲����£���ɫ�ƶ�����Сֵ")]
    private float StrickMoveThreshold = 0.15f;

    [Header("-����")]
    [SerializeField]
    [Tooltip("���������ȴʱ��")]
    private float attackCoolDown = 0.4f;
    [SerializeField]
    [Tooltip("������ϵ��")]
    private float damageFactor = 1f;
    [SerializeField]
    [Tooltip("������Ч�����ĸ��α�λ��")]
    private Transform attackRootT;
    [SerializeField]
    [Tooltip("����ʱ�����Ĺ���λ�����ˮƽƫ����")]
    private float maxAttackRootOffset;
    [SerializeField]
    [Tooltip("����ʱҪ���ŵ���Ч��Ϣ��")]
    private string attackSEName;

    [Header("-��Ծ")]
    [SerializeField]
    [Tooltip("�����Ծ����")]
    private int jumpTimes = 1;
    [SerializeField]
    [Tooltip("��Ծ��غ����ȴʱ��")]
    private float jumpCoolDown = 0.25f;
    [SerializeField]
    [Tooltip("��Ծ�����밴סʱ�����䣬�ṹΪ����ס��ʼ���������Ծ�߶ȵ���С����ʱ�䣬�ﵽ�����Ծ�߶���Ҫ��ʱ�䣩")]
    private Vector2 jumpInputTime;
    [SerializeField]
    [Tooltip("��Ծ������������䣬�ṹΪ���̰�ʱ����С��Ծ���������ﵽ���ʱ������Ծ����")]
    private Vector2 jumpInputForce;
    [SerializeField]
    [Tooltip("��Ծ���ʱҪ���ŵ���Ч��Ϣ��")]
    private string landSEName;

    [Header("-����")]
    [SerializeField]
    [Tooltip("ÿ�ι�������ʱ���ܵ�����С�˺�")]
    private int minDamageTake = 1;
    [SerializeField]
    [Tooltip("�˺�ת��Ϊ�����˺��ı���")]
    private float damageToTrueDamgeRatio = 50f;
    [SerializeField]
    [Tooltip("�ܵ��˺�ʱ��Ч����������X���ֵ��Ϊ�������ڷ����˺�ʱ�ɾ����˺��������")]
    private Vector2 damageTakeForce;
    [SerializeField]
    [Tooltip("�ܵ��˺���ı���ʱ��")]
    private float damageProtectTime = 1f;
    [SerializeField]
    [Tooltip("�ܵ��˺���Ľ�ɫƵ��Ч��Ƶ��")]
    private int damageProtectEffectTimes = 5;
    [SerializeField]
    [Tooltip("�ܵ��˺���Ĳ�������ʱ��")]
    private float ControlLockTimeAfterDamage = 0.25f;
    [SerializeField]
    [Tooltip("�ܵ��˺���Ҫ���ŵ���Ч��Ϣ")]
    private string beHitSEName;
    [HideInInspector]
    [Tooltip("�ܵ��˺���ʧȥ����ֵ�¼�")]
    public UnityEvent afterLoseHealth;

    [Header("�����Ϣ")]
    [SerializeField]
    [Tooltip("��ɫ�ĸ������")]
    private Rigidbody2D rigidbody;
    [SerializeField]
    [Tooltip("��ǰ������Ե�BOSS����")]
    private Enemy bossEnemy;
    [SerializeField]
    [Tooltip("�������Դ�����Ϊ���˹�����Ӱ���α�")]
    private Transform staticStayT;
    [SerializeField]
    [Tooltip("��ɫ�ľ�����Ⱦ��")]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    [Tooltip("��ɫ�Ķ��������")]
    private Animator animator;
    [SerializeField]
    [Tooltip("��������Ч����Ԥ��������б�Ӧ�빥�������ĺ�׺һ��")]
    private List<AttackEffect> attackEffectPrefabs;
    [SerializeField]
    [Tooltip("��ʾ��ɫʣ������ֵ��")]
    private List<GameObject> charaLivesShowGO;
    [SerializeField]
    [Tooltip("�ܵ�����ʱ��ʾ�ܻ�������Ч�����")]
    private CharaBeHitShowUI beHitShowUI;
    [SerializeField]
    [Tooltip("��ʾЧ���ϵ�����λ���α�")]
    private Transform showCentreT;


    /// <summary>
    /// ����ʣ����ȴʱ��
    /// </summary>
    private float t_attackCD = 0f;
    /// <summary>
    /// ʣ����õ���Ծ����
    /// </summary>
    private int availableJumpTimes;
    /// <summary>
    /// ��Ծ������ʣ����ȴʱ��
    /// </summary>
    private float t_jumpCD = 0f;
    /// <summary>
    /// Ҫ���ŵĹ���������ţ���0��ʼ��
    /// </summary>
    private int attackAnimIndex = 0;
    
    /// <summary>
    /// ��ǰ�Ƿ�����ס��Ծ��
    /// </summary>
    private bool doJumpButtonHold = false;
    /// <summary>
    /// ��ǰ�����ˮƽ�ٶȣ�ֻ��Ϊ -1/0/1
    /// </summary>
    private float horizontalInput = 0f;
    /// <summary>
    /// ��Ծ���Ѱ�ס��ʱ��
    /// </summary>
    private float t_jumpInputHold = 0f;
    /// <summary>
    /// �Ƿ��յ���Ծ����
    /// </summary>
    private bool doGetJumpInput = false;
    /// <summary>
    /// �Ƿ��ܵ���������
    /// </summary>
    private bool doGetAttackInput = false;

    /// <summary>
    /// �˺�������ʱ��
    /// </summary>
    private float t_damageProtect = 0f;
    [SerializeField,DisplayOnly]
    /// <summary>
    /// ���˺����������ʱ��
    /// </summary>
    private float t_controlLock = 0f;
    /// <summary>
    /// ��ɫ��ǰ������
    /// </summary>
    private int currentLives;
    /// <summary>
    /// ��ǰ���ڽ�ս��BOSS���˵���Ϊ��
    /// </summary>
    private BehaviorTree bossEnemyTree;
    /// <summary>
    /// ������������ײ��
    /// </summary>
    private Collider2D mainCollider2D;

    /// <summary>
    /// ������ǵ������α�
    /// </summary>
    public Transform GetCentreT => showCentreT;
    /// <summary>
    /// �Ƿ��������˺�������
    /// </summary>
    public bool InDamageProtect => t_damageProtect > 0;
    /// <summary>
    /// ��ǰ�Ƿ��������˺�������
    /// </summary>
    public bool InDamageLock => t_controlLock > 0;
    /// <summary>
    /// ��ǰ�Ƿ���Խ�����Ծ
    /// </summary>
    public bool CanJump => availableJumpTimes > 0;
    /// <summary>
    /// ��ǰ�Ƿ����ڽ��й���
    /// </summary>
    public bool InDoingAttack => t_attackCD > 0;
    /// <summary>
    /// ��ǰ�Ƿ���Թ���
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
        // ����������ʱ��
        if (t_controlLock > 0f) t_controlLock -= Time.unscaledDeltaTime;
        // �˺�������ʱ��
        if (t_damageProtect > 0f) t_damageProtect -= Time.unscaledDeltaTime;
        // ÿ֡��ˮƽ�ƶ�
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
        // ÿ֡����Ծ�ж�
        //-��Ծ�����ʱ������
        if (doJumpButtonHold && !doGetJumpInput)
        {
            //��ס��Ծ��ʱ��ﵽ�����Ծ�߶������ʱ��ʱ���Զ���Ծ
            t_jumpInputHold += Time.deltaTime;
            if(t_jumpInputHold > jumpInputTime.y)
            {
                doJumpButtonHold = false;
                doGetJumpInput = true;
            }
        }
        //-������Ծ��������ʱ���ж��Ƿ���Ч
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
        //-��Ծ�����н���
        animator.SetFloat("verticalSpeed", rigidbody.velocity.y);
        // ÿ֡�Ĺ��������ж�
        if (t_attackCD > 0) t_attackCD -= Time.deltaTime;
        if (!InDamageLock && doGetAttackInput && CanAttack) 
        {
            var randomNum = Random.Range(0, attackEffectPrefabs.Count);
            //������Ч�����α�λ�ø��ݵ�ǰˮƽ�ٶȲ���ƫ�ƣ�ȫ�ٱ���ʱ��ʹ�ù���λ�ÿ�ǰ
            attackRootT.localPosition = new Vector3(horizontalSpeed * maxAttackRootOffset / fullSpeed, 0f, 0f);
            //���Ź�����Ч
            AudioManager.Instance.PlaySE(attackSEName, transform);
            //���ɹ���Ч��ʵ��
            var ae = Instantiate(attackEffectPrefabs[randomNum].gameObject, attackRootT).GetComponent<AttackEffect>();
            ae.Init(gameObject, damageFactor);
            //ѭ�����Ź�����������
            attackAnimIndex = (attackAnimIndex + 1) % 2;
            animator.SetFloat("attackAnimIndex", attackAnimIndex);
            animator.SetTrigger("doAttack");
            //��������CD
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
            //�����Ծ��ƽ̨��ײʱ�ظ���Ծ����
            case "Platform":
                //if (jumpState != JumpState.inFalling) break;
                if (rigidbody.transform.position.y + 0.2f > collider.bounds.max.y)
                    availableJumpTimes = jumpTimes;
                AudioManager.Instance.PlaySE(landSEName, transform);
                break;
        }
    }

    /// <summary>
    /// ��ҽ�ɫ�ܵ��˺�ʱЧ��
    /// </summary>
    /// <param name="hitGO">�˺���ҵĶ���</param>
    /// <param name="damage">�ܵ����˺���</param>
    public void OnBeHit(GameObject hitGO,float damage)
    {
        if (InDamageProtect) return;
        //�ܵ��˺�������
        int finalDamage = minDamageTake + (int)(damage / damageToTrueDamgeRatio);
        currentLives -= finalDamage;
        afterLoseHealth?.Invoke();
        //ʣ������ֵ��ʾ
        for (int i = 0; i < charaLivesShowGO.Count; i++) 
            charaLivesShowGO[i].SetActive(i < currentLives);
        //���˺������
        Vector2 finalForce = damageTakeForce;
        finalForce.x *= hitGO.transform.position.x > transform.position.x ? -1 : 1;
        rigidbody.AddForce(finalForce);
        //�ܵ��˺�������
        StartCoroutine(TestManager.Instance.TimeSlowDownBriefly(0.5f, 1f));
        //�ܻ�UI����
        beHitShowUI.PlayBeHitEffectUIAnimation();
        //��������
        t_controlLock = ControlLockTimeAfterDamage;
        if (currentLives > 0)
        {
            //�˺�����
            t_damageProtect = damageProtectTime;
            var s = DOTween.Sequence();
            float interval = damageProtectTime / damageProtectEffectTimes / 2;
            for (int i = 0; i < damageProtectEffectTimes; i++)
            {
                s.Append(spriteRenderer.DOFade(0.5f, interval));
                s.Append(spriteRenderer.DOFade(1f, interval));
            }
            s.Play();
            //���˺���ݵ���������˵���ײ
            StartCoroutine(IgnoreCollisonWithEnemyTemp("Enemy", damageProtectTime));
            IEnumerator IgnoreCollisonWithEnemyTemp(string aimLayerName, float ignoreTime)
            {
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(aimLayerName), true);
                yield return new WaitForSeconds(ignoreTime);
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(aimLayerName), false);
            }
            // ������ʱ����������Ч
            string seName = beHitSEName + (Random.value > 0.5f ? "1" : "2");
            AudioManager.Instance.PlaySE(seName, transform);
            AudioManager.Instance.SwitchSnapShotTemp("MainCharaBeHit", damageProtectTime);
        }
        // ����ֵ����ʱ�����⴦��
        else
        {
            t_controlLock = 99f;
            t_damageProtect = 99f;
            //������ɵ�����Ļ
            mainCollider2D.isTrigger = true;
            //�����Ӱ����BOSS���˼�����Ϊ������
            staticStayT.SetParent(null, true);
            bossEnemyTree.SetVariableValue("BattleTargetT", staticStayT);
            // ������������ʱ����������Ч
            string seName = beHitSEName + (Random.value > 0.5f ? "1" : "2");
            AudioManager.Instance.PlaySE(seName, transform);
            AudioManager.Instance.SwitchSnapShot("MainCharaBeHit");
            //��Ϸʧ�ܽ���
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
    /// ���ÿ���������ָ��ʱ���ڽ�ɫ�޷����ܿ���
    /// </summary>
    /// <param name="t">��������ʱ��</param>
    public void SetControlLock(float t = 999) => t_controlLock = t;
}
