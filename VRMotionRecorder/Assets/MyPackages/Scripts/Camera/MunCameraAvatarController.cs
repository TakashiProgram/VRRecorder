using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunCameraAvatarController : MonobitEngine.MonoBehaviour
{
    [SerializeField] private CameraAvatarLoader m_Loader;

    private int m_CurrentAvatarIndex = -1;
    private static readonly string AVATAR_ID = "AVATAR_ID";

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

    private IEnumerator SetUp()
    {
        while (false == monobitView.owner.customParameters.ContainsKey(AVATAR_ID))
        {
            yield return null;
        }

        int id = (int)monobitView.owner.customParameters[AVATAR_ID];

        if (m_CurrentAvatarIndex == id)
        {
            yield break;
        }

        m_CurrentAvatarIndex = id;

        if (null != m_Loader)
        {
            m_Loader.Load(m_CurrentAvatarIndex);
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
