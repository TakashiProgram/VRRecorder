using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBlink : MonoBehaviour
{
    private SkinnedMeshRenderer m_SkinnedMeshRenderer;

    [SerializeField] private int m_LeftEyeIndex = 0;
    [SerializeField] private int m_RightEyeIndex = 1;

    [SerializeField] private List<int> m_LeftIgnoreIndexs;
    [SerializeField] private List<int> m_RightIgnoreIndexs;

    [SerializeField] private float m_CloseTime = 0f;
    [SerializeField] private float m_CloseHoldingTime = 0f;
    [SerializeField] private float m_OpenTime = 0f;

    private bool m_IsBlink = false;

    public float m_MinIntervalSec = 3.0f;
    public float m_MaxIntervalSec = 7.0f;

    private static readonly float CLOSE_RATIO = 100f;
    private static readonly float OPEN_RATIO = 0f;

    bool m_IsBlinkLoop = true;

    Coroutine m_Coroutine = null;

    private void Start()
    {
        m_IsBlinkLoop = true;
        m_SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if (m_SkinnedMeshRenderer != null && m_IsBlinkLoop == true && m_Coroutine == null)
        {
            m_Coroutine = StartCoroutine(BlinkWait());
        }
    }


    void LateUpdate()
    {
        if (false == m_IsBlink)
        {
            return;
        }

        BlinkEye(m_RightIgnoreIndexs, m_RightEyeIndex);
        BlinkEye(m_LeftIgnoreIndexs, m_LeftEyeIndex);

        m_IsBlink = false;
    }

    IEnumerator BlinkWait()
    {
        while (m_IsBlinkLoop)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(m_MinIntervalSec, m_MaxIntervalSec));

            if (m_IsBlinkLoop == false)
            {
                break;
            }

            m_IsBlink = true;
        }
    }


    public void StopBlink()
    {
        m_IsBlink = false;
        m_IsBlinkLoop = false;

        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }
    }


    private void BlinkEye(List<int> ignoreList, int eyeIndex)
    {
        if (false == CanBlink(ignoreList))
        {
            return;
        }

        StartCoroutine(Blink(eyeIndex));
    }

    private IEnumerator Blink(int index)
    {
        if (null == m_SkinnedMeshRenderer)
        {
            yield break;
        }

        float elapsed_time = 0f;

        while (m_CloseTime > elapsed_time)
        {
            float value = (elapsed_time / m_CloseTime) * CLOSE_RATIO;
            m_SkinnedMeshRenderer.SetBlendShapeWeight(index, value);
            elapsed_time += Time.deltaTime;

            yield return null;
        }

        if ( 0f < m_CloseHoldingTime )
        {
            yield return new WaitForSeconds(m_CloseHoldingTime);
        }
        
        elapsed_time = 0f;
        while (m_OpenTime > elapsed_time)
        {
            float value = (1.0f - (elapsed_time / m_OpenTime)) * CLOSE_RATIO;
            m_SkinnedMeshRenderer.SetBlendShapeWeight(index, value);
            elapsed_time += Time.deltaTime;

            yield return null;
        }
        m_SkinnedMeshRenderer.SetBlendShapeWeight(index, OPEN_RATIO);

    }


    private bool CanBlink(List<int> indexs)
    {
        if (null == m_SkinnedMeshRenderer)
        {
            return false;
        }

        foreach (var index in indexs)
        {
            if (0 < m_SkinnedMeshRenderer.GetBlendShapeWeight(index))
            {
                return false;
            }
        }

        return true;
    }

}
