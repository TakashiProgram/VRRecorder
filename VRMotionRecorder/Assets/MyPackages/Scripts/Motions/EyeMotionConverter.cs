using Entum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EyeMotionConverter : MonoBehaviour
{
    [SerializeField]
    private Transform m_Eye;

    [SerializeField]
    private MotionDataRecorder m_MotionDataRecorder;


    private Vector3[] m_EyePosition = new Vector3[1];

    private Quaternion[] m_EyeRotation = new Quaternion[1];

    private float[] m_CuurentTime = new float[1];

    private float m_ElapsedTime = 0;

    private Motion m_Motion;

    private bool m_IsRecord = false;
    void Update()
    {
        if (true == m_IsRecord)
        {
            StartRecording();
        }
    }

    public void ExportEyeAnim()
    {
        m_IsRecord = false;
        var clip = new AnimationClip { frameRate = 30 };
        AnimationUtility.SetAnimationClipSettings(clip, new AnimationClipSettings { loopTime = false, keepOriginalPositionY = true });

        {
            //Pos
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            for (int i = 0; i < m_EyePosition.Length; i++)
            {
                curveX.AddKey(m_CuurentTime[i], m_EyePosition[i].x);
                curveY.AddKey(m_CuurentTime[i], m_EyePosition[i].y);
                curveZ.AddKey(m_CuurentTime[i], m_EyePosition[i].z);
            }

            const string muscleX = "localPosition.x";
            clip.SetCurve("", typeof(Transform), muscleX, curveX);
            const string muscleY = "localPosition.y";
            clip.SetCurve("", typeof(Transform), muscleY, curveY);
            const string muscleZ = "localPosition.z";
            clip.SetCurve("", typeof(Transform), muscleZ, curveZ);
        }

        {
            //Rot
            var curve_rotX = new AnimationCurve();
            var curve_rotY = new AnimationCurve();
            var curve_rotZ = new AnimationCurve();
            var curve_rotW = new AnimationCurve();
            for (int i = 0; i < m_EyeRotation.Length; i++)
            {
                curve_rotX.AddKey(m_CuurentTime[i], m_EyeRotation[i].x);
                curve_rotY.AddKey(m_CuurentTime[i], m_EyeRotation[i].y);
                curve_rotZ.AddKey(m_CuurentTime[i], m_EyeRotation[i].z);
                curve_rotW.AddKey(m_CuurentTime[i], m_EyeRotation[i].w);
            }

            const string muscleX = "localRotation.x";
            clip.SetCurve("", typeof(Transform), muscleX, curve_rotX);
            const string muscleY = "localRotation.y";
            clip.SetCurve("", typeof(Transform), muscleY, curve_rotY);
            const string muscleZ = "localRotation.z";
            clip.SetCurve("", typeof(Transform), muscleZ, curve_rotZ);
            const string muscleW = "localRotation.w";
            clip.SetCurve("", typeof(Transform), muscleW, curve_rotW);
        }

        clip.EnsureQuaternionContinuity();

        var path = string.Format("Assets/Resources/Scene" + m_MotionDataRecorder.GetScene()
                                + "/Cat" + m_MotionDataRecorder.GetCat() + "/" + m_MotionDataRecorder.GetAnimatorName() + "/Take" + m_MotionDataRecorder.GetTake() + m_Eye.name + "_Humanoid.anim", DateTime.Now);

        var uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(path);
        m_Motion = clip;
        AssetDatabase.CreateAsset(clip, uniqueAssetPath);
        AssetDatabase.SaveAssets();

    }
    public void SetRecord(bool record)
    {
        m_IsRecord = record;
    }
    public void StartRecording()
    {
        m_EyePosition[m_EyePosition.Length - 1] = m_Eye.transform.localPosition;
        Array.Resize(ref m_EyePosition, m_EyePosition.Length + 1);

        m_EyeRotation[m_EyeRotation.Length - 1] = m_Eye.transform.localRotation;
        Array.Resize(ref m_EyeRotation, m_EyeRotation.Length + 1);

        m_ElapsedTime += Time.deltaTime;
        m_CuurentTime[m_CuurentTime.Length - 1] = m_ElapsedTime;
        Array.Resize(ref m_CuurentTime, m_CuurentTime.Length + 1);
    }

    public Motion GetMotion()
    {
        return m_Motion;
    }


}
