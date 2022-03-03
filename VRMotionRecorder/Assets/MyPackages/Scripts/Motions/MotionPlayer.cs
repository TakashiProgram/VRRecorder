using Entum;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator[] m_Animator;

    [SerializeField]
    private RuntimeAnimatorController[] m_RuntimeAnimatorController;

    [SerializeField]
    private MotionConverter m_MotionConverter;

    [SerializeField]
    private FaceAnimationRecorder m_FaceAnimationRecorder;

    [SerializeField]
    private KeyCode m_RecordPlayKey = KeyCode.P;

    [SerializeField]
    private KeyCode m_RecordStopKey = KeyCode.O;

    [SerializeField]
    private VRIK m_VRIK;

    private bool m_IsPlay = false;
    private static string MOTION = "Motion";

    private static string MOVING_SPEED = "MovingSpeed";

    void Update()
    {
        if (Input.GetKeyDown(m_RecordPlayKey))
        {
            if (null != m_VRIK)
            {
                m_VRIK.enabled = false;
            }
            m_Animator[0].transform.position = Vector3.zero;
            m_Animator[0].transform.rotation = Quaternion.identity;

            //m_Animator[1].transform.position = Vector3.zero;
           // m_Animator[1].transform.rotation = Quaternion.identity;

            AnimatorClipChange(m_RuntimeAnimatorController[0], m_Animator[0],(AnimationClip)m_MotionConverter.GetMotion());
            AnimatorClipChange(m_RuntimeAnimatorController[1], m_Animator[1],(AnimationClip)m_FaceAnimationRecorder.GetFaceMotion());

            m_Animator[0].Play(MOTION, 0, 0);
            m_Animator[1].Play(MOTION, 0, 0);
        }
        if (Input.GetKeyDown(m_RecordStopKey))
        {
            if (true == m_IsPlay)
            {
                m_Animator[0].SetFloat(MOVING_SPEED, 1.0f);
                m_Animator[1].SetFloat(MOVING_SPEED, 1.0f);
                m_IsPlay = false;
            }
            else
            {
                m_Animator[0].SetFloat(MOVING_SPEED, 0.0f);
                m_Animator[1].SetFloat(MOVING_SPEED, 0.0f);
                m_IsPlay = true;
            }
            
        }

    }
    public void AnimatorClipChange(RuntimeAnimatorController model_animatorclip,Animator animator,AnimationClip animationclip)
    {
        //AnimationClip motion = (AnimationClip)m_MotionConverter.GetMotion();
        AnimationClip motion = animationclip;
        animator.runtimeAnimatorController = model_animatorclip;

        AnimatorOverrideController overrideAnimetorController = new AnimatorOverrideController();
        overrideAnimetorController.runtimeAnimatorController = animator.runtimeAnimatorController;

        AnimationClipPair[] clipPairs = overrideAnimetorController.clips;

        clipPairs[0].overrideClip = motion;

        overrideAnimetorController.clips = clipPairs;

        animator.runtimeAnimatorController = overrideAnimetorController;
    }
}
