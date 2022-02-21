using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GrabPose")]
public class GrabPose : ScriptableObject
{
    [Header("Offsets")]
    public Vector3 m_OffsetPosL = Vector3.zero;
    public Vector3 m_OffsetRotL = Vector3.zero;
    public Vector3 m_OffsetPosR = Vector3.zero;
    public Vector3 m_OffsetRotR = Vector3.zero;

    [Header("Curls")]
    [Range(-1, 1)] public float m_ThumbCurl = 0f;
    [Range(-1, 1)] public float m_IndexCurl = 0f;
    [Range(-1, 1)] public float m_MiddleCurl = 0f;
    [Range(-1, 1)] public float m_RingCurl = 0f;
    [Range(-1, 1)] public float m_PinkyCurl = 0f;

    [Header("Splays")]
    [Range(-1, 1)] public float m_ThumbIndexSplay = 0f;
    [Range(-1, 1)] public float m_IndexMiddleSplay = 0f;
    [Range(-1, 1)] public float m_MiddleRingSplay = 0f;
    [Range(-1, 1)] public float m_RingPinkySplay = 0f;
}
