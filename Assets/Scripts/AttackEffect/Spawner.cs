using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 预制体生成器：在指定形状区域内不断生成预制体
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// 生成范围的形状枚举
    /// </summary>
    public enum RangeShape
    {
        [EnumName("定点")]
        point,
        [EnumName("矩形")]
        box2D,
        [EnumName("盒形")]
        box,
        [EnumName("圆形")]
        circle,
        [EnumName("球形")]
        sphere,
    }

    [Header("属性")]
    [SerializeField]
    [Tooltip("生成范围的形状")]
    protected RangeShape rangeShape = RangeShape.box2D;
    [SerializeField]
    [Tooltip("生成范围的大小")]
    protected Vector3 rangeSize = Vector3.one;
    [SerializeField]
    [Tooltip("要生成的预制体")]
    protected GameObject prefabGO;
    [SerializeField]
    [Tooltip("生成预制体后所属的根形变，若不指定则默认为该对象")]
    protected Transform parentT;
    [SerializeField]
    [Min(0.01f)]
    [Tooltip("生成的间隔，若设置为0，则将一次性生成所有预制体")]
    protected float interval = 0.05f;
    [SerializeField]
    [Tooltip("生成持续时间，若设置为0，则将在达到生成数量后停止")]
    protected float lastTime;
    [SerializeField]
    [Tooltip("每次生成的数量")]
    protected int spawnNum = 1;
    [SerializeField]
    [Tooltip("是否在被启用时就开始生成")]
    protected bool startOnAwake;

    [Header("编辑器")]
    [SerializeField]
    [Tooltip("生成范围的编辑下颜色")]
    private Color gizmosColor;

    /// <summary>
    /// 当时是否正处于生成中
    /// </summary>
    protected bool inSpawning;
    /// <summary>
    /// 生成线程
    /// </summary>
    protected Coroutine spawnCoroutine = null;

    /// <summary>
    /// 使用该值来控制生成进度，暂停/继续
    /// </summary>
    public bool InSpawning
    {
        get => inSpawning;
        set
        {
            if (value == inSpawning) return;
            inSpawning = value;
        }
    }

    /// <summary>
    /// 初始化设置函数
    /// </summary>
    /// <param name="prefabGO">要生成的预制体</param>
    /// <param name="interval">生成间隔时间，设置为0时，将一次性生成所有数量预制体</param>
    /// <param name="spawnNum">每次生成的预制体数量</param>
    public void Init(GameObject prefabGO, float interval,int spawnNum)
    {
        this.prefabGO = prefabGO;
        this.interval = interval;
        this.spawnNum = spawnNum;
    }

    protected void Start() { if (startOnAwake) Play(); }

    [ContextMenu("开始生成")]
    /// <summary>
    /// 开始播放当前设定属性下的生成动作
    /// </summary>
    public void Play()
    {
        if (spawnCoroutine != null && inSpawning) return;
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    [ContextMenu("停止生成")]
    /// <summary>
    /// 停止生成
    /// </summary>
    public void Stop()
    {
        if (spawnCoroutine == null) return;
        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
        inSpawning = false;
    }

    /// <summary>
    /// 在当前范围内获得随机一点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomInsidePoint()
    {
        Vector3 halfSize = rangeSize / 2;
        Vector3 result = Vector3.one;
        switch (rangeShape)
        {
            case RangeShape.point:
                result = transform.position;
                break;
            case RangeShape.box2D:
                result.x = Random.Range(halfSize.x * -1, halfSize.x);
                result.y = Random.Range(halfSize.y * -1, halfSize.y);
                result += transform.position;
                break;
            case RangeShape.box:
                result.x = Random.Range(halfSize.x * -1, halfSize.x);
                result.y = Random.Range(halfSize.y * -1, halfSize.y);
                result.z = Random.Range(halfSize.z * -1, halfSize.z);
                result += transform.position;
                break;
            case RangeShape.circle:
                result = (Vector3)(Random.insideUnitCircle * rangeSize.x) + transform.position;
                break;
            case RangeShape.sphere:
                result = Random.insideUnitSphere * rangeSize.x + transform.position;
                break;
        }
        return result;
    }

    /// <summary>
    /// 持续生成的协程
    /// </summary>
    /// <returns></returns>
    protected IEnumerator SpawnRoutine() 
    {
        float beginT = Time.time;
        inSpawning = true;
        while(lastTime == 0 || Time.time -  beginT < lastTime)
        {
            while (!inSpawning) yield return null;
            for (int i = 0; i < spawnNum; i++) SpawnInstance(prefabGO);
            if (interval > 0) yield return new WaitForSeconds(interval);
        }
        inSpawning = false;
    }

    protected virtual GameObject SpawnInstance(GameObject prefab)
    {
        Transform t = parentT == null ? transform : parentT;
        return Instantiate(prefab, GetRandomInsidePoint(), Quaternion.identity, t);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmosColor;
        switch (rangeShape)
        {
            case RangeShape.box2D:
            case RangeShape.box:
                Gizmos.DrawCube(transform.position, rangeSize);
                break;
            case RangeShape.circle:
            case RangeShape.sphere:
                Gizmos.DrawSphere(transform.position, rangeSize.x);
                break;
        }
    }
}
