using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击效果组件：能够对指定目标造成伤害
/// </summary>
[DisallowMultipleComponent]
public class AttackEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("该次攻击的标准伤害")]
    private float damage = 10f;
    [SerializeField]
    [Tooltip("此攻击效果最多存活时长，设置为0时不会自动消除")]
    private float lifeTime = 3f;
    [SerializeField]
    [Tooltip("攻击效果的发出者，在计算命中时会进行忽视")]
    private GameObject belongerGO;
    [SerializeField]
    [Tooltip("能攻击到的层级")]
    private LayerMask aimLayer;

    /// <summary>
    /// 本次攻击的伤害系数
    /// </summary>
    private float damageFactor;

    /// <summary>
    /// 本次攻击最终伤害
    /// </summary>
    public float FinalDamage => damage *= damageFactor;

    private void Start()
    {
        if (lifeTime > 0) Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// 初始化函数，应在生成时调用
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
