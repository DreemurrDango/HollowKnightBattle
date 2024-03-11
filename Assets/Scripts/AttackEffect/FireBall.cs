using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    [Tooltip("该次攻击的标准伤害")]
    private float damage = 10f;
    [SerializeField]
    [Tooltip("最大存续时间")]
    private float lifeTime = 5f;
    [SerializeField]
    [Tooltip("坠落到地面时的硕块飞溅特效")]
    private ParticleSystem sparkOnFallToGround;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "MainChara":
                CharaController.Instance.OnBeHit(gameObject, damage);
                break;
            case "Environment":
                Destroy(gameObject);
                Instantiate(sparkOnFallToGround.gameObject, transform.position, Quaternion.identity);
                break;
        }
    }
}
