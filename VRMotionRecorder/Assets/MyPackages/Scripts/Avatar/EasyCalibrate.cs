using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//VRPlayerControllerのisMineチェックによりオーナーでのみ動作
public class EasyCalibrate : MonobitEngine.MonoBehaviour
{
    [SerializeField] private Transform m_Root;
    [SerializeField] private Transform m_CameraRig;
    [SerializeField] private GameObject m_Window;
    [SerializeField] private SlidetTextPair m_Scale;
    [SerializeField] private SlidetTextPair m_HeightOffset;
    [SerializeField] private IKTargetOffsets m_IKTargetOffsets;
    [SerializeField] private Dropdown m_DropDown;
    [SerializeField] private GameObject[] m_Offset;
    [SerializeField] private InputField[] m_Pos;
    [SerializeField] private InputField[] m_Rot;

    private AvatarCalibrationSettings m_AvatarCalibrationSettings;


    private static readonly string TEXT_FORMAT = "F3";
    private static readonly string CALIBRATE_KEY = "Calibrate";
    private static readonly string SETTINGS_PATH = "AvatarCalibrationSettings.json";

    void Start()
    {
        SetCanvasActive(false);

        m_Scale.slider.onValueChanged.AddListener(OnSlideScale);
        m_HeightOffset.slider.onValueChanged.AddListener(OnSlideHeightOffset);

        if (m_DropDown.value == 0)
        {
            SetOffsetUI(m_DropDown.value);
        }

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
    public void ChangeOffset()
    {
        switch (m_DropDown.value)
        {
            case 0:
                SetOffsetUI(m_DropDown.value);
                break;
            case 1:
                SetOffsetUI(m_DropDown.value);
                break;
            case 2:
                SetOffsetUI(m_DropDown.value);
                break;
            case 3:
                SetOffsetUI(m_DropDown.value);
                break;
            case 4:
                SetOffsetUI(m_DropDown.value);
                break;
            case 5:
                SetOffsetUI(m_DropDown.value); 
                break;
            case 6:
                SetOffsetUI(m_DropDown.value);
                break;
            case 7:
                SetOffsetUI(m_DropDown.value);
                break;
        }
    }
    private void SetOffsetUI(int offst_num)
    {
        m_Pos[0].text = m_Offset[offst_num].transform.localPosition.x.ToString();
        m_Pos[1].text = m_Offset[offst_num].transform.localPosition.y.ToString();
        m_Pos[2].text = m_Offset[offst_num].transform.localPosition.z.ToString();

        Vector3 local_angle_1 = m_Offset[offst_num].transform.localEulerAngles;

        if (local_angle_1.x > 180)
        {
            local_angle_1.x = local_angle_1.x - 360;
        }

        if (local_angle_1.y > 180)
        {
            local_angle_1.y = local_angle_1.y - 360;
        }

        if (local_angle_1.z > 180)
        {
            local_angle_1.z = local_angle_1.z - 360;
        }

        m_Rot[0].text = local_angle_1.x.ToString();
        m_Rot[1].text = local_angle_1.y.ToString();
        m_Rot[2].text = local_angle_1.z.ToString();
    }

    public void SetOffsetPos()
    {
        var pos = new Vector3(int.Parse(m_Pos[0].text), int.Parse(m_Pos[1].text), int.Parse(m_Pos[2].text));
        m_Offset[m_DropDown.value].transform.localPosition = pos;

        
    }

    public void SetOffsetRot()
    {
        var rot = Quaternion.Euler(new Vector3(int.Parse(m_Rot[0].text), int.Parse(m_Rot[1].text), int.Parse(m_Rot[2].text)));
        m_Offset[m_DropDown.value].transform.localRotation = rot;
       
    }
}       
