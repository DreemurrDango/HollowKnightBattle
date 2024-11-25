using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using BehaviorDesigner.Runtime;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("��ʼ����ֵ")]
    private float originHealth = 100f;
    [SerializeField]
    [Tooltip("�����Ƿ�ɱ���������")]
    private bool canBeHit = true;
    [SerializeField,DisplayOnly]
    [Tooltip("�Ƿ������ڽ�ֱ״̬����״̬�µĵ����޹��������ҿɱ����������������ټ�Ϊ����")]
    private bool inLimpState = false;
    [SerializeField, DisplayOnly]
    [Tooltip("����ֵ")]
    private float health;
    [HideInInspector]
    [Tooltip("ʧȥ����ֵʱ�¼�")]
    public UnityEvent afterLoseHealth;
    [DisplayOnly,SerializeField]
    [Tooltip("��ǰ�Ƿ��ڵ�����")]
    private bool onGround;

    [Header("������")]
    [SerializeField]
    [Tooltip("���������")]
    private Animator animator;
    [SerializeField]
    [Tooltip("������Ⱦ��")]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    [Tooltip("�õ��˵���Ϊ�����")]
    private BehaviorTree behaviorTree;
    [SerializeField]
    [Tooltip("�õ��˵���ײ�˺���")]
    private AttackEffect colliderAttackEffect;

    [Header("-������ʱЧ��")]
    [SerializeField]
    [Tooltip("������ʱ��������Ч�����������򲻽�����ʾ")]
    private ParticleSystem hitParticleEffect;
    [SerializeField]
    [Tooltip("�õ�������ʱ�ܵ��Ļ�����Ч��")]
    private Vector2 hitBackForce = Vector2.zero;
    [SerializeField]
    [Tooltip("������ʱ���������ʣ����ÿ��򲻽�����ʾ")]
    private string beHitTrigger;
    [SerializeField]
    [Tooltip("������ʱ������Ч��Ϣ�������ÿ��򲻽�����ʾ")]
    private string beHitSEName;
    [DisplayOnly, SerializeField]
    [Tooltip("����һ��ս����ֱ���ۼƱ����еĴ���")]
    private int beHitCount;
    [Header("--����")]
    [SerializeField]
    [Tooltip("����ʱ������������ʱ���ܻ������ж�")]
    private float t_beHitComboInterval = 2f;
    [DisplayOnly, SerializeField]
    [Tooltip("����������")]
    private int beHitComboCount;

    [Header("-����ʱЧ��")]
    [SerializeField]
    [Tooltip("�Ƿ��ڸõ�������ʱ���ˮƽ�ٶ�")]
    private bool clearXVelocity;
    [SerializeField]
    [Tooltip("�Ƿ��ڸõ�������ʱ�����ֱ�ٶ�")]
    private bool clearYVelocity;
    [SerializeField]
    [Tooltip("�õ�������ʱ�ܵ���Ч��")]
    private Vector2 dieForce = Vector2.zero;
    [SerializeField]
    [Tooltip("������ʱ������Ч��Ϣ�������ÿ��򲻽�����ʾ")]
    private string onDiedSEName;
    [Range(0.1f,1f)]
    [SerializeField]
    [Tooltip("�õ�������ʱ������ͷʱ�䱶��")]
    private float onDiedTimeScale = 0.5f;
    [SerializeField]
    [Tooltip("�õ�������ʱ������ͷЧ������ʱ�䣬Ϊ0ʱ������ͷЧ��")]
    private float onDiedSlowDownLast = 0.4f;

    /// <summary>
    /// �������
    /// </summary>
    private Rigidbody2D rigidbody2D;
    /// <summary>
    /// �����˺��˸õ�λ�Ĺ�����Ч
    /// </summary>
    private AttackEffect hitEffect;
    /// <summary>
    /// ��һ���ܻ���ʱ���
    /// </summary>
    private float t_lastHitTime;
    /// <summary>
    /// ����ľ�����Ⱦ��
    /// </summary>
    private Material material;

    /// <summary>
    /// �ö���ǰ�Ƿ�ɱ���������
    /// </summary>
    public bool CanBeHit { get => canBeHit; set => canBeHit = value; }
    /// <summary>
    /// ���˵�����ֵ
    /// </summary>
    public int Health => (int)health;
    /// <summary>
    /// ���˵�����ֵ����
    /// </summary>
    public int FullHealth => (int)originHealth;
    /// <summary>
    /// ��ǰ�����Ƿ��Դ��
    /// </summary>
    public bool IsAlive => health > 0;
    /// <summary>
    /// �õ��˵�ǰ�Ƿ������Ҳ�
    /// </summary>
    public bool DoFaceRight => transform.localScale.x > 0;
    /// <summary>
    /// ��һ�ν�ֱ�󱻹������ܴ��������ڽ����ж�
    /// </summary>
    public int BeHitCount { get => beHitCount; set => beHitCount = value; }
    /// <summary>
    /// ����������
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
    /// �ܵ��˺�Ч������
    /// </summary>
    /// <param name="damage">�ܵ����˺���</param>
    public void OnBeHit(AttackEffect effect)
    {
        behaviorTree.SendEvent("BeHit");
        if (!IsAlive) return;
        if (!CanBeHit) return;
        //��Ӳֱ״̬���ܻ�����ͳ��
        if (!inLimpState) beHitCount++;
        //��Ӳֱ״̬����������ͳ��
        if(!inLimpState)
        {
            if (Time.time - t_lastHitTime < t_beHitComboInterval) beHitComboCount++;
            else beHitComboCount = 1;
            t_lastHitTime = Time.time;
        }
        //�ܻ�������
        if (hitBackForce != Vector2.zero)
        {
            Vector2 v = effect.transform.position - transform.position;
            rigidbody2D.AddForce( v * -1 * hitBackForce);
        }
        //�����˺�
        health -= effect.FinalDamage;
        behaviorTree.SendEvent<float>("BeHurt",effect.FinalDamage);
        afterLoseHealth?.Invoke();
        hitEffect = effect;
        ShowHitColorEffectBriefly(0.1f);
        //���˶���������
        if (beHitTrigger != "") animator.SetTrigger(beHitTrigger);
        if (health <= 0) OnDied();
        else if(beHitSEName != "") AudioManager.Instance.PlaySE(beHitSEName, transform);


        //�ܻ���ɫ��Ч�߳�
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
    /// ����ʱ����
    /// </summary>
    private void OnDied()
    {
        health = 0;
        if(colliderAttackEffect != null) colliderAttackEffect.enabled = false;
        if (onDiedSEName != "") AudioManager.Instance.PlaySE(onDiedSEName, transform);
        //����
        if (onDiedSlowDownLast > 0f && onDiedTimeScale !=  1)
            StartCoroutine(GameManager.TimeSlowDownBriefly(onDiedTimeScale, onDiedSlowDownLast));
        // ��������ͣ�������ǰ�ٶ�
        Vector2 v = rigidbody2D.velocity;
        if (clearXVelocity) v.x = 0;
        if (clearYVelocity) v.y = 0;
        rigidbody2D.velocity = v;
        // �����������������ʩ��
        Vector2 f = dieForce;
        if (dieForce.x != 0 && hitEffect.transform.position.x > transform.position.x)
            f.x *= -1;
        if (f != Vector2.zero) rigidbody2D.AddForce(f);
    }

    /// <summary>
    /// �ָ�����ֵ
    /// </summary>
    /// <param name="value">Ҫ�ָ�������ֵ��������ָ����ָ�Ϊ������ֵ</param>
    public void RecoverHealth(float value = 0)
    {
        if (value == 0) value = originHealth - health;
        health += value;
    }

    /// <summary>
    /// �õ��˸���ظ���ǰ����ֵ���µ�����ֵ����
    /// </summary>
    /// <param name="newOriginHealth">�µ�����ֵ����</param>
    public void Rebrith(float newOriginHealth = 0)
    {
        if (newOriginHealth == 0) newOriginHealth = originHealth;
        originHealth = newOriginHealth;
        health = newOriginHealth;
        CanBeHit = true;
    }

    /// <summary>
    /// ���ÿɱ��������Ե�ֵ
    /// </summary>
    /// <param name="value"></param>
    public void SetCanBeHit(float value) => CanBeHit = value != 0;

    /// <summary>
    /// ����Ӳֱ״̬�仯
    /// </summary>
    /// <param name="exitOrEnter">F���˳�Ӳֱ״̬��T������</param>
    public void SetLimpState(bool exitOrEnter)
    {
        if(inLimpState == exitOrEnter) return;
        if (exitOrEnter) beHitCount = beHitComboCount = 0;
        inLimpState = exitOrEnter;
    }

    /// <summary>
    /// ת��������һ��
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
