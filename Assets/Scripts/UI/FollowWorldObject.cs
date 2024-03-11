using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorldObject : MonoBehaviour
{

    [SerializeField]
    [Tooltip("跟随的世界形变")]
    private Transform followT;

    [SerializeField]
    [Tooltip("是否正在跟随")]
    private bool inFollowing = false;

    /// <summary>
    /// 设置跟随的形变
    /// </summary>
    public Transform FollowT
    {
        get => followT;
        set => followT = value;
    }

    /// <summary>
    /// 设置跟随状态
    /// </summary>
    public bool InFollowing
    {
        get => inFollowing;
        set => inFollowing = value;
    }

    private void Update()
    {
        if (inFollowing) transform.position = Camera.main.WorldToScreenPoint(followT.position);
    }
}
