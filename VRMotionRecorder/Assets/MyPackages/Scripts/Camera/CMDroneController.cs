using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using Cinemachine;

public class CMDroneController : MonobitEngine.MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private CinemachineBrain m_CMBrain;
    [SerializeField] private GameObject m_Ear;

    [Header("Params")]
    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private float m_MoveOffsetSpeed = 1f;
    [SerializeField] private float m_FovSpeed = 0.2f;

    private CMCamInfo m_CurrentCamInfo = null;
    private List<CMCamsManager> m_Targets = new List<CMCamsManager>();
    private int m_CurrentTargetIndex = 0;

    private static readonly string BUTTON_MOVE_HORIZONTAL = "Drone_MoveHorizontal";
    private static readonly string BUTTON_MOVE_DEPTH = "Drone_MoveDepth";
    private static readonly string BUTTON_MOVE_VERTICAL_UP = "Drone_MoveVerticalUp";
    private static readonly string BUTTON_MOVE_VERTICAL_DOWN = "Drone_MoveVerticalDown";
    private static readonly string BUTTON_OFFSET_X = "Drone_RotateHorizontal";
    private static readonly string BUTTON_OFFSET_Y = "Drone_RotateVertical";
    private static readonly string BUTTON_PLUS_FOV = "Drone_PlusFOV";
    private static readonly string BUTTON_MINUS_FOV = "Drone_MinusFOV";

    private static readonly string BUTTON_NEXT_TARGET = "Drone_NextTarget";
    private static readonly string BUTTON_NEXT_CAMERA = "Drone_NextCamera";
    private static readonly string BUTTON_RELEASE_TARGET = "Drone_ReleaseTarget";
    private static readonly string BUTTON_RESET_POS = "ResetPos";

    void Start()
    {
        if (false == monobitView.isMine)
        {
            if (null != m_Camera)
            {
                m_Camera.enabled = false;
            }

            if (null != m_CMBrain)
            {
                m_CMBrain.enabled = false;
            }

            if (null != m_Ear)
            {
                m_Ear.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (false == monobitView.isMine)
        {
            return;
        }

        if ((null == m_CurrentCamInfo) ||
            (null == m_CurrentCamInfo.m_Cam))
        {
            SearchCamera();
        }

        NextTarget();
        NextCamera();

        if ((null == m_CurrentCamInfo) ||
            (null == m_CurrentCamInfo.m_Cam))
        {
            return;
        }

        MovePlane();
        MoveVertical();
        MoveTargetOffset();
        ChangeFOV();
        ResetPos();
    }

    private void MovePlane()
    {
        float horizontal = Input.GetAxis(BUTTON_MOVE_HORIZONTAL);
        float depth = Input.GetAxis(BUTTON_MOVE_DEPTH);

        //m_CurrentCam.transform.Translate(m_CurrentCam.transform.right.normalized * horizontal * m_MoveSpeed * Time.deltaTime, Space.World);
        //m_CurrentCam.transform.Translate(m_CurrentCam.transform.forward.normalized * depth * m_MoveSpeed * Time.deltaTime, Space.World);

        m_CurrentCamInfo.m_Cam.transform.Translate(Vector3.right * horizontal * m_MoveSpeed * Time.deltaTime, Space.Self);
        m_CurrentCamInfo.m_Cam.transform.Translate(Vector3.forward * depth * m_MoveSpeed * Time.deltaTime, Space.Self);
    }

    private void MoveVertical()
    {
        var up = Input.GetAxis(BUTTON_MOVE_VERTICAL_UP);
        var down = Input.GetAxis(BUTTON_MOVE_VERTICAL_DOWN);

        m_CurrentCamInfo.m_Cam.transform.Translate(Vector3.up * up * m_MoveSpeed * Time.deltaTime, Space.World);
        m_CurrentCamInfo.m_Cam.transform.Translate(Vector3.down * down * m_MoveSpeed * Time.deltaTime, Space.World);
    }

    private void MoveTargetOffset()
    {
        float x = Input.GetAxis(BUTTON_OFFSET_X);
        float y = Input.GetAxis(BUTTON_OFFSET_Y);

        m_CurrentCamInfo.m_Composer.m_ScreenX = Mathf.Clamp01(m_CurrentCamInfo.m_Composer.m_ScreenX - x * m_MoveOffsetSpeed * Time.deltaTime);
        m_CurrentCamInfo.m_Composer.m_ScreenY = Mathf.Clamp01(m_CurrentCamInfo.m_Composer.m_ScreenY - y * m_MoveOffsetSpeed * Time.deltaTime);
    }

    private void ChangeFOV()
    {
        if (null == m_Camera)
        {
            return;
        }

        if (Input.GetButton(BUTTON_MINUS_FOV))
        {
            m_CurrentCamInfo.m_Cam.m_Lens.FieldOfView -= m_FovSpeed;
        }
        else if (Input.GetButton(BUTTON_PLUS_FOV))
        {
            m_CurrentCamInfo.m_Cam.m_Lens.FieldOfView += m_FovSpeed;
        }
    }

    private void ResetPos()
    {
        if (false == Input.GetButtonDown(BUTTON_RESET_POS))
        {
            return;
        }

        if (m_CurrentTargetIndex >= m_Targets.Count ||
            (null == m_Targets[m_CurrentTargetIndex]))
        {
            return;
        }

        m_Targets[m_CurrentTargetIndex].ResetLocation();
    }

    private void SearchCamera()
    {
        ReleaseTargets();
        SearchTargets();

        if ( ( null == m_Targets ) ||
            0 >= m_Targets.Count )
        {
            return;
        }

        m_CurrentCamInfo = m_Targets[0].Activate();

        m_CurrentTargetIndex = 0;
    }

    private void SearchTargets()
    {
        var targets = FindObjectsOfType<CMCamsManager>();

        for (int i = 0; i < targets.Length; ++i)
        {
            m_Targets.Add(targets[i]);
        }
    }

    private void ReleaseTargets()
    {
        if ((null != m_Targets) &&
            (0 < m_Targets.Count))
        {
            m_Targets[m_CurrentTargetIndex].Deactivate();
            m_Targets.Clear();
        }
    }

    private void NextTarget()
    {
        if (false == Input.GetButtonDown(BUTTON_NEXT_TARGET))
        {
            return;
        }

        ReleaseTargets();
        SearchTargets();

        if ((m_CurrentTargetIndex + 1) < m_Targets.Count)
        {

            ++m_CurrentTargetIndex;
        }
        else
        {
            m_CurrentTargetIndex = 0;
        }

        if (m_CurrentTargetIndex >= m_Targets.Count ||
            (null == m_Targets[m_CurrentTargetIndex]))
        {
            return;
        }

        m_CurrentCamInfo = m_Targets[m_CurrentTargetIndex].Activate();
    }

    private void NextCamera()
    {
        if (false == Input.GetButtonDown(BUTTON_NEXT_CAMERA))
        {
            return;
        }

        if (m_CurrentTargetIndex >= m_Targets.Count ||
            (null == m_Targets[m_CurrentTargetIndex]))
        {
            return;
        }

        m_CurrentCamInfo = m_Targets[m_CurrentTargetIndex].NextCamera();

    }
}


        
