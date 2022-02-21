using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Solver m_Solver = Solver.LEFT_HAND;
    [SerializeField] private SteamVR_InputHandle m_InputHandle = null;

    private Transform m_HandTransform = null;
    private VRFingerSync m_FingerSync = null;

    private InteractiveItem m_TouchingItem = null;
    private InteractiveItem m_GrabbingItem = null;
    
    [SerializeField] private bool m_IsEasyInput = false;
    [SerializeField] private GrabPose[] m_EasyInputPose;

    private readonly string SWITCH_INPUT_KEY = "SwitchHandInput";

    public void Init(Transform hand_transform, VRFingerSync finger_sync)
    {
        m_HandTransform = hand_transform;
        m_FingerSync = finger_sync;
    }

    public Transform GetHandTransform()
    {
        return m_HandTransform;
    }

    public Solver GetSolver()
    {
        return m_Solver;
    }

    void Update()
    {
        if (Input.GetButtonDown(SWITCH_INPUT_KEY))
        {
            m_IsEasyInput = !m_IsEasyInput;
            if (false == m_IsEasyInput)
            {
                if (null != m_FingerSync)
                {
                    m_FingerSync.SetPose(null);
                }
            }
        }

        if ((null == m_GrabbingItem) &&
            (null != m_TouchingItem) &&
            (true == m_InputHandle.IsGrabPinchDown()))
        {
            GrabItem();
        }

        if ((null != m_GrabbingItem) &&
            (true == m_InputHandle.IsGrabPinchUp()))
        {
            UnGrabItem();
        }

        if ((true == m_IsEasyInput) &&
            (null == m_GrabbingItem) )
        {
            var is_grab_pinch = m_InputHandle.IsGrabPinch();
            var is_grab_grip = m_InputHandle.IsGrabGrip();

            if ((true == is_grab_pinch) && (true == is_grab_grip))
            {
                m_FingerSync.SetPose(m_EasyInputPose[1]);
            }
            else if ((false == is_grab_pinch) && (true == is_grab_grip))
            {
                m_FingerSync.SetPose(m_EasyInputPose[2]);
            }
            else
            {
                m_FingerSync.SetPose(m_EasyInputPose[0]);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var item = other.gameObject.GetComponent<InteractiveItem>();
        if (null == item)
        {
            return;
        }

        m_TouchingItem = item;
    }

    void OnTriggerExit(Collider other)
    {
        var item = other.gameObject.GetComponent<InteractiveItem>();
        if (null == item)
        {
            return;
        }

        if (item == m_TouchingItem)
        {
            m_TouchingItem = null;
        }
    }

    private void GrabItem()
    {
        var pose = m_TouchingItem.Attach(this);
        m_GrabbingItem = m_TouchingItem;

        if ( (null != pose ) &&
             (null != m_FingerSync))
        {
            m_FingerSync.SetPose(pose);
        }
    }

    private void UnGrabItem()
    {
        m_GrabbingItem.Detach();
        m_GrabbingItem = null;

        m_TouchingItem = null;

        if (null != m_FingerSync)
        {
            m_FingerSync.SetPose(null);
        }
    }
}

public enum Solver
{
    LEFT_HAND,
    RIGHT_HAND
}
