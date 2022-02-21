using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSync : SyncObject
{
    [SerializeField] private Transform m_Transform = null;
    [SerializeField] private float m_LerpRate = 10f;

    /* 大幅に離れてしまった場合のワープ処理. */
    [SerializeField] private bool m_SnapEnabled = true;
    [SerializeField] private float m_SnapThreshold = 5;

    /* 同期データ */
    private Vector3 m_LastUpdatedPos = Vector3.zero;
    private Quaternion m_LastUpdatedRot = Quaternion.identity;

    public override void UpdateForOwner()
    {
        
    }

    public override void UpdateForClient()
    {
        m_Transform.position = Vector3.Lerp(m_Transform.position, m_LastUpdatedPos, m_LerpRate * Time.deltaTime);
        m_Transform.rotation = Quaternion.Lerp(m_Transform.rotation, m_LastUpdatedRot, m_LerpRate * Time.deltaTime);

        if (m_SnapEnabled && (Vector3.Distance(m_Transform.position, m_LastUpdatedPos) > m_SnapThreshold))
        {
            m_Transform.position = m_LastUpdatedPos;
        }
    }

    public override void OnEnqueue(MonobitEngine.MonobitStream stream)
    {
        m_LastUpdatedPos = m_Transform.position;
        m_LastUpdatedRot = m_Transform.rotation;

        stream.Enqueue(m_LastUpdatedPos);
        stream.Enqueue(m_LastUpdatedRot);
    }

    public override void OnDequeue(MonobitEngine.MonobitStream stream)
    {
        m_LastUpdatedPos = (Vector3)stream.Dequeue();
        m_LastUpdatedRot = (Quaternion)stream.Dequeue();
    }
}
