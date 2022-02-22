using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.UI;

public class DroneController : MonobitEngine.MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private GameObject m_Ear;

    [Header("Params")]
    [SerializeField] private float m_MovePlaneSpeed = 1f;
    [SerializeField] private float m_MoveVerticalSpeed = 1f;
    [SerializeField] private float m_RotateSpeed = 1f;
    [SerializeField] private float m_FovSpeed = 0.2f;
    
    //private int m_CurrentPlayerIndex = -1;
    //private Transform m_Target = null;

    //private Vector3 m_LastTargetDif = CV.zero;


    private static readonly string BUTTON_MOVE_HORIZONTAL       = "Drone_MoveHorizontal";
    private static readonly string BUTTON_MOVE_DEPTH            = "Drone_MoveDepth";
    private static readonly string BUTTON_MOVE_VERTICAL_UP      = "Drone_MoveVerticalUp";
    private static readonly string BUTTON_MOVE_VERTICAL_DOWN    = "Drone_MoveVerticalDown";
    private static readonly string BUTTON_ROTATE_HORIZONTAL     = "Drone_RotateHorizontal";
    private static readonly string BUTTON_ROTATE_VERTICAL       = "Drone_RotateVertical";
    private static readonly string BUTTON_PLUS_FOV              = "Drone_PlusFOV";
    private static readonly string BUTTON_MINUS_FOV             = "Drone_MinusFOV";
    

    //private static readonly string BUTTON_NEXT_TARGET       = "Drone_NextTarget";
    //private static readonly string BUTTON_RELEASE_TARGET    = "Drone_ReleaseTarget";
    private static readonly string BUTTON_RESET_POS         = "ResetPos";

    void Start()
    {
        if (false == monobitView.isMine)
        {
            if (null != m_Camera)
            {
                m_Camera.enabled = false;
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

        //FollowTarget();

        MovePlane();
        MoveVertical();
        Rotate();
        ChangeFOV();

        //NextTarget();
        //ReleaseTarget();
        ResetPos();

        
        //if (null != m_Target)
        //{
        //    m_LastTargetDif = transform.position - m_Target.position;
        //}
    }



    private void MovePlane()
    {
        float horizontal = Input.GetAxis(BUTTON_MOVE_HORIZONTAL);
        float depth = Input.GetAxis(BUTTON_MOVE_DEPTH);

        //if (null == m_Target)
        //{
            transform.Translate(Vector3.right * horizontal * m_MovePlaneSpeed * Time.deltaTime, Space.Self);
            transform.Translate(Vector3.forward * depth * m_MovePlaneSpeed * Time.deltaTime, Space.Self);
        //}
        //else
        //{
        //    Vector3 target_dir = (m_Target.position - transform.position).normalized;
        //    transform.Translate(Vector3.right * horizontal * m_MovePlaneSpeed * Time.deltaTime, Space.Self);
        //    transform.Translate(target_dir * depth * m_MovePlaneSpeed * Time.deltaTime, Space.World);
        //}
    }


    private void MoveVertical()
    {
        var up = Input.GetAxis(BUTTON_MOVE_VERTICAL_UP);
        var down = Input.GetAxis(BUTTON_MOVE_VERTICAL_DOWN);

        transform.Translate(Vector3.up * (up - down) * m_MoveVerticalSpeed * Time.deltaTime, Space.Self);
    }

    private void Rotate()
    {
        float horizontal = Input.GetAxis(BUTTON_ROTATE_HORIZONTAL);
        float vertical = Input.GetAxis(BUTTON_ROTATE_VERTICAL);

        //if (null == m_Target)
        //{
            transform.Rotate(Vector3.up ,horizontal * m_RotateSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, vertical * m_RotateSpeed * Time.deltaTime, Space.Self);
        //}
        //else
        //{
        //    transform.RotateAround(m_Target.transform.position, Vector3.up, horizontal * m_RotateSpeed * Time.deltaTime);
        //    transform.RotateAround(m_Target.transform.position, transform.right, vertical * m_RotateSpeed * Time.deltaTime);
        //}
    }

    private void ChangeFOV()
    {
        if (null == m_Camera)
        {
            return;
        }

        if (Input.GetButton(BUTTON_MINUS_FOV))
        {
            m_Camera.fieldOfView -= m_FovSpeed;
        }
        else if (Input.GetButton(BUTTON_PLUS_FOV))
        {
            m_Camera.fieldOfView += m_FovSpeed;
        }
    }

    //private void NextTarget()
    //{
    //    if (false == Input.GetButtonDown(BUTTON_NEXT_TARGET))
    //    {
    //        return;
    //    }

    //    var players = FindObjectsOfType<AbstractPlayerController>();

    //    if (0 >= players.Length)
    //    {
    //        //プレイヤーが不在
    //        m_CurrentPlayerIndex = -1;
    //        m_Target = null;
    //        return;
    //    }
    //    else if ( (m_CurrentPlayerIndex + 1) < players.Length )
    //    {
    //        ++m_CurrentPlayerIndex;
    //        m_Target = players[m_CurrentPlayerIndex].transform;

    //        FocusTarget();
    //    }
    //    else
    //    {
    //        //リストの先頭に戻る
    //        m_CurrentPlayerIndex = 0;
    //        m_Target = players[0].transform;

    //        FocusTarget();
    //    }
    //}

    //private void ReleaseTarget()
    //{
    //    if (false == Input.GetButtonDown(BUTTON_RELEASE_TARGET))
    //    {
    //        return;
    //    }

    //    m_CurrentPlayerIndex = -1;
    //    m_Target = null;
    //}

    private void ResetPos()
    {
        if (false == Input.GetButtonDown(BUTTON_RESET_POS))
        {
            return;
        }

        //m_CurrentPlayerIndex = -1;
        //m_Target = null;

        Respawn();
    }

    private void Respawn()
    {
        var spawn_pos = CV.zero;

        var points = FindObjectsOfType<SpawnPos>();
        if (0 < points.Length)
        {
            int random = UnityEngine.Random.Range(0, points.Length);
            spawn_pos = points[random].transform.position;
        }

        transform.position = spawn_pos;
        transform.rotation = Quaternion.identity;


        if (null != m_Camera)
        {
            m_Camera.fieldOfView = 60f;
        }
    }

    //private void FocusTarget()
    //{
    //    if (null == m_Target)
    //    {
    //        return;
    //    }

    //    transform.position = m_Target.position + new Vector3(0f, 0.75f, -5f);
    //    transform.rotation = Quaternion.identity;

    //    m_LastTargetDif = transform.position - m_Target.position;
    //}

    //private void FollowTarget()
    //{
    //    if (null == m_Target)
    //    {
    //        return;
    //    }

    //    var destination = m_Target.position + m_LastTargetDif;
    //    transform.position = Vector3.Lerp(transform.position, destination, 0.9f);
    //}
   
}
