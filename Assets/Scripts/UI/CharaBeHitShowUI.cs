using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBeHitShowUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("主角受伤时闪电特效的z轴旋转值的范围")]
    private Vector2 hitShowRotateRange;

    [SerializeField]
    [Tooltip("主角受伤时闪电特效的根节点")]
    private RectTransform hitLightningRootRT;
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// 播放角色受击UI动画
    /// </summary>
    public void PlayBeHitEffectUIAnimation()
    {
        //随机化闪电特效的Z轴旋转值
        Vector3 rotate = new Vector3(0, 0, Random.Range(hitShowRotateRange.x, hitShowRotateRange.y));
        hitLightningRootRT.localRotation = Quaternion.Euler(rotate);
        //播放动画效果
        animator.SetTrigger("showBeHitEffect");
    }
}
