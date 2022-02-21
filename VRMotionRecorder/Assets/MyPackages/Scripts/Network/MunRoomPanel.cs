using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonobitEngine;

public class MunRoomPanel : MonobitEngine.MonoBehaviour
{
    public Action<string> OnJoinRoom;

    [SerializeField] private Text m_RoomNameText = null;
    [SerializeField] private Text m_ConnectNumText = null;

    private string m_RoomName = "";

    public void Init(string room_name)
    {
        m_RoomName = room_name;

        if ( null != m_RoomNameText )
        {
            m_RoomNameText.text = room_name;
        }
    }

    public Text GetConnectNumText()
    {
        return m_ConnectNumText;
    }

    public void Join()
    {
        if (null != OnJoinRoom)
        {
            OnJoinRoom(m_RoomName);
        }
    }
}
