using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class VRIKRootController_Debug : MonoBehaviour
{
    [SerializeField] private VRIKRootController[] m_RootControllers;

    void Start()
    {
        for (int i = 0; i < m_RootControllers.Length; ++i)
        {
            if (null == m_RootControllers[i])
            {
                continue;
            }
            m_RootControllers[i].Calibrate();
        }
    }
}
