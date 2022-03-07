using Entum;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator m_ModelAniamtor;

    [SerializeField]
    private Animator m_FaceAnimator;

    [SerializeField]
    private Animator m_LeftEyeAnimator;

    [SerializeField]
    private Animator m_RightEyeAnimator;
    
    [SerializeField]
    private RuntimeAnimatorController m_ModelRuntimeAnimator;

    [SerializeField]
    private RuntimeAnimatorController m_FaceRuntimeAnimator;

    [SerializeField]
    private RuntimeAnimatorController m_LeftEyeRuntimeAnimator;

    [SerializeField]
    private RuntimeAnimatorController m_RightEyeRuntimeAnimator;

    [SerializeField]
    private MotionConverter m_MotionConverter;

    [SerializeField]
    private EyeMotionConverter[] m_EyeMotionConverter;

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
            m_ModelAniamtor.transform.position = Vector3.zero;
            m_ModelAniamtor.transform.rotation = Quaternion.identity;
            
            AnimatorClipChange(m_ModelRuntimeAnimator, m_ModelAniamtor,(AnimationClip)m_MotionConverter.GetMotion());
            AnimatorClipChange(m_FaceRuntimeAnimator, m_FaceAnimator,(AnimationClip)m_FaceAnimationRecorder.GetFaceMotion());

            m_ModelAniamtor.Play(MOTION, 0, 0);
            m_FaceAnimator.Play(MOTION, 0, 0);

            if ((m_LeftEyeAnimator!=null) || (m_RightEyeAnimator != null))
            {
                AnimatorClipChange(m_LeftEyeRuntimeAnimator, m_LeftEyeAnimator, (AnimationClip)m_EyeMotionConverter[0].GetMotion());
                AnimatorClipChange(m_RightEyeRuntimeAnimator, m_RightEyeAnimator, (AnimationClip)m_EyeMotionConverter[1].GetMotion());

                m_LeftEyeAnimator.Play(MOTION, 0, 0);
                m_RightEyeAnimator.Play(MOTION, 0, 0);
            }
        }
        
        if (Input.GetKeyDown(m_RecordStopKey))
        {
            m_VRIK.enabled = true;
            m_ModelAniamtor.runtimeAnimatorController = null;
            m_FaceAnimator.runtimeAnimatorController = null;
            m_LeftEyeAnimator.runtimeAnimatorController = null;
            m_RightEyeAnimator.runtimeAnimatorController = null;
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
