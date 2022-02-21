using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BGMatColorChanger : MonoBehaviour
{
    [SerializeField] private GameObject m_Window = null;
    [SerializeField] private Renderer m_BG = null;
    [SerializeField] private FlexibleColorPicker m_ColorPicker = null;

    [SerializeField] private Text m_RText = null;
    [SerializeField] private Text m_GText = null;
    [SerializeField] private Text m_BText = null;

    private Material m_Material = null;

    private static readonly string COLOR_CHANGE_KEY = "ChangeBGColor";


    void Start()
    {
        CreateNewMaterial();

        if (null != m_ColorPicker)
        {
            if (null != m_Material)
            {
                m_ColorPicker.color = m_Material.color;
            }

            m_ColorPicker.ObserveEveryValueChanged(x => x.color)
                .Subscribe(x => OnChangeColor(x));
        }

        m_Window.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown(COLOR_CHANGE_KEY))
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

    public void OnChangeColor(Color color)
    {
        if (null != m_Material)
        {
            m_Material.color = color;
        }

        if (null != m_RText)
        {
            int value = Mathf.FloorToInt(color.r * 255f);
            m_RText.text = value.ToString();
        }

        if (null != m_GText)
        {
            int value = Mathf.FloorToInt(color.g * 255f);
            m_GText.text = value.ToString();
        }

        if (null != m_BText)
        {
            int value = Mathf.FloorToInt(color.b * 255f);
            m_BText.text = value.ToString();
        }
    }

    private void CreateNewMaterial()
    {
        if ((null == m_BG)||
             (null == m_BG.material))
        {
            return;
        }

        m_Material = new Material(m_BG.material);

        m_BG.material = m_Material;
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
}
