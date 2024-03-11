using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�ôι����ı�׼�˺�")]
    private float damage = 10f;
    [SerializeField]
    [Tooltip("������ʱ��")]
    private float lifeTime = 5f;
    [SerializeField]
    [Tooltip("׹�䵽����ʱ��˶��ɽ���Ч")]
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
