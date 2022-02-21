using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCalibrationUIController : MonoBehaviour
{
    [SerializeField] private GameObject m_Window;
    [SerializeField] private SlidetTextPair m_Scale;
    [SerializeField] private SlidetTextPair m_HeadRotationOffsetX;
    [SerializeField] private SlidetTextPair m_HeadRotationOffsetY;
    [SerializeField] private SlidetTextPair m_HeadRotationOffsetZ;
    [SerializeField] private SlidetTextPair m_LeftShoulderRotationWeight;
    [SerializeField] private SlidetTextPair m_LeftArmLengthMlp;
    [SerializeField] private SlidetTextPair m_RightShoulderRotationWeight;
    [SerializeField] private SlidetTextPair m_RightArmLengthMlp;

    private AvatarCalibrator m_AvatarCalibrator;

    private Vector3 m_HeadRotationOffsetValue = Vector3.zero;

    private static readonly string TEXT_FORMAT = "F2";

    void Start()
    {
        m_Scale.slider.onValueChanged.AddListener(OnSlideScale);
        m_HeadRotationOffsetX.slider.onValueChanged.AddListener(OnChangeHeadRotationOffsetX);
        m_HeadRotationOffsetY.slider.onValueChanged.AddListener(OnChangeHeadRotationOffsetY);
        m_HeadRotationOffsetZ.slider.onValueChanged.AddListener(OnChangeHeadRotationOffsetZ);
        m_LeftShoulderRotationWeight.slider.onValueChanged.AddListener(OnSlideLeftShoulderRotationWeight);
        m_RightShoulderRotationWeight.slider.onValueChanged.AddListener(OnSlideRightShoulderRotationWeight);
        m_LeftArmLengthMlp.slider.onValueChanged.AddListener(OnSlideLeftArmLengthMlp);
        m_RightArmLengthMlp.slider.onValueChanged.AddListener(OnSlideRightArmLengthMlp);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchActiveWindow();
        }
    }

    public void Save()
    {
        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.Save();
    }

    public void Load()
    {
        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.Load();
    }

    public void OnSlideScale(float value)
    {
        TrimSliderValue(ref value);
        m_Scale.text.text = value.ToString(TEXT_FORMAT);

        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.ChangeScale(value);
    }

    public void OnChangeHeadRotationOffsetX(float value)
    {
        TrimSliderValue(ref value);
        m_HeadRotationOffsetX.text.text = value.ToString(TEXT_FORMAT);
        m_HeadRotationOffsetValue.x = value;

        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.ChangeHeadRotationOffset(m_HeadRotationOffsetValue);
    }

    public void OnChangeHeadRotationOffsetY(float value)
    {
        TrimSliderValue(ref value);
        m_HeadRotationOffsetY.text.text = value.ToString(TEXT_FORMAT);
        m_HeadRotationOffsetValue.y = value;

        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.ChangeHeadRotationOffset(m_HeadRotationOffsetValue);
    }

    public void OnChangeHeadRotationOffsetZ(float value)
    {
        TrimSliderValue(ref value);
        m_HeadRotationOffsetZ.text.text = value.ToString(TEXT_FORMAT);
        m_HeadRotationOffsetValue.z = value;

        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.ChangeHeadRotationOffset(m_HeadRotationOffsetValue);
    }

    public void OnSlideLeftShoulderRotationWeight(float value)
    {
        TrimSliderValue(ref value);
        m_LeftShoulderRotationWeight.text.text = value.ToString(TEXT_FORMAT);

        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.ChangeLeftShoulderRotationWeight(value);
    }

    public void OnSlideRightShoulderRotationWeight(float value)
    {
        TrimSliderValue(ref value);
        m_RightShoulderRotationWeight.text.text = value.ToString(TEXT_FORMAT);

        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.ChangeRightShoulderRotationWeight(value);
    }

    public void OnSlideLeftArmLengthMlp(float value)
    {
        TrimSliderValue(ref value);
        m_LeftArmLengthMlp.text.text = value.ToString(TEXT_FORMAT);

        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.ChangeLeftArmLenghtMlp(value);
    }

    public void OnSlideRightArmLengthMlp(float value)
    {
        TrimSliderValue(ref value);
        m_RightArmLengthMlp.text.text = value.ToString(TEXT_FORMAT);

        if (null == m_AvatarCalibrator)
        {
            return;
        }
        m_AvatarCalibrator.ChangeRightArmLenghtMlp(value);
    }

    private void OnLoadCompleted(AvatarCalibrationSettings settings)
    {
        m_Scale.slider.value = settings.s_Scale;
        m_Scale.text.text = settings.s_Scale.ToString(TEXT_FORMAT);

        m_HeadRotationOffsetX.slider.value = settings.s_HeadRotationOffset.x;
        m_HeadRotationOffsetX.text.text = settings.s_HeadRotationOffset.x.ToString(TEXT_FORMAT);
        m_HeadRotationOffsetY.slider.value = settings.s_HeadRotationOffset.y;
        m_HeadRotationOffsetY.text.text = settings.s_HeadRotationOffset.y.ToString(TEXT_FORMAT);
        m_HeadRotationOffsetZ.slider.value = settings.s_HeadRotationOffset.z;
        m_HeadRotationOffsetZ.text.text = settings.s_HeadRotationOffset.z.ToString(TEXT_FORMAT);

        m_LeftShoulderRotationWeight.slider.value = settings.s_LeftShoulderRotationWeight;
        m_LeftShoulderRotationWeight.text.text = settings.s_LeftShoulderRotationWeight.ToString(TEXT_FORMAT);

        m_RightShoulderRotationWeight.slider.value = settings.s_RightShoulderRotationWeight;
        m_RightShoulderRotationWeight.text.text = settings.s_RightShoulderRotationWeight.ToString(TEXT_FORMAT);

        m_LeftArmLengthMlp.slider.value = settings.s_LeftArmLengthMlp;
        m_LeftArmLengthMlp.text.text = settings.s_LeftArmLengthMlp.ToString(TEXT_FORMAT);

        m_RightArmLengthMlp.slider.value = settings.s_RightArmLengthMlp;
        m_RightArmLengthMlp.text.text = settings.s_RightArmLengthMlp.ToString(TEXT_FORMAT);
    }

    private void TrimSliderValue(ref float value)
    {
        value = Mathf.Floor(value * 100) / 100;
    }

    private void SwitchActiveWindow()
    {
        if ( null == m_Window )
        {
            return;
        }

        if ( ( true == m_Window.activeSelf ) ||
            (null != m_AvatarCalibrator))
        {
            m_Window.SetActive(!m_Window.activeSelf);
            return;
        }

        AvatarCalibrator[] calibs = FindObjectsOfType<AvatarCalibrator>();
        for (int i = 0; i < calibs.Length; ++i)
        {
            //if (false == calibs[i].IsMine())
            //{
            //    continue;
            //}

            m_AvatarCalibrator = calibs[i];
            m_AvatarCalibrator.OnLoadCompleted += OnLoadCompleted;

            Load();

            m_Window.SetActive(true);
            break;
        }
    }
}

[Serializable]
public struct SlidetTextPair
{
    public Slider slider;
    public Text text;
}
