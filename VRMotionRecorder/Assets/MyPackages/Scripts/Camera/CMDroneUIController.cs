using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMDroneUIController : MonoBehaviour
{
    [SerializeField] private CMDroneController m_Controller = null;

    [SerializeField] private GameObject m_Window = null;
    [SerializeField] private Text m_Text = null;
    [SerializeField] private float m_DisplaySec = 1.5f;

    private float m_DisplayLeftSec = 0f;

    void Start()
    {
        if (null != m_Controller)
        {
            m_Controller.OnNextTarget += OnNextTarget;
        }   
    }

    void Update()
    {
        CountDisplayTime();
    }

    private void OnNextTarget(string name)
    {
        if (null != m_Text)
        {
            m_Text.text = name;
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
}
