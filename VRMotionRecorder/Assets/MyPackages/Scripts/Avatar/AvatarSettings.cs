using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSettings : MonoBehaviour
{
    public Action<bool> OnOwnerChanged;

    [SerializeField] private IKTargetOffsets m_IKTargetOffsets;
    [SerializeField] private VRFingerSync m_FingerSyncL;
    [SerializeField] private VRFingerSync m_FingerSyncR;

    public IKTargetOffsets GetIKTargetOffsets()
    {
        return m_IKTargetOffsets;
    }

    public VRFingerSync GetFingerSyncL()
    {
        return m_FingerSyncL;
    }

    public VRFingerSync GetFingerSyncR()
    {
        return m_FingerSyncR;
    }
}
