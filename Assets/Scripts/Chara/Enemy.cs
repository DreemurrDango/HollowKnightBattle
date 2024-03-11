using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("��ʼ����ֵ")]
    private float originHealth = 100f;
    [SerializeField]
    [Tooltip("�����Ƿ�ɱ���������")]
    private bool canBeHit = true;
    [SerializeField, DisplayOnly]
    [Tooltip("����ֵ")]
    private float health;
    [HideInInspector]
    [Tooltip("ʧȥ����ֵʱ�¼�")]
    public UnityEvent afterLoseHealth;

    [Header("������")]
    [SerializeField]
    [Tooltip("���������")]
    private Animator animator;
    [SerializeField]
    [Tooltip("������Ⱦ��")]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    [Tooltip("��������")]
    private SpriteMask spriteMask;

    [Header("-������ʱЧ��")]
    [SerializeField]
    [Tooltip("����ʱ�ı�ɫ��Ч�����������򲻽�����ʾ")]
    private GameObject hitColorEffect;
    [SerializeField]
    [Tooltip("������ʱ��������Ч�����������򲻽�����ʾ")]
    private ParticleSystem hitParticleEffect;
    [SerializeField]
    [Tooltip("������ʱ���������ʣ����ÿ��򲻽�����ʾ")]
    private string beHitTrigger;
    [SerializeField]
    [Tooltip("������ʱ������Ч��Ϣ�������ÿ��򲻽�����ʾ")]
    private string beHitSEName;

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

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        health = originHealth;
    }

    private void Update()
    {
        if(rigidbody2D != null)animator.SetFloat("verticalSpeed", rigidbody2D.velocity.y);
    }

    /// <summary>
    /// �ܵ��˺�Ч������
    /// </summary>
    /// <param name="damage">�ܵ����˺���</param>
    public void OnBeHit(AttackEffect effect)
    {
        if (!IsAlive) return;
        health -= effect.FinalDamage;
        afterLoseHealth?.Invoke();
        hitEffect = effect;
        if (hitColorEffect != null)
            StartCoroutine(ShowHitColorEffectBriefly(0.1f));
        if (beHitTrigger != "") animator.SetTrigger(beHitTrigger);
        if (health <= 0) OnDied();
        else
        {
            if (beHitSEName != "") AudioManager.Instance.PlaySE("FalseKnight_BeHit", transform);
        }

        //�ܻ���ɫ��Ч�߳�
        IEnumerator ShowHitColorEffectBriefly(float t)
        {
            spriteMask.sprite = spriteRenderer.sprite;
            hitColorEffect.SetActive(true);
            yield return new WaitForSecondsRealtime(t);
            hitColorEffect.SetActive(false);
        }
    }

    /// <summary>
    /// ����ʱ����
    /// </summary>
    private void OnDied()
    {
        health = 0;
        if (onDiedSEName != "") AudioManager.Instance.PlaySE(onDiedSEName, transform);
        //����
        if (onDiedSlowDownLast > 0f)
            StartCoroutine(TestManager.Instance.TimeSlowDownBriefly(onDiedTimeScale, onDiedSlowDownLast));
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
    /// ת��������һ��
    /// </summary>
    public void TurnAround()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
