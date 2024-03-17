using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// 控制游戏的整体流程：进入场景时根据设备显示操作提示、游戏正式开始、游戏结束结算
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Tooltip("获胜菜单界面")]
    public EndMenuUI victoryMenuUI;
    [Tooltip("失败菜单界面")]
    public EndMenuUI defeatMenuUI;
    [Tooltip("失败时环境音音频素材")]
    public AudioClip defeatAmbientCilp;
    [Tooltip("失败时混音效果")]
    public string defeatSnapshotName;
    [Tooltip("移动设备的控制UI")]
    public GameObject mobileControlUI;

    [Tooltip("初始化事件")]
    public UnityEvent initEvents;
    [Tooltip("游戏正式开始的相关事件")]
    public UnityEvent gameBeginEvents;


    protected override void Awake()
    {
        base.Awake();
        initEvents?.Invoke();
    }

    private void Start()
    {
        if(Application.isMobilePlatform)
        {
            Screen.autorotateToPortrait = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            //Screen.fullScreen = true;
        }
        mobileControlUI.SetActive(Application.isMobilePlatform);
    }

    /// <summary>
    /// 游戏正式开始，执行一系列动作
    /// </summary>
    public void GameBegin() => gameBeginEvents?.Invoke();

    /// <summary>
    /// 游戏结束时系列动作
    /// </summary>
    /// <param name="doPlayerWin">玩家是否获得了胜利</param>
    /// <param name="waitTime">显示结算UI的等待时间</param>
    public void GameEnd(bool doPlayerWin,float waitTime = 2f)
    {
        StartCoroutine(ShowGameResultUIRoutine());
        IEnumerator ShowGameResultUIRoutine()
        {
            if (waitTime > 0) yield return new WaitForSeconds(waitTime);
            if (doPlayerWin) victoryMenuUI.gameObject.SetActive(true);
            else
            {
                defeatMenuUI.gameObject.SetActive(true);
                AudioManager.Instance.PlayAmbient(defeatAmbientCilp);
                AudioManager.Instance.SwitchSnapShot(defeatSnapshotName);
            }
        }
    }

    /// <summary>
    /// 重载游戏关卡
    /// </summary>
    /// <param name="waitTime">短暂等待的时间</param>
    public void ReloadGame(float waitTime)
    {
        StartCoroutine(ReloadGameRoutine(waitTime));
        IEnumerator ReloadGameRoutine(float waitTime)
        {
            if (waitTime > 0) yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    /// <param name="waitTime">短暂等待的时间</param>
    public void ExitGame(float waitTime)
    {
        StartCoroutine(ExitGameRoutine(waitTime));
        IEnumerator ExitGameRoutine(float waitTime)
        {
            if (waitTime > 0) yield return new WaitForSeconds(waitTime);
            Application.Quit();
        }
    }

    public void SwitchFullScreen() => Screen.fullScreen = !Screen.fullScreen;
}
