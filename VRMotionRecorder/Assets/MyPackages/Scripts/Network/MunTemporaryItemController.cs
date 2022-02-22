using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunTemporaryItemController : MonobitEngine.MonoBehaviour
{
    [SerializeField] private string m_ParamName = "";
    [SerializeField] private KeyCode m_SwitchingKey = KeyCode.None;
    [SerializeField] private GameObject[] m_Items = null;
    [SerializeField] private bool m_IsActive = false;

    void Start()
    {
        if (MonobitNetwork.isHost)
        {
            AddParam();
        }
        else
        {
            
            StartCoroutine(LoadParam());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(m_SwitchingKey))
        {
            SwitchParam();
        }
    }

    private IEnumerator LoadParam()
    {
        while (false == MonobitNetwork.room.customParameters.ContainsKey(m_ParamName))
        {
            yield return null;
        }

        var index = (int)MonobitNetwork.room.customParameters[m_ParamName];
       
            SetActive(index);
    }

    //ルーム内プレイヤーのパラメータが変更された際のコールバック
    void OnMonobitCustomRoomParametersChanged(Hashtable peopertiesThatChanged)
    {
        if (false == peopertiesThatChanged.ContainsKey(m_ParamName))
        {
            return;
        }

        var index = (int)peopertiesThatChanged[m_ParamName];

        SetActive(index);
    }

    private void AddParam()
    {
        Hashtable customParams = MonobitNetwork.room.customParameters;
        var count = 0;
        if (true == m_IsActive)
        {
            customParams[m_ParamName] = 0;
            count = 0;
        }
        else
        {
            customParams[m_ParamName] = -1;
            count = - 1;
        }

        MonobitNetwork.room.SetCustomParameters(customParams);

        SetActive(count);
    }

    private void SwitchParam()
    {
        Hashtable customParams = MonobitNetwork.room.customParameters;
        if (false == customParams.ContainsKey(m_ParamName))
        {
            return;
        }
        var index = (int)customParams[m_ParamName];

        if ((m_Items.Length - 1) > index)
        {
            index++;
        }
        else
        {
            index = -1;
        }
        customParams[m_ParamName] = index;
        MonobitNetwork.room.SetCustomParameters(customParams);
        
        SetActive(index);        
    }

    private void SetActive(int index)
    {
        for (int i = 0; i < m_Items.Length; ++i)
        {
            SetActive(m_Items[i], false);
        }

        if (-1 >= index)
        {
            return;
        }

        SetActive(m_Items[index], true);
    }

    private void SetActive(GameObject obj, bool is_active)
    {
        if ((null == obj) ||
            (is_active == obj.activeSelf))
        {
            return;
        }

        obj.SetActive(is_active);
    }
}
