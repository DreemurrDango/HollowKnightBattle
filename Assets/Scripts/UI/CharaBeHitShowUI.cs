using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBeHitShowUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("��������ʱ������Ч��z����תֵ�ķ�Χ")]
    private Vector2 hitShowRotateRange;

    [SerializeField]
    [Tooltip("��������ʱ������Ч�ĸ��ڵ�")]
    private RectTransform hitLightningRootRT;
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// ���Ž�ɫ�ܻ�UI����
    /// </summary>
    public void PlayBeHitEffectUIAnimation()
    {
        //�����������Ч��Z����תֵ
        Vector3 rotate = new Vector3(0, 0, Random.Range(hitShowRotateRange.x, hitShowRotateRange.y));
        hitLightningRootRT.localRotation = Quaternion.Euler(rotate);
        //���Ŷ���Ч��
        animator.SetTrigger("showBeHitEffect");
    }
}
