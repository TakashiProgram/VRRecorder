using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunPlayBackCameraManager : MonobitEngine.MonoBehaviour
{
    [SerializeField] private PlaybackCamera m_PlaybackCamera = null;

    private static readonly string PLAYER_TYPE = "PLAYER_TYPE";

    void Awake()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        while (false == MonobitNetwork.player.customParameters.ContainsKey(PLAYER_TYPE))
        {
            yield return null;
        }

        var type = (PlayerType)MonobitNetwork.player.customParameters[PLAYER_TYPE];

        if ( true == IsNeedPlayerType() )
        {
            if (null != m_PlaybackCamera)
            {
                m_PlaybackCamera.m_Enable = true;
            }
        }
        else
        {
            if (null != m_PlaybackCamera)
            {
                m_PlaybackCamera.m_Enable = false;
            }
        }
    }

    private bool IsNeedPlayerType()
    {
        var type = (PlayerType)MonobitNetwork.player.customParameters[PLAYER_TYPE];

        if ((PlayerType.VR_PLAYER == type) ||
            (PlayerType.FULL_BODY_VR_PLAYER == type))
        {
            return true;
        }

        return false;
    }
}
