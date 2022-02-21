using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SyncObject : MonoBehaviour
{
    private SyncObjectManager m_Manager;

    protected virtual void Start()
    {
        m_Manager = GetComponentInParent<SyncObjectManager>();
        if (null != m_Manager)
        {
            m_Manager.Add(this);
        }
    }

    protected virtual void OnDestroy()
    {
        if (null != m_Manager)
        {
            m_Manager.Remove(this);
        }
    }

    public abstract void UpdateForOwner();
    public abstract void UpdateForClient();
    public abstract void OnEnqueue(MonobitEngine.MonobitStream stream);
    public abstract void OnDequeue(MonobitEngine.MonobitStream stream);
}
