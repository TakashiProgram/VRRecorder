using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVSync : SyncObject
{
    [SerializeField] private Camera m_Camera = null;

    /* 同期データ */
    private float m_FOV = 0f;

    public override void UpdateForOwner()
    {

    }

    public override void UpdateForClient()
    {

    }

    public override void OnEnqueue(MonobitEngine.MonobitStream stream)
    {
        m_FOV = m_Camera.fieldOfView;

        stream.Enqueue(m_FOV);
    }

    public override void OnDequeue(MonobitEngine.MonobitStream stream)
    {
        m_FOV = (float)stream.Dequeue();

        m_Camera.fieldOfView = m_FOV;
    }
}
