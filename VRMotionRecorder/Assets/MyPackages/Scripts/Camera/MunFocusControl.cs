using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Rendering.PostProcessing;
using MonobitEngine;

public class MunFocusControl : MonobitEngine.MonoBehaviour
{
    [SerializeField] private GameObject m_Window = null;
    [SerializeField] private Text m_Text = null;
    [SerializeField] private float m_MoveSpeed = 0.5f;
    [SerializeField] private float m_DisplaySec = 1.5f;

    //private PostProcessVolume m_PPVolume = null;
    private float m_DisplayLeftSec = 0f;

    private static readonly string FOCUS_DISTANCE_BUTTON = "Drone_FocusDistance";
    private static readonly string TEXT_FORMAT = "F2";


    void Update()
    {
        if ( false == monobitView.isOwner )
        {
            return;
        }

        InputKey();
        CountDisplayTime();
    }

    private void InputKey()
    {
        float input = Input.GetAxis(FOCUS_DISTANCE_BUTTON);
        if (0f == input)
        {
            return;
        }

        //if (null == m_PPVolume)
        //{
        //    FindPPVolume();
        //}

        //if (null == m_PPVolume)
        //{
        //    return;
        //}

        //DepthOfField pr;
        //if (false == m_PPVolume.sharedProfile.TryGetSettings<DepthOfField>(out pr))
        //{
        //    return;
        //}

        //float value = pr.focusDistance.value + (input * m_MoveSpeed * Time.deltaTime);
        //pr.focusDistance.value = value;

        //if (null != m_Text)
        //{
        //    m_Text.text = value.ToString(TEXT_FORMAT);
        //}

        //m_DisplayLeftSec = m_DisplaySec;
    }

    private void CountDisplayTime()
    {
        if ( 0f < m_DisplayLeftSec )
        {
            m_DisplayLeftSec -= Time.deltaTime;
        }

        if ( ( false == m_Window.activeSelf) &&
            ( 0f < m_DisplayLeftSec ))
        {
            m_Window.SetActive(true);
        }

        if ((true == m_Window.activeSelf) &&
            (0f >= m_DisplayLeftSec))
        {
            m_Window.SetActive(false);
        }
    }

    private void FindPPVolume()
    {
        //var ppv = FindObjectOfType<PostProcessVolume>();

        //if ( null == ppv )
        //{
        //    return;
        //}

        //m_PPVolume = ppv;
    }
}
