using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//VRPlayerControllerのisMineチェックによりオーナーでのみ動作
public class EasyCalibrate : MonoBehaviour
{
    [SerializeField] private Transform m_Root;
    [SerializeField] private Transform m_CameraRig;
    [SerializeField] private GameObject m_Window;
    [SerializeField] private SlidetTextPair m_Scale;
    [SerializeField] private SlidetTextPair m_HeightOffset;

    private AvatarCalibrationSettings m_AvatarCalibrationSettings;

    private static readonly string TEXT_FORMAT = "F3";
    private static readonly string CALIBRATE_KEY = "Calibrate";
    private static readonly string SETTINGS_PATH = "AvatarCalibrationSettings.json";

    void Start()
    {
        SetCanvasActive(false);

        m_Scale.slider.onValueChanged.AddListener(OnSlideScale);
        m_HeightOffset.slider.onValueChanged.AddListener(OnSlideHeightOffset);

        Load();
    }

    void Update()
    {
        if (Input.GetButtonDown(CALIBRATE_KEY))
        {
            bool is_active = !m_Window.activeSelf;
            SetCanvasActive(is_active);
        }
    }

    public void Save()
    {
        JsonHelper<AvatarCalibrationSettings>.Write(SETTINGS_PATH, m_AvatarCalibrationSettings);
    }

    public void Load()
    {
        m_AvatarCalibrationSettings = JsonHelper<AvatarCalibrationSettings>.Read(SETTINGS_PATH);

        ChangeScale(m_AvatarCalibrationSettings.s_Scale);
        ChangeHeightOffset(m_AvatarCalibrationSettings.s_HeightOffset);
    }

    private void ChangeScale(float value)
    {
        m_CameraRig.localScale = new Vector3(value, value, value);
        m_AvatarCalibrationSettings.s_Scale = value;

        m_Scale.slider.value = value;
        m_Scale.text.text = value.ToString(TEXT_FORMAT);
    }

    private void ChangeHeightOffset(float value)
    {
        var pos = m_Root.localPosition;
        pos.y = value;
        m_Root.localPosition = pos;
        m_AvatarCalibrationSettings.s_HeightOffset = value;

        m_HeightOffset.slider.value = value;
        m_HeightOffset.text.text = value.ToString(TEXT_FORMAT);
    }

    private void SetCanvasActive(bool is_active)
    {
        if ( (null == m_Window) ||
            ( is_active == m_Window.activeSelf))
        {
            return;
        }

        m_Window.SetActive(is_active);

        if (true == is_active)
        {
            MouseCursorSettings.Instance.UnHide();
        }
        else
        {
            MouseCursorSettings.Instance.Hide();
        }
    }

    private void OnSlideScale(float value)
    {
        TrimSliderValue(ref value);
        m_Scale.text.text = value.ToString(TEXT_FORMAT);

        ChangeScale(value);
    }

    private void OnSlideHeightOffset(float value)
    {
        TrimSliderValue(ref value);
        m_HeightOffset.text.text = value.ToString(TEXT_FORMAT);

        ChangeHeightOffset(value);
    }

    private void TrimSliderValue(ref float value)
    {
        value = Mathf.Floor(value * 1000) / 1000;
    }
}
