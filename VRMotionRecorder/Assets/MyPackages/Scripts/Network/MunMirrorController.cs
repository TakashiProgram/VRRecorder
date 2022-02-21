using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunMirrorController : MonobitEngine.MonoBehaviour
{
    [SerializeField] private GameObject m_Mirror;

    private static readonly string PLAYER_TYPE = "PLAYER_TYPE";
    private static readonly string IS_DEBUG_MODE = "IS_DEBUG_MODE";

    void Start()
    {
        StartCoroutine(SetUp());
    }

    private IEnumerator SetUp()
    {
        SetActive(false);

        while (false == MonobitNetwork.player.customParameters.ContainsKey(PLAYER_TYPE))
        {
            yield return null;
        }

        if (false == IsNeedPlayerType())
        {
            yield break;
        }

        while (false == MonobitNetwork.room.customParameters.ContainsKey(IS_DEBUG_MODE))
        {
            yield return null;
        }

        var is_debug_mode = (bool)MonobitNetwork.room.customParameters[IS_DEBUG_MODE];

        SetActive(is_debug_mode);
    }


    //ルーム内プレイヤーのパラメータが変更された際のコールバック
    void OnMonobitCustomRoomParametersChanged(Hashtable peopertiesThatChanged)
    {
        if (false == IsNeedPlayerType())
        {
            return;
        }

        if (false == peopertiesThatChanged.ContainsKey(IS_DEBUG_MODE))
        {
            return;
        }

        var is_debug_mode = (bool)peopertiesThatChanged[IS_DEBUG_MODE];

        SetActive(is_debug_mode);
    }

    private void SetActive(bool is_active)
    {
        if ( null == m_Mirror )
        {
            return;
        }

        if ( is_active == m_Mirror.activeSelf )
        {
            return;
        }

        m_Mirror.SetActive(is_active);
    }

    private bool IsNeedPlayerType()
    {
        if (false == MonobitNetwork.player.customParameters.ContainsKey(PLAYER_TYPE))
        {
            return false;
        }

        var type = (PlayerType)MonobitNetwork.player.customParameters[PLAYER_TYPE];

        if ((PlayerType.VR_PLAYER == type) ||
            (PlayerType.FULL_BODY_VR_PLAYER == type))
        {
            return true;
        }

        return false;
    }
}
