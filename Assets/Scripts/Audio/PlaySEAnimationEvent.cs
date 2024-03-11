using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySEAnimationEvent : MonoBehaviour
{
    public void PlaySE(string seName) => AudioManager.Instance.PlaySE(seName, transform);
}
