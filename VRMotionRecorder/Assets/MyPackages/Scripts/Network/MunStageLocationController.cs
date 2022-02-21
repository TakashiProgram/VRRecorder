using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunStageLocationController : MonobitEngine.MonoBehaviour
{
    [SerializeField] private GameObject m_Window = null;
    [SerializeField] private Transform m_Dummy = null;

    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private float m_RotateSpeed = 1f;

    private static readonly string OFFSET_POS = "OFFSET_POS";
    private static readonly string OFFSET_ROT = "OFFSET_ROT";

    private bool m_IsEnabled = false;

    void Update()
    {
        if (false == MonobitNetwork.inRoom)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchEnabled();
        }

        if ( false == m_IsEnabled)
        {
            return;
        }

        MovePlane();
        Rotate();
    }

    public void Done()
    {
        var pos = m_Dummy.position;
        var rot = m_Dummy.rotation;

        Hashtable customParams = MonobitNetwork.room.customParameters;
        customParams[OFFSET_POS] = pos.ToString();
        customParams[OFFSET_ROT] = rot.eulerAngles.ToString();
        MonobitNetwork.room.SetCustomParameters(customParams);

        m_Dummy.localPosition = Vector3.zero;
        m_Dummy.localRotation = Quaternion.identity;
    }

    public void ResetLocation()
    {
        if ((false == MonobitEngine.MonobitNetwork.room.customParameters.ContainsKey(OFFSET_POS)) ||
            (false == MonobitEngine.MonobitNetwork.room.customParameters.ContainsKey(OFFSET_ROT)))
        {
            return;
        }

        var offset_pos_str = (string)MonobitEngine.MonobitNetwork.room.customParameters[OFFSET_POS];
        var offset_rot_str = (string)MonobitEngine.MonobitNetwork.room.customParameters[OFFSET_ROT];

        m_Dummy.position = StringToVector3(offset_pos_str);
        m_Dummy.rotation = Quaternion.Euler(StringToVector3(offset_rot_str));
    }

    private void SwitchEnabled()
    {
        if (false == m_IsEnabled)
        {
            MouseCursorSettings.Instance.UnHide();
            m_Window.SetActive(true);
            m_Dummy.gameObject.SetActive(true);
            m_IsEnabled = true;
        }
        else
        {
            MouseCursorSettings.Instance.Hide();
            m_Window.SetActive(false);
            m_Dummy.gameObject.SetActive(false);
            m_IsEnabled = false;
        }
    }

    private void MovePlane()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float depth = Input.GetAxis("Vertical");

        m_Dummy.Translate(Vector3.right * horizontal * m_MoveSpeed * Time.deltaTime, Space.Self);
        m_Dummy.Translate(Vector3.forward * depth * m_MoveSpeed * Time.deltaTime, Space.Self);
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            m_Dummy.Rotate(Vector3.up, m_RotateSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            m_Dummy.Rotate(Vector3.up, -m_RotateSpeed * Time.deltaTime, Space.World);
        }
    }

    private Vector3 StringToVector3(string str)
    {
        if (str.StartsWith("(") && str.EndsWith(")"))
        {
            str = str.Substring(1, str.Length - 2);
        }

        // split the items
        string[] array = str.Split(',');

        return new Vector3(
            float.Parse(array[0]),
            float.Parse(array[1]),
            float.Parse(array[2]));
    }
}
