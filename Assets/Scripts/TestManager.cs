using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class TestManager : Singleton<TestManager>
{
    #region ��ͷ�𶯲���
    [Header("dotween��ͷ")]
    public Camera mainCamera;
    public float duration;
    public float strength;
    public int vibrato;

    [Header("cinemachine��ͷ")]
    public CinemachineImpulseSource impulseSource;
    public float force;

    [ContextMenu("����DOTWEEN��ͷ��")]
    public void DoShakeByDoTween()
    {
        mainCamera.DOShakePosition(duration, strength, vibrato);
    }

    [ContextMenu("����Cinemachine��ͷ��")]
    public void DoShakeByCinemachine()
    {
        impulseSource.GenerateImpulse(force);
    }
    #endregion

    #region ����ʿ��Ծ

    [Header("����ʿ��Ծ")]
    public Rigidbody2D rigidbody2D;
    public Vector2 addForce;
    public bool backOrForward;

    [ContextMenu("��Ծ����")]
    public void DoJump()
    {
        Vector2 force = addForce;
        force.x *= rigidbody2D.transform.localScale.x * (backOrForward ? 1 : -1);
        rigidbody2D.AddForce(force);
    }

    [Header("����ʿ��Ծ")]
    public Transform aimT;
    public Vector2 forceScale;

    [ContextMenu("��Ծ��Ŀ��")]
    public void JumpToAim()
    {
        Vector2 force = Vector2.zero;
        force.x = (aimT.position - rigidbody2D.transform.position).x / rigidbody2D.mass * forceScale.x;
        force.y = forceScale.y;
        rigidbody2D.AddForce(force);
    }
    #endregion

    /// <summary>
    /// ʱ��������Ч
    /// </summary>
    /// <param name="timeScale">�����ڼ��ʱ�����٣�ӦС��1</param>
    /// <param name="t">���ų���ʱ��</param>
    /// <returns></returns>
    public IEnumerator TimeSlowDownBriefly(float timeScale, float t)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(t);
        Time.timeScale = 1;
    }

    public float timeScale;
    private void Update()
    {
        timeScale = Time.timeScale;
    }
}
