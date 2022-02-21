using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通信を使用せずに特定のSyncObjectのUpdateForOwnerを呼び出すデバッグ用クラス
/// </summary>
public class LocalSyncObjUpdate : MonoBehaviour
{
    [SerializeField] private SyncObject m_SyncObject = null;

    void Update()
    {
        if (null != m_SyncObject)
        {
            m_SyncObject.UpdateForOwner();
        }
    }
}
