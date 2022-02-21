using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamVR_InputHandle : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources m_Controller;
    [SerializeField] private SteamVR_Action_Boolean m_GrabPinchAction;
    [SerializeField] private SteamVR_Action_Boolean m_GrabGripAction;

    public bool IsGrabPinchDown()
    {
        return m_GrabPinchAction.GetStateDown(m_Controller);
    }

    public bool IsGrabPinchUp()
    {
        return m_GrabPinchAction.GetStateUp(m_Controller);
    }

    public bool IsGrabPinch()
    {
        return m_GrabPinchAction.GetState(m_Controller);
    }

    public bool IsGrabGripDown()
    {
        return m_GrabGripAction.GetStateDown(m_Controller);
    }

    public bool IsGrabGripUp()
    {
        return m_GrabGripAction.GetStateUp(m_Controller);
    }

    public bool IsGrabGrip()
    {
        return m_GrabGripAction.GetState(m_Controller);
    }
}
