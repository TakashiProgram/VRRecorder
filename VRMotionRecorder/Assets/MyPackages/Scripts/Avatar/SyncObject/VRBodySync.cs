using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBodySync : SyncObject
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_LerpRate = 10f;

    /* 大幅に離れてしまった場合のワープ処理. */
    [SerializeField] private bool m_SnapEnabled = true;
    [SerializeField] private float m_SnapThreshold = 5;

    /* 同期データ */
    private Vector3 m_LastUpdatedPos = Vector3.zero;
    private Quaternion m_LastUpdatedRot = Quaternion.identity;

    public override void UpdateForOwner()
    {
        transform.position = m_Target.position;
        transform.rotation = m_Target.rotation;
    }

    public override void UpdateForClient()
    {
        transform.position = Vector3.Lerp(transform.position, m_LastUpdatedPos, m_LerpRate * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, m_LastUpdatedRot, m_LerpRate * Time.deltaTime);

        if (m_SnapEnabled && (Vector3.Distance(transform.position, m_LastUpdatedPos) > m_SnapThreshold))
        {
            transform.position = m_LastUpdatedPos;
        }
    }

    public override void OnEnqueue(MonobitEngine.MonobitStream stream)
    {
        m_LastUpdatedPos = transform.position;
        m_LastUpdatedRot = transform.rotation;

        stream.Enqueue(m_LastUpdatedPos);
        stream.Enqueue(m_LastUpdatedRot);
    }

    public override void OnDequeue(MonobitEngine.MonobitStream stream)
    {
        m_LastUpdatedPos = (Vector3)stream.Dequeue();
        m_LastUpdatedRot = (Quaternion)stream.Dequeue();
    }
}
