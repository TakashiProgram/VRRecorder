using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunReturnController : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject m_Return;

    private static readonly string BUTTON_RETURN = "Drone_Return";


    void Update()
    {
        if (false == monobitView.isMine)
        {
            return;
        }

        ReturnActive();
    }

    private void ReturnActive()
    {
        if (false == Input.GetButtonDown(BUTTON_RETURN))
        {
            return;
        }
        bool is_return;
        if (true == m_Return.activeSelf)
        {
            is_return = false;
        }
        else
        {
            is_return = true;
        }
        monobitView.RPC("Return",MonobitTargets.All, is_return);
    }

    [MunRPC]
    private void Return(bool is_return)
    {
        m_Return.SetActive(is_return);
    }
}
