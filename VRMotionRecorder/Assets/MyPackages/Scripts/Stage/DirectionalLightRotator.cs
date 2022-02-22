using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DirectionalLightRotator : MonoBehaviour
{
    [SerializeField] private GameObject m_Window = null;
    [SerializeField] private Text m_Text = null;
    //[SerializeField] private SyncObjectManager m_SyncObjectManager;
    [SerializeField] private float m_RotateSpeed = 50f;
    [SerializeField] private float m_DisplaySec = 1.5f;

    public Action<Vector3> OnLightRota;


    private float m_DisplayLeftSec = 0f;

    private static readonly string ROTATE_BUTTON = "RotateDirectionalLight";
    private static readonly string TEXT_FORMAT = "F1";

    void Update()
    {
       
        InputKey();
        CountDisplayTime();
    }

    private void InputKey()
    {
        float value = Input.GetAxis(ROTATE_BUTTON);
        if ( 0f == value )
        {
            return;
        }

         transform.Rotate(Vector3.up, value * m_RotateSpeed * Time.deltaTime, Space.World);
        if (null != OnLightRota)
        {
            OnLightRota(transform.rotation.eulerAngles);
        }
        if (null != m_Text)
        {
            float y = transform.rotation.eulerAngles.y;
            m_Text.text = RoundValue(y).ToString(TEXT_FORMAT);
        }

        m_DisplayLeftSec = m_DisplaySec;
    }

    private void CountDisplayTime()
    {
        if (0f < m_DisplayLeftSec)
        {
            m_DisplayLeftSec -= Time.deltaTime;
        }

        if ((false == m_Window.activeSelf) &&
            (0f < m_DisplayLeftSec))
        {
            m_Window.SetActive(true);
        }

        if ((true == m_Window.activeSelf) &&
            (0f >= m_DisplayLeftSec))
        {
            m_Window.SetActive(false);
        }
    }

 


    private float RoundValue(float value)
    {
        while ( 360f <= value )
        {
            value -= 360f;
        }

        while ( 0f > value)
        {
            value += 360f;
        }

        return value;
    }
}
