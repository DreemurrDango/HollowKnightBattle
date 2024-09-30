using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySEAnimationEvent : MonoBehaviour
{
    public void PlaySE(string seName) => AudioManager.Instance.PlaySE(seName, transform);
    public void PlayRandomSE(string seNameList)
    {
        string[] seNames = seNameList.Split(",");
        int i = Random.Range(0, seNames.Length);
        if (seNames[i] != "") AudioManager.Instance.PlaySE(seNames[i], transform);
    }
}
