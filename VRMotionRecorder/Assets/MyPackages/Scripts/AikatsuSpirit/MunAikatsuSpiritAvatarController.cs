using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunAikatsuSpiritAvatarController : MonobitEngine.MonoBehaviour
{
    [SerializeField] private AikatsuSpiritAvatarLoader m_Loader;

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

    public void NextAvatar()
    {
        if (null == m_Loader)
        {
            return;
        }

        if ((m_CurrentAvatarIndex + 1) < m_Loader.GetAvatarCount())
        {
            ++m_CurrentAvatarIndex;
        }
        else
        {
            m_CurrentAvatarIndex = 0;
        }

        SetAvatarIndex(m_CurrentAvatarIndex);
    }

    public void PrevAvatar()
    {
        if (null == m_Loader)
        {
            return;
        }

        if ((m_CurrentAvatarIndex - 1) >= 0)
        {
            --m_CurrentAvatarIndex;
        }
        else
        {
            m_CurrentAvatarIndex = m_Loader.GetAvatarCount() - 1;
        }

        SetAvatarIndex(m_CurrentAvatarIndex);
    }

    public void SetAvatarIndex(int index)
    {
        m_CurrentAvatarIndex = index;

        Hashtable customParams = monobitView.owner.customParameters;
        customParams[AVATAR_ID] = index;
        MonobitEngine.MonobitNetwork.SetPlayerCustomParameters(customParams);
    }
    
    //ルーム内プレイヤーのパラメータが変更された際のコールバック
    void OnMonobitPlayerParametersChanged(object[] playerAndUpdatedParams)
    {
        var target = (MonobitPlayer)playerAndUpdatedParams[0];
        if ( monobitView.owner.ID != target.ID )
        {
            return;
        }

        var targetParam = (Hashtable)playerAndUpdatedParams[1];
        var id = (int)targetParam[AVATAR_ID];

        if (null != m_Loader)
        {
            m_Loader.Unload();
            m_Loader.Load(id);
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
