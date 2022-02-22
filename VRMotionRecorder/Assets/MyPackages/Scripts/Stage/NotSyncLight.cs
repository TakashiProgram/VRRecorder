using MonobitEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotSyncLight : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private DirectionalLightRotator m_DirectionalLightRotator;

    [SerializeField]
    private Transform m_Rot;

    private static readonly string SYNC_ONOFF = "SyncOnOff";
    private static readonly string SYNC_LIGHT = "SyncLight";
    private static readonly string ROTATION = "Rotation";

    private bool m_IsSync = true;
    void Start()
    {
        m_DirectionalLightRotator.OnLightRota += OnLightRota;
    }

    // Update is called once per frame
    void Update()
    {
        SyncOnOff();
    }

    private void SyncOnOff()
    {
        if (Input.GetButtonDown(SYNC_ONOFF))
        {
            bool is_sync;
            if (true == m_IsSync)
            {
                is_sync = false;
            }
            else
            {
                is_sync = true;
            }

            monobitView.RPC(SYNC_LIGHT, MonobitTargets.All, is_sync);
        }
    }

    [MunRPC]
    private void SyncLight(bool is_sync)
    {
        m_IsSync = is_sync;

        if (true == m_IsSync)
        {
            m_DirectionalLightRotator.OnLightRota += OnLightRota;
        }
        else
        {
            m_DirectionalLightRotator.OnLightRota -= OnLightRota;
        }
    }

    private void OnLightRota(Vector3 rot)
    {
        monobitView.RPC(ROTATION, MonobitTargets.All, rot);
    }
    [MunRPC]
    private void Rotation(Vector3 rot)
    {
        m_Rot.eulerAngles = rot;
    }
}
