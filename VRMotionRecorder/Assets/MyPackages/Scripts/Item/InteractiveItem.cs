using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class InteractiveItem : MonobitEngine.MonoBehaviour
{
    [SerializeField] private GrabPose m_GrabPose = null;

    private Rigidbody m_Rigidbody = null;
    private Hand m_Hand = null;

    void Start()
    {
        m_Rigidbody = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        FollowHand();
    }

    public GrabPose Attach( Hand hand )
    {
        m_Hand = hand;

        //所有者でしか位置を変更できないため
        monobitView.RequestOwnership();

        //他プレイヤーから見て掴んでいる物に重力が効き続けて見える問題について、
        //暫定的にRPCで対処。出来れば使わずにMonobitViewのRigidbody同期をうまく使いたい。
        monobitView.RPC("RigidbodyUseGravity", MonobitEngine.MonobitTargets.All, false);

        return m_GrabPose;
    }

    public void Detach()
    {
        if ( null == m_Hand )
        {
            return;
        }

        m_Hand = null;

        monobitView.RPC("RigidbodyUseGravity", MonobitEngine.MonobitTargets.All, true);
    }

    private void FollowHand()
    {
        if ((null == m_Hand) ||
            (null == m_GrabPose ))
        {
            return;
        }

        var hand_transform = m_Hand.GetHandTransform();
        if (null == hand_transform)
        {
            return;
        }

        var solver = m_Hand.GetSolver();
        if ( Solver.LEFT_HAND == solver )
        {
            transform.position = hand_transform.position + ( hand_transform.rotation * m_GrabPose.m_OffsetPosL );
            transform.rotation = hand_transform.rotation * Quaternion.Euler(m_GrabPose.m_OffsetRotL);
        }
        else
        {
            transform.position = hand_transform.position + ( hand_transform.rotation * m_GrabPose.m_OffsetPosR );
            transform.rotation = hand_transform.rotation * Quaternion.Euler(m_GrabPose.m_OffsetRotR);
        }
    }

    [MunRPC]
    void RigidbodyUseGravity(bool use_gravity)
    {
        if (null != m_Rigidbody)
        {
            m_Rigidbody.useGravity = use_gravity;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;

            if (true == use_gravity)
            {
                m_Rigidbody.WakeUp();
            }
            else
            {
                m_Rigidbody.Sleep();
            }
        }
    }
}
