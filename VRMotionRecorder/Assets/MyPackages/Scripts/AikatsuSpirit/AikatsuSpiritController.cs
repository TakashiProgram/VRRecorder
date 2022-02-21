using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class AikatsuSpiritController : AbstractPlayerController
{
    [Header("Components")]
    [SerializeField] private MunAikatsuSpiritAvatarController m_AvatarController;
    [SerializeField] private GameObject m_Camera;
    [SerializeField] private GameObject m_UI;

    [Header("Params")]
    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private float m_RotateSpeed = 1f;
    [SerializeField] private float m_ScaleSpeed = 0.1f;

    private static readonly string BUTTON_MOVE_HORIZONTAL = "AikatsuSpirit_MoveHorizontal";
    private static readonly string BUTTON_MOVE_DEPTH = "AikatsuSpirit_MoveDepth";
    private static readonly string BUTTON_MOVE_VERTICAL_UP = "AikatsuSpirit_MoveVerticalUp";
    private static readonly string BUTTON_MOVE_VERTICAL_DOWN = "AikatsuSpirit_MoveVerticalDown";
    private static readonly string BUTTON_ROTATE_HORIZONTAL = "AikatsuSpirit_RotateHorizontal";
    private static readonly string BUTTON_SCALE_UP = "AikatsuSpirit_ScaleUp";
    private static readonly string BUTTON_SCALE_DOWN = "AikatsuSpirit_ScaleDown";

    private static readonly string BUTTON_NEXT_AVATAR = "AikatsuSpirit_NextAvatar";
    private static readonly string BUTTON_PREV_AVATAR = "AikatsuSpirit_PrevAvatar";

    //private static readonly string BUTTON_PLAY_TIMELINE = "AikatsuSpirit_PlayTimeline";
    //private static readonly string BUTTON_STOP_TIMELINE = "AikatsuSpirit_StopTimeline";
    //private static readonly string BUTTON_NEXT_TIMELINE = "AikatsuSpirit_NextTimeline";
    private static readonly string BUTTON_RESET_POS = "ResetPos";

    void Awake()
    {
        if (false == monobitView.isMine)
        {
            if (null != m_Camera)
            {
                m_Camera.SetActive(false);
            }

            if (null != m_UI)
            {
                m_UI.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (false == monobitView.isMine)
        {
            return;
        }

        MovePlane();
        MoveVertical();
        Rotate();
        Scale();

        NextAvatar();
        PrevAvatar();
        ResetPos();
    }

    private void MovePlane()
    {
        var horizontal = Input.GetAxis(BUTTON_MOVE_HORIZONTAL);
        var depth = Input.GetAxis(BUTTON_MOVE_DEPTH);

        transform.Translate(Vector3.right * horizontal * m_MoveSpeed * Time.deltaTime, Space.Self);
        transform.Translate(Vector3.forward * depth * m_MoveSpeed * Time.deltaTime, Space.Self);
    }

    private void MoveVertical()
    {
        var up = Input.GetAxis(BUTTON_MOVE_VERTICAL_UP);
        var down = Input.GetAxis(BUTTON_MOVE_VERTICAL_DOWN);

        transform.Translate(Vector3.up * (up - down) * m_MoveSpeed * Time.deltaTime, Space.Self);
    }

    private void Rotate()
    {
        var horizontal = Input.GetAxis(BUTTON_ROTATE_HORIZONTAL);
        //var vertivcal = Input.GetAxis(BUTTON_ROTATE_VERTICAL);

        transform.Rotate(Vector3.up, horizontal * m_RotateSpeed * Time.deltaTime);
        //transform.RotateAround(m_Target.transform.position, transform.right, vertivcal * m_RotateSpeed * Time.deltaTime);
    }

    private void Scale()
    {
        var up = Input.GetAxis(BUTTON_SCALE_UP);
        var down = Input.GetAxis(BUTTON_SCALE_DOWN);

        transform.localScale += Vector3.one * ( up - down ) * m_ScaleSpeed * Time.deltaTime;
    }

    private void ResetPos()
    {
        if (false == Input.GetButtonDown(BUTTON_RESET_POS))
        {
            return;
        }

        var spawn_pos = CV.zero;

        var points = FindObjectsOfType<SpawnPos>();
        if (0 < points.Length)
        {
            int random = UnityEngine.Random.Range(0, points.Length);
            spawn_pos = points[random].transform.position;
        }

        transform.position = spawn_pos;
        transform.rotation = Quaternion.identity;
    }

    private void NextAvatar()
    {
        if (false == Input.GetButtonDown(BUTTON_NEXT_AVATAR))
        {
            return;
        }

        if (null == m_AvatarController)
        {
            return;
        }

        m_AvatarController.NextAvatar();
    }

    private void PrevAvatar()
    {
        if (false == Input.GetButtonDown(BUTTON_PREV_AVATAR))
        {
            return;
        }

        if (null == m_AvatarController)
        {
            return;
        }

        m_AvatarController.PrevAvatar();
    }
}
