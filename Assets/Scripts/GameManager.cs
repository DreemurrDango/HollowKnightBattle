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
            LoadScene();
        }
    }

    /// <summary>
    /// 重载游戏关卡
    /// </summary>
    /// <param name="waitTime">短暂等待的时间</param>
    public void LoadNextLevel(float waitTime)
    {
        StartCoroutine(LoadLevelRoutine(waitTime));
        IEnumerator LoadLevelRoutine(float waitTime)
        {
            if (waitTime > 0) yield return new WaitForSeconds(waitTime);
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    /// <summary>
    /// 立即载入场景
    /// </summary>
    /// <param name="buildIndex">场景构建序号，不填时默认为当前场景</param>
    public void LoadScene(int buildIndex = -1) => 
        SceneManager.LoadScene(buildIndex != -1? buildIndex :SceneManager.GetActiveScene().buildIndex);

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

    /// <summary>
    /// 时间慢放特效
    /// </summary>
    /// <param name="timeScale">慢放期间的时间流速，应小于1</param>
    /// <param name="t">慢放持续时间</param>
    /// <returns></returns>
    public static IEnumerator TimeSlowDownBriefly(float timeScale, float t)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(t);
        Time.timeScale = 1;
    }
    public void SwitchFullScreen() => Screen.fullScreen = !Screen.fullScreen;
}
