using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Ԥ��������������ָ����״�����ڲ�������Ԥ����
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// ���ɷ�Χ����״ö��
    /// </summary>
    public enum RangeShape
    {
        [EnumName("����")]
        point,
        [EnumName("����")]
        box2D,
        [EnumName("����")]
        box,
        [EnumName("Բ��")]
        circle,
        [EnumName("����")]
        sphere,
    }

    [Header("����")]
    [SerializeField]
    [Tooltip("���ɷ�Χ����״")]
    protected RangeShape rangeShape = RangeShape.box2D;
    [SerializeField]
    [Tooltip("���ɷ�Χ�Ĵ�С")]
    protected Vector3 rangeSize = Vector3.one;
    [SerializeField]
    [Tooltip("Ҫ���ɵ�Ԥ����")]
    protected GameObject prefabGO;
    [SerializeField]
    [Tooltip("����Ԥ����������ĸ��α䣬����ָ����Ĭ��Ϊ�ö���")]
    protected Transform parentT;
    [SerializeField]
    [Min(0.01f)]
    [Tooltip("���ɵļ����������Ϊ0����һ������������Ԥ����")]
    protected float interval = 0.05f;
    [SerializeField]
    [Tooltip("���ɳ���ʱ�䣬������Ϊ0�����ڴﵽ����������ֹͣ")]
    protected float lastTime;
    [SerializeField]
    [Tooltip("ÿ�����ɵ�����")]
    protected int spawnNum = 1;
    [SerializeField]
    [Tooltip("�Ƿ��ڱ�����ʱ�Ϳ�ʼ����")]
    protected bool startOnAwake;

    [Header("�༭��")]
    [SerializeField]
    [Tooltip("���ɷ�Χ�ı༭����ɫ")]
    private Color gizmosColor;

    /// <summary>
    /// ��ʱ�Ƿ�������������
    /// </summary>
    protected bool inSpawning;
    /// <summary>
    /// �����߳�
    /// </summary>
    protected Coroutine spawnCoroutine = null;

    /// <summary>
    /// ʹ�ø�ֵ���������ɽ��ȣ���ͣ/����
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
    /// ��ʼ�����ú���
    /// </summary>
    /// <param name="prefabGO">Ҫ���ɵ�Ԥ����</param>
    /// <param name="interval">���ɼ��ʱ�䣬����Ϊ0ʱ����һ����������������Ԥ����</param>
    /// <param name="spawnNum">ÿ�����ɵ�Ԥ��������</param>
    public void Init(GameObject prefabGO, float interval,int spawnNum)
    {
        this.prefabGO = prefabGO;
        this.interval = interval;
        this.spawnNum = spawnNum;
    }

    protected void Start() { if (startOnAwake) Play(); }

    [ContextMenu("��ʼ����")]
    /// <summary>
    /// ��ʼ���ŵ�ǰ�趨�����µ����ɶ���
    /// </summary>
    public void Play()
    {
        if (spawnCoroutine != null && inSpawning) return;
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    [ContextMenu("ֹͣ����")]
    /// <summary>
    /// ֹͣ����
    /// </summary>
    public void Stop()
    {
        if (spawnCoroutine == null) return;
        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
        inSpawning = false;
    }

    /// <summary>
    /// �ڵ�ǰ��Χ�ڻ�����һ��
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
    /// �������ɵ�Э��
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
