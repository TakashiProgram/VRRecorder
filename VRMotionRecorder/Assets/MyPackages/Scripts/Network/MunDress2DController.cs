using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunDress2DController : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject m_SurveillanceCamera;
    
    private static readonly string PLAYER_TYPE = "PLAYER_TYPE";
    private static readonly string IS_DRESS2D = "IS_DRESS2D";

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
        while (false == MonobitNetwork.room.customParameters.ContainsKey(IS_DRESS2D))
        {
            yield return null;
        }
        var is_surveillance = (bool)MonobitNetwork.room.customParameters[IS_DRESS2D];

        SetActive(is_surveillance);
    }
    //ルーム内プレイヤーのパラメータが変更された際のコールバック
    void OnMonobitCustomRoomParametersChanged(Hashtable peopertiesThatChanged)
    {
        //if (false == IsNeedPlayerType())
        //{
        //    return;
        //}

        if (false == peopertiesThatChanged.ContainsKey(IS_DRESS2D))
        {
            return;
        }

        var is_debug_mode = (bool)peopertiesThatChanged[IS_DRESS2D];

        SetActive(is_debug_mode);
    }
    private void SetActive(bool is_active)
    {
        if (null == m_SurveillanceCamera)
        {
            return;
        }

        if (is_active == m_SurveillanceCamera.activeSelf)
        {
            return;
        }

        m_SurveillanceCamera.SetActive(is_active);
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
