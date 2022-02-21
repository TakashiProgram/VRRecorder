using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Calibrator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_HeadTracker = null;
    [SerializeField] private Transform m_HandLTracker = null;
    [SerializeField] private Transform m_HandRTracker = null;
    [SerializeField] private Transform m_ElbowLTracker = null;
    [SerializeField] private Transform m_ElbowRTracker = null;
    //[SerializeField] private Transform m_ElbowLOffset = null;
    //[SerializeField] private Transform m_ElbowROffset = null;
    [SerializeField] private Transform m_WaistTracker = null;
    [Header("Roots")]
    [SerializeField] private Transform m_HeadTrackerRoot = null;
    [SerializeField] private Transform m_HandTrackerRoot = null;
    [SerializeField] private Transform m_FootTrackerRoot = null;
    [Header("Params")]
    [SerializeField] private float m_CalibrateDelay = 3f;

    private Transform m_WristL = null;
    private Transform m_WristR = null;

    private VRIK m_VRIK = null;
    private VRIKRootController m_VRIKRootController = null;
    private static readonly string SETTINGS_PATH = "CalibrationSettings.json";


    public void Init(VRIK ik, VRIKRootController ik_root_controller)
    {
        m_VRIK = ik;
        m_VRIKRootController = ik_root_controller;

        if (null != m_VRIKRootController)
        {
            m_VRIKRootController.Calibrate();
        }
    }

    public void DelayCalibrate()
    {
        StartCoroutine(DelayCalibrate(m_CalibrateDelay));
    }

    private IEnumerator DelayCalibrate( float delay )
    {
        yield return new WaitForSeconds(delay);

        //Calibrate();
    }
    
    //private void Calibrate()
    //{
    //    if ( null == m_VRIK )
    //    {
    //        return;
    //    }

    //    var root = transform.root;
    //    var root_height = root.position.y;

    //    m_HeadTrackerRoot.localScale = Vector3.one;
    //    m_HandTrackerRoot.localScale = Vector3.one;
    //    m_FootTrackerRoot.localScale = Vector3.one;
    //    //m_HeadTrackerRoot.position = Vector3.zero;
    //    //m_HandTrackerRoot.position = Vector3.zero;
    //    //m_FootTrackerRoot.position = Vector3.zero;

    //    //コントローラーの場合手首までのオフセットを追加
    //    if (null == m_WristL)
    //    {
    //        var offset = new GameObject("WristLOffset");
    //        offset.transform.parent = m_HandLTracker;
    //        offset.transform.localPosition = new Vector3(-0.04f, 0.04f, -0.15f);
    //        offset.transform.localRotation = Quaternion.Euler(60, 0, 90);
    //        offset.transform.localScale = Vector3.one;
    //        m_WristL = offset.transform;
    //    }
    //    if (null == m_WristR)
    //    {
    //        var offset = new GameObject("WristROffset");
    //        offset.transform.parent = m_HandRTracker;
    //        offset.transform.localPosition = new Vector3(0.04f, 0.04f, -0.15f);
    //        offset.transform.localRotation = Quaternion.Euler(60, 0, -90);
    //        offset.transform.localScale = Vector3.one;
    //        m_WristR = offset.transform;
    //    }

    //    // モデルの体の中心を取っておく
    //    var model_center_position = Vector3.Lerp(m_VRIK.references.leftHand.position, m_VRIK.references.rightHand.position, 0.5f);
    //    model_center_position = new Vector3(model_center_position.x, m_VRIK.references.root.position.y, model_center_position.z);
    //    var model_center_distance = Vector3.Distance(m_VRIK.references.root.position, model_center_position);

    //    // モデルのポジションを手と手の中心位置に移動
    //    var center_position = Vector3.Lerp(m_WristL.position, m_WristR.position, 0.5f);
    //    m_VRIK.references.root.position = new Vector3(center_position.x, m_VRIK.references.root.position.y, center_position.z);

    //    Vector3 hmd_forward_angle = m_HeadTracker.forward;
    //    hmd_forward_angle.y = 0f;
    //    m_VRIK.references.root.rotation = Quaternion.LookRotation(hmd_forward_angle);

    //    // 手のトラッカー全体のスケールを手の位置に合わせる
    //    var model_hand_distance = Vector3.Distance(m_VRIK.references.leftHand.position, m_VRIK.references.rightHand.position);
    //    var real_hand_distance = Vector3.Distance(m_WristL.position, m_WristR.position);
    //    var w_scale = model_hand_distance / real_hand_distance;
    //    var model_hand_height = (m_VRIK.references.leftHand.position.y + m_VRIK.references.rightHand.position.y) / 2f - root_height;
    //    var real_hand_height = (m_WristL.position.y + m_WristR.position.y) / 2f - root_height;
    //    var h_scale = model_hand_height / real_hand_height;
    //    m_HandTrackerRoot.localScale = new Vector3(w_scale, h_scale, w_scale);

    //    // モデルのポジションを再度手と手の中心位置に移動
    //    center_position = Vector3.Lerp(m_WristL.position, m_WristR.position, 0.5f);
    //    m_VRIK.references.root.position = new Vector3(center_position.x, m_VRIK.references.root.position.y, center_position.z)
    //        + m_VRIK.references.root.forward * model_center_distance + m_VRIK.references.root.forward * 0.1f;
    //    m_VRIK.references.root.rotation = Quaternion.LookRotation(hmd_forward_angle);

    //    // 頭のトラッカー全体のスケールを頭の位置に合わせる
    //    var model_head_height = m_VRIK.references.head.position.y - root_height;
    //    var real_head_height = m_HeadTracker.position.y - root_height;
    //    var head_h_scale = model_head_height / real_head_height;
    //    m_HeadTrackerRoot.localScale = new Vector3(w_scale, head_h_scale, w_scale);

    //    // 腰のトラッカー全体のスケールを腰の位置に合わせる
    //    if (m_WaistTracker != null)
    //    {
    //        var model_pelvis_height = m_VRIK.references.pelvis.position.y - root_height;
    //        var real_pelvis_height = m_WaistTracker.position.y - root_height;
    //        var pelvis_h_scale = model_pelvis_height / real_pelvis_height;
    //        m_FootTrackerRoot.localScale = new Vector3(w_scale, pelvis_h_scale, w_scale);
    //    }

    //    if (null != m_VRIKRootController)
    //    {
    //        m_VRIKRootController.Calibrate();
    //    }

    //    Debug.Log("Calibrator: Calibration completed !");
    //}

    //private void Calibrate()
    //{
    //    if (null == m_VRIK)
    //    {
    //        return;
    //    }

    //    //Left Elbow
    //    if (m_ElbowLTracker != null)
    //    {
    //        var elbowSetting = CalibrateElbow(
    //            m_VRIK.solver.leftArm.wristToPalmAxis,
    //            m_VRIK.solver.leftArm.palmToThumbAxis,
    //            Vector3.forward,
    //            Vector3.up,
    //            Vector3.zero,
    //            m_ElbowLTracker
    //        );

    //        Transform elbowTarget = m_ElbowLOffset;
    //        elbowTarget.parent = elbowSetting.parent;
    //        elbowTarget.position = elbowSetting.position;
    //        elbowTarget.rotation = elbowSetting.rotation;

    //        m_VRIK.solver.leftArm.bendGoal = elbowTarget;
    //        m_VRIK.solver.leftArm.bendGoalWeight = 1.0f;
    //    }
    //    else
    //    {
    //        m_VRIK.solver.leftArm.bendGoalWeight = 0.0f;
    //    }

    //    //Right Elbow
    //    if (m_ElbowRTracker != null)
    //    {
    //        var elbowSetting = CalibrateElbow(
    //            m_VRIK.solver.rightArm.wristToPalmAxis,
    //            m_VRIK.solver.rightArm.palmToThumbAxis,
    //            Vector3.forward,
    //            Vector3.up,
    //            Vector3.zero,
    //            m_ElbowRTracker
    //        );

    //        Transform elbowTarget = m_ElbowROffset;
    //        elbowTarget.parent = elbowSetting.parent;
    //        elbowTarget.position = elbowSetting.position;
    //        elbowTarget.rotation = elbowSetting.rotation;

    //        m_VRIK.solver.rightArm.bendGoal = elbowTarget;
    //        m_VRIK.solver.rightArm.bendGoalWeight = 1.0f;
    //    }
    //    else
    //    {
    //        m_VRIK.solver.rightArm.bendGoalWeight = 0.0f;
    //    }
    //}

    //private CalibrateSetting CalibrateElbow(Vector3 wristToPalmAxis, Vector3 palmToThumbAxis, Vector3 forward, Vector3 up, Vector3 offset, Transform tracker)
    //{
    //    Vector3 position = tracker.position + tracker.rotation * Quaternion.LookRotation(forward, up) * offset;
    //    Vector3 leftHandUp = Vector3.Cross(wristToPalmAxis, palmToThumbAxis);
    //    Quaternion rotation = RootMotion.QuaTools.MatchRotation(tracker.rotation * Quaternion.LookRotation(forward, up), forward, up, wristToPalmAxis, leftHandUp);

    //    return new CalibrateSetting(
    //        tracker,
    //        position,
    //        rotation
    //    );
    //}
}

//public struct CalibrateSetting
//{
//    public Transform parent { get; }
//    public Vector3 position { get; }
//    public Quaternion rotation { get; }

//    public CalibrateSetting(Transform parent, Vector3 position, Quaternion rotation)
//    {
//        this.parent = parent;
//        this.position = position;
//        this.rotation = rotation;
//    }
//}
