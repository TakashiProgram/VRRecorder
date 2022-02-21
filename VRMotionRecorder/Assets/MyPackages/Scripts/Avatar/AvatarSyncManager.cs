using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSyncManager : MonobitEngine.MunMonoBehaviour
{
    [SerializeField] private AvatarCalibrator m_AvatarCalibrator;

    void Start()
    {
        if ( false == monobitView.isMine )
        {
            //自分以外のプレイヤーの送信フラグを再度立てる
            monobitView.RPC("SendSync", MonobitEngine.MonobitTargets.Others, null);
        }
    }

    public void OnMonobitSerializeView(MonobitEngine.MonobitStream stream, MonobitEngine.MonobitMessageInfo info)
    {
        if (stream.isWriting)
        {
            /*
             * 同期データの書き込み
             */
            stream.Enqueue(m_AvatarCalibrator.m_IsSynced);
            if (false == m_AvatarCalibrator.m_IsSynced)
            {
                stream.Enqueue(m_AvatarCalibrator.m_FloatParams);
                stream.Enqueue(m_AvatarCalibrator.m_Vector3Params);
                m_AvatarCalibrator.OnEnqueue();
            }
        }
        else
        {
            /*
             * 同期データの読み込み
             */
            m_AvatarCalibrator.m_IsSynced = (bool)stream.Dequeue();
            if (false == m_AvatarCalibrator.m_IsSynced)
            {
                m_AvatarCalibrator.m_FloatParams = (float[])stream.Dequeue();
                m_AvatarCalibrator.m_Vector3Params = (Vector3[])stream.Dequeue();
                m_AvatarCalibrator.OnDequeue();
            }
        }
    }

    [MunRPC]
    private void SendSync()
    {
        if (false == monobitView.isOwner)
        {
            return;
        }

        m_AvatarCalibrator.m_IsSynced = false;
    }
}
