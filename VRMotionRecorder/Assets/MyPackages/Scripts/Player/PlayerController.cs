using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
//using Invector.vCharacterController;

public class PlayerController : AbstractPlayerController
{
    //[SerializeField] private vThirdPersonController m_TPController;
    //[SerializeField] private ThirdPersonInput m_TPInput;
    [SerializeField] private GameObject m_Camera;
    [SerializeField] private GameObject m_UI;

    void Awake()
    {
        if (false == monobitView.isMine)
        {
            //if (null != m_TPController)
            //{
            //    //m_TPController.enabled = false;
            //    Destroy(m_TPController);
            //}

            //if (null != m_TPInput)
            //{
            //    //m_TPController.enabled = false;
            //    Destroy(m_TPInput);
            //}

            if (null != m_Camera)
            {
                m_Camera.SetActive(false);
            }

            if (null != m_UI)
            {
                m_UI.SetActive(false);
            }
        }
    }
}
