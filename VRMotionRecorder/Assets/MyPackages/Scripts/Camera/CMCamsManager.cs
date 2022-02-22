using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMCamsManager : MonoBehaviour
{

    [SerializeField] private string m_DisplayName = "Unknown";
    [SerializeField] private float m_BlendSec = 2f;
    [SerializeField] private List<CinemachineVirtualCamera> m_Cams;


    private List<CMCamInfo> m_CamInfo = new List<CMCamInfo>();

    private int m_CurrentCamIndex = 0;

    private void Start()
    {
        for (int i = 0; i < m_Cams.Count; ++i)
        {
            var info = new CMCamInfo();
            info.m_Cam = m_Cams[i];
            info.m_Composer = m_Cams[i].GetCinemachineComponent<CinemachineComposer>();
            info.m_DefaultPos = m_Cams[i].transform.position;
            info.m_DefaultRot = m_Cams[i].transform.rotation;
            info.m_DefaultFov = m_Cams[i].m_Lens.FieldOfView;
            m_CamInfo.Add(info);
        }
    }

    public string GetName()
    {
        return m_DisplayName;
    }

    public float GetBlendSec()
    {
        return m_BlendSec;
    }

    public CMCamInfo Activate()
    {
        m_CurrentCamIndex = 0;

        if (0 >= m_CamInfo.Count||
            (null == m_CamInfo[0]))
        {
            return null;
        }

        DisableAllCams();
        EnableCam(0);

        return m_CamInfo[0];
    }

    public void Deactivate()
    {
        DisableAllCams();
    }


    public CMCamInfo NextCamera()
    {
        if ((m_CurrentCamIndex + 1) < m_CamInfo.Count)
        {
            ++m_CurrentCamIndex;
        }
        else
        {
            m_CurrentCamIndex = 0;
        }

        if (m_CurrentCamIndex >= m_CamInfo.Count ||
            (null == m_CamInfo[m_CurrentCamIndex]))
        {
            return null;
        }

        DisableAllCams();
        EnableCam(m_CurrentCamIndex);

        return m_CamInfo[m_CurrentCamIndex];
    }

    public void ResetLocation()
    {
        if (m_CurrentCamIndex >= m_CamInfo.Count ||
            (null == m_CamInfo[m_CurrentCamIndex]))
        {
            return;
        }

        m_CamInfo[m_CurrentCamIndex].m_Cam.transform.position = m_CamInfo[m_CurrentCamIndex].m_DefaultPos;
        m_CamInfo[m_CurrentCamIndex].m_Cam.transform.rotation = m_CamInfo[m_CurrentCamIndex].m_DefaultRot;
        m_CamInfo[m_CurrentCamIndex].m_Cam.m_Lens.FieldOfView = m_CamInfo[m_CurrentCamIndex].m_DefaultFov;

        m_CamInfo[m_CurrentCamIndex].m_Composer.m_ScreenX = 0.5f;
        m_CamInfo[m_CurrentCamIndex].m_Composer.m_ScreenY = 0.5f;
    }

    private void DisableAllCams()
    {
        for ( int i = 0; i < m_CamInfo.Count; ++i )
        {
            if ( null == m_CamInfo[i] )
            {
                continue;
            }
            m_CamInfo[i].m_Cam.Priority = 0;
        }
    }

    private void EnableCam(int index)
    {
        if (index >= m_CamInfo.Count ||
            (null == m_CamInfo[index]))
        {
            return;
        }

        m_CamInfo[index].m_Cam.Priority = 10;
    }

    
}

[Serializable]
public class CMCamInfo
{
    public CinemachineVirtualCamera m_Cam;
    public CinemachineComposer m_Composer;
    public Vector3 m_DefaultPos;
    public Quaternion m_DefaultRot;
    public float m_DefaultFov;
}
