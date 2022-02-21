using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VREyeSync : SyncObject
{
    [SerializeField] private Transform m_EyeL, m_EyeR;
    [SerializeField] private float m_LerpRate = 10f;

    /* 同期データ */
    private Vector3    m_LastUpdatedPosL, m_LastUpdatedPosR = Vector3.zero;
    private Quaternion m_LastUpdatedRotL, m_LastUpdatedRotR = Quaternion.identity;

    public override void UpdateForOwner()
    {
        //NOP
    }

    public override void UpdateForClient()
    {
        m_EyeL.localPosition = Vector3.Lerp(m_EyeL.localPosition, m_LastUpdatedPosL, m_LerpRate * Time.deltaTime);
        m_EyeL.localRotation = Quaternion.Lerp(m_EyeL.localRotation, m_LastUpdatedRotL, m_LerpRate * Time.deltaTime);

        m_EyeR.localPosition = Vector3.Lerp(m_EyeR.localPosition, m_LastUpdatedPosR, m_LerpRate * Time.deltaTime);
        m_EyeR.localRotation = Quaternion.Lerp(m_EyeR.localRotation, m_LastUpdatedRotR, m_LerpRate * Time.deltaTime);
    }

    public override void OnEnqueue(MonobitEngine.MonobitStream stream)
    {
        m_LastUpdatedPosL = m_EyeL.localPosition;
        m_LastUpdatedRotL = m_EyeL.localRotation;

        m_LastUpdatedPosR = m_EyeR.localPosition;
        m_LastUpdatedRotR = m_EyeR.localRotation;

        stream.Enqueue(m_LastUpdatedPosL);
        stream.Enqueue(m_LastUpdatedRotL);

        stream.Enqueue(m_LastUpdatedPosR);
        stream.Enqueue(m_LastUpdatedRotR);
    }

    public override void OnDequeue(MonobitEngine.MonobitStream stream)
    {
        m_LastUpdatedPosL = (Vector3)stream.Dequeue();
        m_LastUpdatedRotL = (Quaternion)stream.Dequeue();

        m_LastUpdatedPosR = (Vector3)stream.Dequeue();
        m_LastUpdatedRotR = (Quaternion)stream.Dequeue();
    }
}
