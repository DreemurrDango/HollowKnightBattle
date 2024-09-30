using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

[ExecuteInEditMode]
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
    public Vector2 direction;

    [ContextMenu("����DOTWEEN��ͷ��")]
    public void DoShakeByDoTween()
    {
        mainCamera.DOShakePosition(duration, strength, vibrato);
    }

    [ContextMenu("����Cinemachine��ͷ��")]
    public void DoShakeByCinemachine()
    {
        impulseSource.GenerateImpulse(direction * force);
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

    #region ʱ������Ч������
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
    #endregion


    #region ʱ������Ч������
    public Transform sourceT;
    public Transform targetT;

    [ContextMenu("���ĳ���")]
    public void LookAt()
    {
        Vector3 direction = (targetT.position - sourceT.position).normalized;
        // ����Ƕ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;
        if (angle > 90 && angle < 270)
        {
            sourceT.localScale = new Vector3(-1f, 1f, 1f);
            angle -= 180;
        }
        else sourceT.localScale = new Vector3(1f, 1f, 1f);
        sourceT.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 direction = (targetT.position - sourceT.position).normalized;
    //    // ����Ƕ�
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    angle = (angle + 360) % 360;
    //    if(angle > 90 && angle < 270)
    //    {
    //        sourceT.localScale = new Vector3(-1f, 1f, 1f);
    //        angle -= 180;
    //    }
    //    else sourceT.localScale = new Vector3(1f, 1f, 1f);
    //    sourceT.rotation = Quaternion.Euler(0f, 0f, angle);
    //}
    #endregion
}
