using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ч��������ܹ���ָ��Ŀ������˺�
/// </summary>
[DisallowMultipleComponent]
public class AttackEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�ôι����ı�׼�˺�")]
    private float damage = 10f;
    [SerializeField]
    [Tooltip("�˹���Ч�������ʱ��������Ϊ0ʱ�����Զ�����")]
    private float lifeTime = 3f;
    [SerializeField]
    [Tooltip("����Ч���ķ����ߣ��ڼ�������ʱ����к���")]
    private GameObject belongerGO;
    [SerializeField]
    [Tooltip("�ܹ������Ĳ㼶")]
    private LayerMask aimLayer;

    /// <summary>
    /// ���ι������˺�ϵ��
    /// </summary>
    private float damageFactor;

    /// <summary>
    /// ���ι��������˺�
    /// </summary>
    public float FinalDamage => damage *= damageFactor;

    private void Start()
    {
        if (lifeTime > 0) Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// ��ʼ��������Ӧ������ʱ����
    /// </summary>
    /// <param name="factor"></param>
    public void Init(GameObject belonger, float factor = 1f)
    {
        belongerGO = belonger;
        damageFactor = factor;
    }

    public void OnHitObject(GameObject hitGO)
    {
        //Debug.Log(hitGO.name);
        if (!this.enabled || hitGO == belongerGO) return;
        if((aimLayer.value & 1 << hitGO.layer) > 0)
        {
            switch (LayerMask.LayerToName(hitGO.layer))
            {
                case "Enemy":
                    var e = hitGO.GetComponent<Enemy>();
                    e.OnBeHit(this);
                    break;
                case "MainChara":
                    CharaController.Instance.OnBeHit(gameObject, FinalDamage);
                    break;
            }
        }        
    }
}
