using Entum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRMotionRecorder : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Window;

    [SerializeField]
    private String m_Name;

    private MotionDataRecorder[] m_MotionDataRecorder;
    
    private static readonly string DISPLAY_KEY = "MotionRecorder";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    public void CountAdd(InputField text)
    {
        int count = int.Parse(text.text);

        count++;

        text.text = count.ToString();
        switch (text.name)
        {
            case "Scene":
                for (int i = 0; i < m_MotionDataRecorder.Length; i++)
                {
                    m_MotionDataRecorder[i].SetScene(text.text);
                }
                    break;

            case "Cat":
                for (int i = 0; i < m_MotionDataRecorder.Length; i++)
                {
                    m_MotionDataRecorder[i].SetCat(text.text);
                }
                break;

            case "Take":
                for (int i = 0; i < m_MotionDataRecorder.Length; i++)
                {
                    m_MotionDataRecorder[i].SetTake(text.text);
                }
                break;
        }
    }

    public void CountTake(InputField text)
    {
        int count = int.Parse(text.text);

        count--;

        text.text = count.ToString();

        switch (text.name)
        {
            case "Scene":
                for (int i = 0; i < m_MotionDataRecorder.Length; i++)
                {
                    m_MotionDataRecorder[i].SetScene(text.text);
                }
                break;

            case "Cat":
                for (int i = 0; i < m_MotionDataRecorder.Length; i++)
                {
                    m_MotionDataRecorder[i].SetCat(text.text);
                }
                break;

            case "Take":
                for (int i = 0; i < m_MotionDataRecorder.Length; i++)
                {
                    m_MotionDataRecorder[i].SetTake(text.text);
                }
                break;
        }
    }

    public void MotionRecorder()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Recorder");
        Array.Resize(ref m_MotionDataRecorder,objects.Length);

        for (int i = 0; i < objects.Length; i++)
        {
            m_MotionDataRecorder[i] = objects[i].GetComponent<MotionDataRecorder>();
        }
    }

}
