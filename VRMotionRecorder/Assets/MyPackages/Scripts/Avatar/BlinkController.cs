using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float m_BlinkRate = 0.5f;
    [SerializeField] private bool m_IsOneEyedBlink = false;
    [SerializeField] private float m_BlinkCloseSec = 0.1f;
    [SerializeField] private float m_BlinkOpenSec = 0.1f;
    [SerializeField] private FacialExpressionController m_FacialExpressionController;

    private SkinnedMeshRenderer m_Renderer = null;
    private int m_LeftIndex = 0;
    private int m_RightIndex = 0;

    private BlinkState m_LeftBlinkState = BlinkState.OPEN;
    private BlinkState m_RightBlinkState = BlinkState.OPEN;

    private Coroutine m_LeftBlinkCoroutine = null;
    private Coroutine m_RightBlinkCoroutine = null;

    private float m_LeftCurrentRate = 0f;

    private static readonly float CLOSE_RATIO = 100f;
    private static readonly float OPEN_RATIO = 0f;

    public void Init(float rate, bool is_one_eyed)
    {
        m_BlinkRate = Mathf.Clamp01(rate);
        m_IsOneEyedBlink = is_one_eyed;
    }

    public void SetBlinkIndex(SkinnedMeshRenderer renderer, int index, bool is_left)
    {
        if ( null == m_Renderer)
        {
            m_Renderer = renderer;
        }

        if (is_left)
        {
            m_LeftIndex = index;
        }
        else
        {
            m_RightIndex = index;
        }
    }

    public void UpdateEyeOpenRate( float rate, bool is_left)
    {
        if (null == m_Renderer)
        {
            return;
        }

        if ( true == m_IsOneEyedBlink) //片目ずつバラバラに動作する場合
        {
            if (true == is_left)
            {
                UpdateLeftEye(rate);
            }
            else
            {
                UpdateRightEye(rate);
            }
        }
        else //両目が同時に動作する場合
        {
            if (true == is_left)
            {
                m_LeftCurrentRate = rate;
                return;
            }

            ////左右のうち、より開いている目のrateに合わせる
            //rate = Mathf.Min(m_LeftCurrentRate, rate);

            //左右のうち、より閉じている目のrateに合わせる
            rate = Mathf.Max(m_LeftCurrentRate, rate);

            UpdateLeftEye(rate);
            UpdateRightEye(rate);
        }
    }

    private void UpdateLeftEye(float rate)
    {
        if (false == CanBlink(true))
        {
            ForceStopBlink(true);
            return;
        }

        if ((BlinkState.OPEN == m_LeftBlinkState) &&
             (m_BlinkRate <= rate))
        {
            m_LeftBlinkCoroutine = StartCoroutine(CloseEye(true));
        }
        else if ((BlinkState.CLOSE == m_LeftBlinkState) &&
             (m_BlinkRate > rate))
        {
            m_LeftBlinkCoroutine = StartCoroutine(OpenEye(true));
        }
    }

    private void UpdateRightEye(float rate)
    {
        if( false == CanBlink(false))
        {
            ForceStopBlink(false);
            return;
        }

        if ((BlinkState.OPEN == m_RightBlinkState) &&
             (m_BlinkRate <= rate))
        {
            m_RightBlinkCoroutine = StartCoroutine(CloseEye(false));
        }
        else if ((BlinkState.CLOSE == m_RightBlinkState) &&
             (m_BlinkRate > rate))
        {
            m_RightBlinkCoroutine = StartCoroutine(OpenEye(false));
        }
    }

    private IEnumerator CloseEye( bool is_left )
    {
        float elapsed_time = 0f;

        if (true == is_left)
        {
            m_LeftBlinkState = BlinkState.IS_CLOSING;
            while (m_BlinkCloseSec > elapsed_time)
            {
                float value = (elapsed_time / m_BlinkCloseSec) * CLOSE_RATIO;
                m_Renderer.SetBlendShapeWeight(m_LeftIndex, value);
                elapsed_time += Time.deltaTime;

                yield return null;
            }
            m_Renderer.SetBlendShapeWeight(m_LeftIndex, CLOSE_RATIO);
            m_LeftBlinkState = BlinkState.CLOSE;
            m_LeftBlinkCoroutine = null;
        }
        else
        {
            m_RightBlinkState = BlinkState.IS_CLOSING;
            while (m_BlinkCloseSec > elapsed_time)
            {
                float value = (elapsed_time / m_BlinkCloseSec) * CLOSE_RATIO;
                m_Renderer.SetBlendShapeWeight(m_RightIndex, value);
                elapsed_time += Time.deltaTime;

                yield return null;
            }
            m_Renderer.SetBlendShapeWeight(m_RightIndex, CLOSE_RATIO);
            m_RightBlinkState = BlinkState.CLOSE;
            m_RightBlinkCoroutine = null;
        }
    }

    private IEnumerator OpenEye( bool is_left )
    {
        float elapsed_time = 0f;

        if (true == is_left)
        {
            m_LeftBlinkState = BlinkState.IS_OPENING;
            while (m_BlinkOpenSec > elapsed_time)
            {
                float value = ( 1f- (elapsed_time / m_BlinkOpenSec) ) * CLOSE_RATIO;
                m_Renderer.SetBlendShapeWeight(m_LeftIndex, value);
                elapsed_time += Time.deltaTime;

                yield return null;
            }
            m_Renderer.SetBlendShapeWeight(m_LeftIndex, OPEN_RATIO);
            m_LeftBlinkState = BlinkState.OPEN;
            m_LeftBlinkCoroutine = null;
        }
        else
        {
            m_RightBlinkState = BlinkState.IS_OPENING;
            while (m_BlinkOpenSec > elapsed_time)
            {
                float value = (1f - (elapsed_time / m_BlinkOpenSec)) * CLOSE_RATIO;
                m_Renderer.SetBlendShapeWeight(m_RightIndex, value);
                elapsed_time += Time.deltaTime;

                yield return null;
            }
            m_Renderer.SetBlendShapeWeight(m_RightIndex, OPEN_RATIO);
            m_RightBlinkState = BlinkState.OPEN;
            m_RightBlinkCoroutine = null;
        }
    }

    private bool CanBlink(bool is_left)
    {
        if ( null == m_FacialExpressionController )
        {
            return true;
        }

        return m_FacialExpressionController.CanBlink(is_left);
    }

    private void ForceStopBlink(bool is_left)
    {
        if (true == is_left)
        {
            if (null != m_LeftBlinkCoroutine)
            {
                StopCoroutine(m_LeftBlinkCoroutine);
            }

            m_Renderer.SetBlendShapeWeight(m_LeftIndex, OPEN_RATIO);
            m_LeftBlinkState = BlinkState.OPEN;
        }
        else
        {
            if (null != m_RightBlinkCoroutine)
            {
                StopCoroutine(m_RightBlinkCoroutine);
            }

            m_Renderer.SetBlendShapeWeight(m_RightIndex, OPEN_RATIO);
            m_RightBlinkState = BlinkState.OPEN;
        }

    }

    private enum BlinkState : int
    {
        OPEN = 0,
        IS_CLOSING,
        CLOSE,
        IS_OPENING
    }
}
