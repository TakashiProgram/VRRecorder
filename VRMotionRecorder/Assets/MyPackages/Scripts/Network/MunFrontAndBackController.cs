using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunFrontAndBackController : MonobitEngine.MonoBehaviour
{
    [SerializeField] private string m_ParamName = "";
    [SerializeField] private KeyCode m_SwitchingKey = KeyCode.None;
    [SerializeField] private GameObject[] m_Items = null;
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
    private void AddParam()
    {
        Hashtable customParams = MonobitNetwork.room.customParameters;
        customParams[m_ParamName] = -1;
        MonobitNetwork.room.SetCustomParameters(customParams);

    }
    private IEnumerator LoadParam()
    {
        while (false == MonobitNetwork.room.customParameters.ContainsKey(m_ParamName))
        {
            yield return null;
        }

        var index = (int)MonobitNetwork.room.customParameters[m_ParamName];

    }
    void OnMonobitCustomRoomParametersChanged(Hashtable peopertiesThatChanged)
    {
        if (false == peopertiesThatChanged.ContainsKey(m_ParamName))
        {
            return;
        }

        var index = (int)peopertiesThatChanged[m_ParamName];

        SetRotate(index);
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

        SetRotate(index);
    }

    private void SetRotate(int index)
    {
        for (int i = 0; i < m_Items.Length; ++i)
        {
            SetRotate(m_Items[i], false);
        }

        if (-1 >= index)
        {
            return;
        }

        SetRotate(m_Items[index], true);
    }

    private void SetRotate(GameObject obj, bool is_rotate)
    {
        if (null == obj)
        {
            return;
        }
        var rot = obj.transform.rotation;
        if (true == is_rotate)
        {
            rot.y = 180;

        }
        else
        {
            rot.y = 0;
        }
        obj.transform.rotation = rot;
    }
}
