using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventTransmit : MonoBehaviour
{
    [SerializeField]
    [Tooltip("2D触发器进入消息")]
    private UnityEvent<GameObject> onTriggerEnter2D;
    [SerializeField]
    [Tooltip("2D碰撞器发生消息")]
    private UnityEvent<GameObject> onCollisionEnter2D;

    private void OnTriggerEnter2D(Collider2D collision) => onTriggerEnter2D?.Invoke(collision.gameObject);

    private void OnCollisionEnter2D(Collision2D collision) => onCollisionEnter2D?.Invoke(collision.gameObject);
}
