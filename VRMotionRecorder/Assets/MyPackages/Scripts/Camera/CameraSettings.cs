using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;

    public Camera GetMainCamera()
    {
        return m_MainCamera;
    }
}
