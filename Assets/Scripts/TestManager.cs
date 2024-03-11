using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class TestManager : Singleton<TestManager>
{
    #region 镜头震动测试
    [Header("dotween镜头")]
    public Camera mainCamera;
    public float duration;
    public float strength;
    public int vibrato;

    [Header("cinemachine镜头")]
    public CinemachineImpulseSource impulseSource;
    public float force;

    [ContextMenu("测试DOTWEEN镜头震动")]
    public void DoShakeByDoTween()
    {
        mainCamera.DOShakePosition(duration, strength, vibrato);
    }

    [ContextMenu("测试Cinemachine镜头震动")]
    public void DoShakeByCinemachine()
    {
        impulseSource.GenerateImpulse(force);
    }
    #endregion

    #region 假骑士跳跃

    [Header("假骑士跳跃")]
    public Rigidbody2D rigidbody2D;
    public Vector2 addForce;
    public bool backOrForward;

    [ContextMenu("跳跃测试")]
    public void DoJump()
    {
        Vector2 force = addForce;
        force.x *= rigidbody2D.transform.localScale.x * (backOrForward ? 1 : -1);
        rigidbody2D.AddForce(force);
    }

    [Header("假骑士跳跃")]
    public Transform aimT;
    public Vector2 forceScale;

    [ContextMenu("跳跃至目标")]
    public void JumpToAim()
    {
        Vector2 force = Vector2.zero;
        force.x = (aimT.position - rigidbody2D.transform.position).x / rigidbody2D.mass * forceScale.x;
        force.y = forceScale.y;
        rigidbody2D.AddForce(force);
    }
    #endregion

    /// <summary>
    /// 时间慢放特效
    /// </summary>
    /// <param name="timeScale">慢放期间的时间流速，应小于1</param>
    /// <param name="t">慢放持续时间</param>
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
