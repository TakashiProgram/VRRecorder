using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRFingerSync : SyncObject
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private SteamVR_Action_Skeleton m_SkeletonAction;
    [SerializeField] private Solver m_Solver = Solver.LEFT_HAND;

    [SerializeField] private float m_LerpRate = 10f;
    [SerializeField][Range(0f,1f)] private float m_MaxFingerStretchRate = 1f; //自然状態での指の最大伸び率。この値を基準に各指の伸びと間隔をスケール

    private HumanPoseHandler m_Handler = null;
    private GrabPose m_GrabPose = null;
    
    /* 同期データ */
    private float[] m_FingerInfo = new float[9];


    private bool m_IsEasyInput = false;

    protected override void Start()
    {
        base.Start();

        Init();
    }

    private void Init()
    {
        m_Handler = new HumanPoseHandler(m_Animator.avatar, m_Animator.transform);
    }

    public void SetPose(GrabPose pose)
    {
        m_GrabPose = pose;
    }
    
    public override void UpdateForOwner()
    {
        if(null != m_GrabPose)
        {
            //各指の曲がり具合
            m_FingerInfo[0] = m_GrabPose.m_ThumbCurl; //thumb curl
            m_FingerInfo[1] = m_GrabPose.m_IndexCurl; //index curl
            m_FingerInfo[2] = m_GrabPose.m_MiddleCurl; //middle curl
            m_FingerInfo[3] = m_GrabPose.m_RingCurl; //ring curl
            m_FingerInfo[4] = m_GrabPose.m_PinkyCurl; //pinky curl

            //各指の間隔
            m_FingerInfo[5] = m_GrabPose.m_ThumbIndexSplay; //thumb-index splay
            m_FingerInfo[6] = m_GrabPose.m_IndexMiddleSplay; //index-middle splay
            m_FingerInfo[7] = m_GrabPose.m_MiddleRingSplay; //middle-ring splay
            m_FingerInfo[8] = m_GrabPose.m_RingPinkySplay; //ring-pinky splay
        }
        else
        {
            //各指の曲がり具合
            m_FingerInfo[0] = AdjustForCurl(m_SkeletonAction.GetFingerCurl(SteamVR_Skeleton_FingerIndexEnum.thumb)); //thumb curl
            m_FingerInfo[1] = AdjustForCurl(m_SkeletonAction.GetFingerCurl(SteamVR_Skeleton_FingerIndexEnum.index)); //index curl
            m_FingerInfo[2] = AdjustForCurl(m_SkeletonAction.GetFingerCurl(SteamVR_Skeleton_FingerIndexEnum.middle)); //middle curl
            m_FingerInfo[3] = AdjustForCurl(m_SkeletonAction.GetFingerCurl(SteamVR_Skeleton_FingerIndexEnum.ring)); //ring curl
            m_FingerInfo[4] = AdjustForCurl(m_SkeletonAction.GetFingerCurl(SteamVR_Skeleton_FingerIndexEnum.pinky)); //pinky curl

            //各指の間隔
            m_FingerInfo[5] = AdjustForSplay(m_SkeletonAction.GetSplay(SteamVR_Skeleton_FingerSplayIndexEnum.thumbIndex)); //thumb-index splay
            m_FingerInfo[6] = AdjustForSplay(m_SkeletonAction.GetSplay(SteamVR_Skeleton_FingerSplayIndexEnum.indexMiddle)); //index-middle splay
            m_FingerInfo[7] = AdjustForSplay(m_SkeletonAction.GetSplay(SteamVR_Skeleton_FingerSplayIndexEnum.middleRing)) / 2f; //middle-ring splay
            m_FingerInfo[8] = AdjustForSplay(m_SkeletonAction.GetSplay(SteamVR_Skeleton_FingerSplayIndexEnum.ringPinky)) / 2f; //ring-pinky splay
        }

        SetMuscles();
    }

    public override void UpdateForClient()
    {
        SetMuscles(m_LerpRate * Time.deltaTime);
    }

    public override void OnEnqueue(MonobitEngine.MonobitStream stream)
    {
        stream.Enqueue(m_FingerInfo);
    }

    public override void OnDequeue(MonobitEngine.MonobitStream stream)
    {
        m_FingerInfo = (float[])stream.Dequeue();
    }

    private void SetMuscles(float lerp = 1f)
    {
        if (null == m_Handler)
        {
            return;
        }

        var humanpose = new HumanPose();
        m_Handler.GetHumanPose(ref humanpose);
        
        if (Solver.LEFT_HAND == m_Solver)
        {
            humanpose.muscles[55] = Mathf.Lerp(humanpose.muscles[55], m_FingerInfo[0], lerp);
            humanpose.muscles[56] = Mathf.Lerp(humanpose.muscles[56], m_FingerInfo[5], lerp);
            humanpose.muscles[57] = Mathf.Lerp(humanpose.muscles[57], m_FingerInfo[0], lerp);
            humanpose.muscles[58] = Mathf.Lerp(humanpose.muscles[58], m_FingerInfo[0], lerp);

            humanpose.muscles[59] = Mathf.Lerp(humanpose.muscles[59], m_FingerInfo[1], lerp);
            humanpose.muscles[60] = Mathf.Lerp(humanpose.muscles[60], m_FingerInfo[6], lerp);
            humanpose.muscles[61] = Mathf.Lerp(humanpose.muscles[61], m_FingerInfo[1], lerp);
            humanpose.muscles[62] = Mathf.Lerp(humanpose.muscles[62], m_FingerInfo[1], lerp);

            humanpose.muscles[63] = Mathf.Lerp(humanpose.muscles[63], m_FingerInfo[2], lerp);
            humanpose.muscles[64] = Mathf.Lerp(humanpose.muscles[64], m_FingerInfo[7], lerp);
            humanpose.muscles[65] = Mathf.Lerp(humanpose.muscles[65], m_FingerInfo[2], lerp);
            humanpose.muscles[66] = Mathf.Lerp(humanpose.muscles[66], m_FingerInfo[2], lerp);

            humanpose.muscles[67] = Mathf.Lerp(humanpose.muscles[67], m_FingerInfo[3], lerp);
            humanpose.muscles[68] = Mathf.Lerp(humanpose.muscles[68], m_FingerInfo[8], lerp);
            humanpose.muscles[69] = Mathf.Lerp(humanpose.muscles[69], m_FingerInfo[3], lerp);
            humanpose.muscles[70] = Mathf.Lerp(humanpose.muscles[70], m_FingerInfo[3], lerp);

            humanpose.muscles[71] = Mathf.Lerp(humanpose.muscles[71], m_FingerInfo[4], lerp);
            humanpose.muscles[72] = Mathf.Lerp(humanpose.muscles[72], m_FingerInfo[8], lerp);
            humanpose.muscles[73] = Mathf.Lerp(humanpose.muscles[73], m_FingerInfo[4], lerp);
            humanpose.muscles[74] = Mathf.Lerp(humanpose.muscles[74], m_FingerInfo[4], lerp);
        }
        else if (Solver.RIGHT_HAND == m_Solver)
        {
            humanpose.muscles[75] = Mathf.Lerp(humanpose.muscles[75], m_FingerInfo[0], lerp);
            humanpose.muscles[76] = Mathf.Lerp(humanpose.muscles[76], m_FingerInfo[5], lerp);
            humanpose.muscles[77] = Mathf.Lerp(humanpose.muscles[77], m_FingerInfo[0], lerp);
            humanpose.muscles[78] = Mathf.Lerp(humanpose.muscles[78], m_FingerInfo[0], lerp);

            humanpose.muscles[79] = Mathf.Lerp(humanpose.muscles[79], m_FingerInfo[1], lerp);
            humanpose.muscles[80] = Mathf.Lerp(humanpose.muscles[80], m_FingerInfo[6], lerp);
            humanpose.muscles[81] = Mathf.Lerp(humanpose.muscles[81], m_FingerInfo[1], lerp);
            humanpose.muscles[82] = Mathf.Lerp(humanpose.muscles[82], m_FingerInfo[1], lerp);

            humanpose.muscles[83] = Mathf.Lerp(humanpose.muscles[83], m_FingerInfo[2], lerp);
            humanpose.muscles[84] = Mathf.Lerp(humanpose.muscles[84], m_FingerInfo[7], lerp);
            humanpose.muscles[85] = Mathf.Lerp(humanpose.muscles[85], m_FingerInfo[2], lerp);
            humanpose.muscles[86] = Mathf.Lerp(humanpose.muscles[86], m_FingerInfo[2], lerp);

            humanpose.muscles[87] = Mathf.Lerp(humanpose.muscles[87], m_FingerInfo[3], lerp);
            humanpose.muscles[88] = Mathf.Lerp(humanpose.muscles[88], m_FingerInfo[8], lerp);
            humanpose.muscles[89] = Mathf.Lerp(humanpose.muscles[89], m_FingerInfo[3], lerp);
            humanpose.muscles[90] = Mathf.Lerp(humanpose.muscles[90], m_FingerInfo[3], lerp);

            humanpose.muscles[91] = Mathf.Lerp(humanpose.muscles[91], m_FingerInfo[4], lerp);
            humanpose.muscles[92] = Mathf.Lerp(humanpose.muscles[92], m_FingerInfo[8], lerp);
            humanpose.muscles[93] = Mathf.Lerp(humanpose.muscles[93], m_FingerInfo[4], lerp);
            humanpose.muscles[94] = Mathf.Lerp(humanpose.muscles[94], m_FingerInfo[4], lerp);
        }
        else
        {
            //NOP
        }

        m_Handler.SetHumanPose(ref humanpose);
    }

    private float AdjustForCurl(float rate)
    {
        rate = 1f - rate; //1~0→0~1に変換
        float max_finger_stretch = Mathf.Lerp(-1f, 1f, m_MaxFingerStretchRate); //指の最大伸び率を実値に変換
        float curl = Mathf.Lerp(-1f, max_finger_stretch, rate); //0~1→-1~1に変換
        return curl;
    }

    private float AdjustForSplay(float rate)
    {
        float max_finger_stretch = Mathf.Lerp(-1f, 1f, m_MaxFingerStretchRate); //指の最大伸び率を実値に変換
        float splay = Mathf.Lerp(-1f, max_finger_stretch, rate); //0~1→-1~1に変換
        return splay;
    }
}
