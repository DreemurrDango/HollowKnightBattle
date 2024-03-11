using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorldObject : MonoBehaviour
{

    [SerializeField]
    [Tooltip("����������α�")]
    private Transform followT;

    [SerializeField]
    [Tooltip("�Ƿ����ڸ���")]
    private bool inFollowing = false;

    /// <summary>
    /// ���ø�����α�
    /// </summary>
    public Transform FollowT
    {
        get => followT;
        set => followT = value;
    }

    /// <summary>
    /// ���ø���״̬
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
