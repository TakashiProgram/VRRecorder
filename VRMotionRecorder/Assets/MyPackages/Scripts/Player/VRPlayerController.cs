using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
//using Invector;
//using Invector.vCharacterController;
//using Invector.vMelee;

public class VRPlayerController : AbstractPlayerController
{
    //[SerializeField] private AbstractVirtualWalker m_VW;
    //[SerializeField] private CMF.Mover m_Mover;
    [SerializeField] private GameObject m_CameraRig;
    [SerializeField] private List<GameObject> m_LocalObjs;
    //[SerializeField] private Rigidbody m_Rb;
    ////[SerializeField] private RegisterObject.Register m_Register;

    void Awake()
    {
        //ローカルにしか必要のないコンポーネントは削除する
        if (false == monobitView.isMine)
        {
            if (null != m_CameraRig)
            {
                m_CameraRig.SetActive(false);
            }

            for (int i = 0; i < m_LocalObjs.Count; ++i)
            {
                if (null == m_LocalObjs[i])
                {
                    continue;
                }
                m_LocalObjs[i].SetActive(false);
            }
        }
    }
}
