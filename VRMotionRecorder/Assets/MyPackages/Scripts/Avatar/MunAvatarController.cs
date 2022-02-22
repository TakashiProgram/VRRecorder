using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using RootMotion.FinalIK;

public class MunAvatarController : MonobitEngine.MonoBehaviour
{
    [SerializeField] private AvatarLoader m_Loader;
    [SerializeField] private Calibrator m_Calibrator;

    private int m_CurrentAvatarIndex = -1;
    private static readonly string AVATAR_ID = "AVATAR_ID";
    private static readonly string PLAYER_NUM = "PLAYER_NUM";

    void Start()
    {
        StartCoroutine(SetUp());
    }

    void Update()
    {
        if (false == monobitView.isMine)
        {
            return;
        }

        //暫定：アバターが表示されない不具合が発生した際に手動でリロードする
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReloadAvatar();
        }
    }

    //void Update()
    //{
    //    if (false == monobitView.isMine)
    //    {
    //        return;
    //    }

    //    if (Input.GetButtonDown(CALIBRATE_KEY))
    //    {
    //        m_Calibrator.DelayCalibrate();
    //    }
    //}

    //void OnMonobitPlayerParametersChanged(object[] playerAndUpdatedParams)
    //{
    //    (MonobitPlayer)playerAndUpdatedParams[0]).ID
    //    monobitView.owner.ID
    //}

    //void OnOtherPlayerConnected(MonobitPlayer newPlayer)
    //{
    //}

    private IEnumerator SetUp()
    {
        while (false == monobitView.owner.customParameters.ContainsKey(AVATAR_ID))
        {
            yield return null;
        }
        int num = (int)monobitView.owner.customParameters[PLAYER_NUM];
        int id = (int)monobitView.owner.customParameters[AVATAR_ID];

        if (m_CurrentAvatarIndex == id)
        {
            yield break;
        }

        m_CurrentAvatarIndex = id;
        VRIK ik = null;
        VRIKRootController ik_root_controller = null;

        if (null != m_Loader)
        {
            m_Loader.Load(m_CurrentAvatarIndex, num, monobitView.isOwner, ref ik, ref ik_root_controller);
        }

        if (null != m_Calibrator)
        {
            m_Calibrator.Init(ik, ik_root_controller);
        }
    }

    private void ReloadAvatar()
    {
        Hashtable customParams = monobitView.owner.customParameters;
        if (false == customParams.ContainsKey(AVATAR_ID))
        {
            return;
        }

        var id = (int)customParams[AVATAR_ID];

        customParams[AVATAR_ID] = id;

        MonobitEngine.MonobitNetwork.SetPlayerCustomParameters(customParams);
        Debug.Log("MunAvatarController: avatar reloaded, AVATAR ID = " + id);
    }
}
