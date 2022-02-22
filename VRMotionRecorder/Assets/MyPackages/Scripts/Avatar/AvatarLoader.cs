using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class AvatarLoader : MonoBehaviour
{
    [SerializeField] private AvatarMap m_Map = null;
    [SerializeField] private AvatarMap[] m_AvatarMap = null;

    [SerializeField] private Transform m_HeadIKTarget = null;
    [SerializeField] private Transform m_HandLIKTarget = null;
    [SerializeField] private Transform m_HandRIKTarget = null;
    [SerializeField] private Transform m_FootLIKTarget = null;
    [SerializeField] private Transform m_FootRIKTarget = null;
    [SerializeField] private Transform m_WaistIKTarget = null;
    [SerializeField] private Transform m_ElbowLIKTarget = null;
    [SerializeField] private Transform m_ElbowRIKTarget = null;

    [SerializeField] private Hand m_HandL = null;
    [SerializeField] private Hand m_HandR = null;

    public void Load(int id, int num,bool is_owner, ref VRIK ik, ref VRIKRootController ik_root_controller)
    {
        if (null == m_Map)
        {
            return;
        }
        GameObject avatar = Instantiate(m_AvatarMap[num].m_Avatars[id].prefab, transform);

        var settings = avatar.GetComponentInChildren<AvatarSettings>();

        ik = avatar.GetComponentInChildren<VRIK>();
        ik_root_controller = ik.transform.GetComponent<VRIKRootController>();

        if (null != settings)
        {
            SetIKTargets(settings, ik);
            InitHands(settings, ik);

            if(null != settings.OnOwnerChanged)
                settings.OnOwnerChanged(is_owner);
        }

        //if (false == is_owner)
        //{
        //    SetChildLayers(); //シェーダーの都合か、OtherPlayerレイヤーに設定したモデルが黒くなるため一旦保留
        //}
    }

    private void SetIKTargets(AvatarSettings settings, VRIK ik)
    {
        if ( null == ik )
        {
            return;
        }
        var offsets = settings.GetIKTargetOffsets();

        //head
        m_HeadIKTarget.localPosition = offsets.head.pos;
        m_HeadIKTarget.localRotation = Quaternion.Euler(offsets.head.rot);
        ik.solver.spine.headTarget = m_HeadIKTarget;

        //hand L
        m_HandLIKTarget.localPosition = offsets.handL.pos;
        m_HandLIKTarget.localRotation = Quaternion.Euler(offsets.handL.rot);
        ik.solver.leftArm.target = m_HandLIKTarget;

        //hand R
        m_HandRIKTarget.localPosition = offsets.handR.pos;
        m_HandRIKTarget.localRotation = Quaternion.Euler(offsets.handR.rot);
        ik.solver.rightArm.target = m_HandRIKTarget;

        //foot L
        m_FootLIKTarget.localPosition = offsets.footL.pos;
        m_FootLIKTarget.localRotation = Quaternion.Euler(offsets.footL.rot);
        ik.solver.leftLeg.target = m_FootLIKTarget;

        //foot R
        m_FootRIKTarget.localPosition = offsets.footR.pos;
        m_FootRIKTarget.localRotation = Quaternion.Euler(offsets.footR.rot);
        ik.solver.rightLeg.target = m_FootRIKTarget;

        //waist
        m_WaistIKTarget.localPosition = offsets.waist.pos;
        m_WaistIKTarget.localRotation = Quaternion.Euler(offsets.waist.rot);
        ik.solver.spine.pelvisTarget = m_WaistIKTarget;

        //elbow L
        m_ElbowLIKTarget.localPosition = offsets.elbowL.pos;
        m_ElbowLIKTarget.localRotation = Quaternion.Euler(offsets.elbowL.rot);
        ik.solver.leftArm.bendGoal = m_ElbowLIKTarget;

        //elbow R
        m_ElbowRIKTarget.localPosition = offsets.elbowR.pos;
        m_ElbowRIKTarget.localRotation = Quaternion.Euler(offsets.elbowR.rot);
        ik.solver.rightArm.bendGoal = m_ElbowRIKTarget;
    }

    private void InitHands(AvatarSettings settings, VRIK ik)
    {
        if ( null == ik )
        {
            return;
        }

        if (null != m_HandL)
        {
            var finger_sync = settings.GetFingerSyncL();
            var hand_transform = ik.references.leftHand;
            m_HandL.Init(hand_transform, finger_sync);
        }

        if (null != m_HandR)
        {
            var finger_sync = settings.GetFingerSyncR();
            var hand_transform = ik.references.rightHand;
            m_HandR.Init(hand_transform, finger_sync);
        }
    }

    private void SetChildLayers()
    {
        int layer_index = LayerMask.NameToLayer("OtherPlayer");

        var child = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < child.Length; ++i)
        {
            child[i].gameObject.layer = layer_index;
        }
    }
}

