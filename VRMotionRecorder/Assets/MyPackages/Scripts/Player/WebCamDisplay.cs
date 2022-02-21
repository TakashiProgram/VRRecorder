using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamDisplay : MonoBehaviour
{
    [SerializeField] private RawImage m_Image;
    [SerializeField] private Transform m_ImageRoot;
    [SerializeField] private Transform m_Head;
    [Header("Canvas")]
    [SerializeField] private GameObject m_Window;
    [SerializeField] private Dropdown m_WebCamIndexDropDown;
    [SerializeField] private InputField m_ScaleInputField;
    [SerializeField] private InputField m_HeightInputField;
    [SerializeField] private InputField m_DepthInputField;
    [SerializeField] private InputField m_SlopeInputField;

    private WebCamDisplaySettings m_WebCamDisplaySettings;
    private WebCamTexture m_WebCamTexture = null;

    private Vector3 m_OffsetPos = Vector3.zero;
    private Vector3 m_OffsetRot = Vector3.zero;

    private static readonly int WIDTH = 1920;
    private static readonly int HEIGHT = 1080;
    private static readonly int FPS = 30;

    private static readonly string TEXT_FORMAT = "F1";
    private static readonly string DISPLAY_KEY = "WebCamDisplay";
    private static readonly string SETTINGS_PATH = "WebCamDisplaySettings.json";

    void Start()
    {
        m_ScaleInputField.onEndEdit.AddListener(OnChangeScale);
        m_HeightInputField.onEndEdit.AddListener(OnChangeHeight);
        m_DepthInputField.onEndEdit.AddListener(OnChangeDepth);
        m_SlopeInputField.onEndEdit.AddListener(OnChangeSlope);
        m_WebCamIndexDropDown.onValueChanged.AddListener(OnSelectWebCam);

        Load();

        m_Window.SetActive(false);
    }

    void Update()
    {
        FollowHead();

        if (Input.GetButtonDown(DISPLAY_KEY))
        {
            bool is_active = !m_Window.activeSelf;
            if (is_active)
            {
                OpenWindow();
            }
            else
            {
                CloseWindow();
            }
        }
    }

    public void Save()
    {
        JsonHelper<WebCamDisplaySettings>.Write(SETTINGS_PATH, m_WebCamDisplaySettings);
    }

    public void Load()
    {
        m_WebCamDisplaySettings = JsonHelper<WebCamDisplaySettings>.Read(SETTINGS_PATH);

        m_ImageRoot.localScale = new Vector3(m_WebCamDisplaySettings.s_Scale, 1.0f, m_WebCamDisplaySettings.s_Scale);//子のRotationの都合でx,yでなくx,zでスケールする
        m_ScaleInputField.text = m_WebCamDisplaySettings.s_Scale.ToString(TEXT_FORMAT);

        m_OffsetPos = new Vector3(0f, m_WebCamDisplaySettings.s_Height, m_WebCamDisplaySettings.s_Depth);
        m_OffsetRot = new Vector3(m_WebCamDisplaySettings.s_Slope, 0f, 0f);

        m_HeightInputField.text = m_WebCamDisplaySettings.s_Height.ToString(TEXT_FORMAT);
        m_DepthInputField.text = m_WebCamDisplaySettings.s_Depth.ToString(TEXT_FORMAT);
        m_SlopeInputField.text = m_WebCamDisplaySettings.s_Slope.ToString(TEXT_FORMAT);


        WebCamDevice[] devices = WebCamTexture.devices;
        m_WebCamIndexDropDown.ClearOptions();

        if ( 0 >= devices.Length )
        {
            ActiveImage(false);
            return;
        }

        var list = new List<string>();
        for (int i = 0; i < devices.Length; ++i)
        {
            list.Add(devices[i].name);
        }

        m_WebCamIndexDropDown.AddOptions(list);
        m_WebCamIndexDropDown.value = Mathf.Clamp(m_WebCamDisplaySettings.s_WebCamIndex, 0, devices.Length - 1);

        ChangeWebCam(m_WebCamIndexDropDown.value);
    }

    public void OnChangeScale(string text)
    {
        float value = float.Parse(text);
        m_WebCamDisplaySettings.s_Scale = value;

        m_ImageRoot.localScale = new Vector3(value, 1.0f, value); //子のRotationの都合でx,yでなくx,zでスケールする
    }

    public void OnChangeHeight(string text)
    {
        float value = float.Parse(text);
        m_WebCamDisplaySettings.s_Height = value;
        m_OffsetPos.y = value;
    }

    public void OnChangeDepth(string text)
    {
        float value = float.Parse(text);
        m_WebCamDisplaySettings.s_Depth = value;
        m_OffsetPos.z = value;
    }

    public void OnChangeSlope(string text)
    {
        float value = float.Parse(text);
        m_WebCamDisplaySettings.s_Slope = value;
        m_OffsetRot.x = value;
    }

    public void OnSelectWebCam(int index)
    {
        m_WebCamDisplaySettings.s_WebCamIndex = index;
        ChangeWebCam(index);
    }

    private void ChangeWebCam( int index )
    {
        if (null != m_WebCamTexture)
        {
            if (true == m_WebCamTexture.isPlaying)
            {
                m_WebCamTexture.Stop();
            }
            Destroy(m_WebCamTexture);
            m_WebCamTexture = null;
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        m_WebCamTexture = new WebCamTexture(devices[index].name, WIDTH, HEIGHT, FPS);
        m_Image.texture = m_WebCamTexture;
        m_WebCamTexture.Play();
    }


    private void OpenWindow()
    {
        if ((null == m_Window) ||
            (true == m_Window.activeSelf))
        {
            return;
        }

        m_Window.SetActive(true);

        MouseCursorSettings.Instance.UnHide();
    }

    private void CloseWindow()
    {
        if ((null == m_Window) ||
            (false == m_Window.activeSelf))
        {
            return;
        }

        m_Window.SetActive(false);

        MouseCursorSettings.Instance.Hide();
    }

    private void ActiveImage(bool is_active)
    {
        if ((null == m_Image) ||
            (is_active == m_Image.gameObject.activeSelf))
        {
            return;
        }

        m_Image.gameObject.SetActive(is_active);
    }

    public void SwitchActiveImage()
    {
        if (null == m_Image)
        {
            return;
        }

        if (false == m_Image.gameObject.activeSelf)
        {
            ActiveImage(true);
        }
        else
        {
            ActiveImage(false);
        }
    }

    private void FollowHead()
    {
        if ((null == m_ImageRoot)||
            (null == m_Head))
        {
            return;
        }

        var head_pos = m_Head.position;
        var head_forward = new Vector3(m_Head.forward.x, 0f, m_Head.forward.z).normalized;

        m_ImageRoot.position = head_pos + Vector3.up * m_OffsetPos.y + head_forward * m_OffsetPos.z ;
        m_ImageRoot.LookAt(new Vector3(head_pos.x, m_ImageRoot.position.y, head_pos.z), Vector3.up);

        var rot = m_ImageRoot.localRotation.eulerAngles;
        rot.x = m_OffsetRot.x;
        m_ImageRoot.localRotation = Quaternion.Euler(rot);
    }



    [System.Serializable]
    private struct WebCamDisplaySettings
    {
        public int s_WebCamIndex;
        public float s_Scale;
        public float s_Height;
        public float s_Depth;
        public float s_Slope;
    }
}
