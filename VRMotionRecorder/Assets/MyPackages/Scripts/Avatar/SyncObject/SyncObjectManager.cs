using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class SyncObjectManager : MonobitEngine.MonoBehaviour
{
    [SerializeField] private List<SyncObject> m_Objs;

    public void Add(SyncObject obj)
    {
        if ((null == obj) ||
            (true == m_Objs.Contains(obj)))
        {
            return;
        }

        m_Objs.Add(obj);
    }

    public void Remove(SyncObject obj)
    {
        if ((null == obj) ||
            (false == m_Objs.Contains(obj)))
        {
            return;
        }

        m_Objs.Remove(obj);
    }

    void Update()
    {
        if ((null == m_Objs) ||
            (0 >= m_Objs.Count))
        {
            return;
        }

        if (true == monobitView.isMine)
        {
            for (int i = 0; i < m_Objs.Count; ++i)
            {
                if (null == m_Objs[i]) continue;
                m_Objs[i].UpdateForOwner();
            }
        }
        else
        {
            for (int i = 0; i < m_Objs.Count; ++i)
            {
                if (null == m_Objs[i]) continue;
                m_Objs[i].UpdateForClient();
            }
        }

    }

    public void OnMonobitSerializeView(MonobitEngine.MonobitStream stream, MonobitEngine.MonobitMessageInfo info)
    {
        if ((null == m_Objs) ||
            (0 >= m_Objs.Count))
        {
            return;
        }

        if (stream.isWriting)
        {
            /*
             * 同期データの書き込み
             */
            for (int i = 0; i < m_Objs.Count; ++i)
            {
                if (null == m_Objs[i]) continue;
                m_Objs[i].OnEnqueue(stream);
            }
        }
        else
        {
            /*
             * 同期データの読み込み
             */
            for (int i = 0; i < m_Objs.Count; ++i)
            {
                if (null == m_Objs[i]) continue;
                m_Objs[i].OnDequeue(stream);
            }
        }
    }

}
