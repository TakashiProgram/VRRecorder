using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private RuntimeAnimatorController m_RuntimeAnimatorController;

    [SerializeField]
    private MotionConverter m_MotionConverter;

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

            AnimatorClipChange();

            m_Animator.Play(MOTION, 0, 0);
        }
        if (Input.GetKeyDown(m_RecordStopKey))
        {
            if (true == m_IsPlay)
            {
                m_Animator.SetFloat(MOVING_SPEED, 1.0f);
                m_IsPlay = false;
            }
            else
            {
                m_Animator.SetFloat(MOVING_SPEED, 0.0f);
                m_IsPlay = true;
            }
            
        }

    }
    public void AnimatorClipChange()
    {
        AnimationClip motion = (AnimationClip)m_MotionConverter.GetMotion();
        m_Animator.runtimeAnimatorController = m_RuntimeAnimatorController;

        AnimatorOverrideController overrideAnimetorController = new AnimatorOverrideController();
        overrideAnimetorController.runtimeAnimatorController = m_Animator.runtimeAnimatorController;

        AnimationClipPair[] clipPairs = overrideAnimetorController.clips;

        clipPairs[0].overrideClip = motion;

        overrideAnimetorController.clips = clipPairs;

        m_Animator.runtimeAnimatorController = overrideAnimetorController;
    }
}
