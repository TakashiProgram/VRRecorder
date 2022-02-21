using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlaybackCamera : MonoBehaviour
{
    public bool m_Enable = true;
    [SerializeField] private Camera m_PlaybackCamera = null;
    [SerializeField] private RawImage m_Image = null;

    private Camera m_MainCamera = null;
    private RenderTexture m_RenderTexture = null;
    private bool m_IsEnable = false;

    private static int TEXTURE_WIDTH = 960;
    private static int TEXTURE_HEIGHT = 540;

    void Start()
    {
        CreateNewTexture();
        SwitchUsePlayback(m_Enable);

        //FOVの値変化
        m_MainCamera.ObserveEveryValueChanged(x => x.fieldOfView)
            .Subscribe(_ => m_PlaybackCamera.fieldOfView = m_MainCamera.fieldOfView);
    }

    void Update()
    {
        if (m_IsEnable != m_Enable)
        {
            SwitchUsePlayback(m_Enable);
        }
    }

    public void SetMainCamera(Camera cam)
    {
        m_PlaybackCamera.CopyFrom(cam);
        m_MainCamera = cam;

        if (null == m_RenderTexture)
        {
            CreateNewTexture();
        }
        m_PlaybackCamera.targetTexture = m_RenderTexture;
    }

    private void SwitchUsePlayback(bool is_enable)
    {
        if (( null == m_Image ) ||
            ( null == m_PlaybackCamera ))
        {
            return;
        }

        if (is_enable != m_Image.gameObject.activeSelf)
        {
            m_Image.gameObject.SetActive(is_enable);
        }

        if (is_enable != m_PlaybackCamera.enabled)
        {
            m_PlaybackCamera.enabled = is_enable;
        }

        m_IsEnable = is_enable;
    }

    private void CreateNewTexture()
    {
        if (( null != m_RenderTexture) ||
            ( null == m_Image ) )
        {
            return;
        }

        m_RenderTexture = new RenderTexture(TEXTURE_WIDTH, TEXTURE_HEIGHT, 32, RenderTextureFormat.RGB565);

        m_Image.texture = m_RenderTexture;
    }
}
