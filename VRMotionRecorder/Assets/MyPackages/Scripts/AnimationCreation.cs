using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationCreation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    private bool m_IsStart = false;

    private Vector3[] m_TestPostion = new Vector3[1];

    private Quaternion[] m_TestRotation = new Quaternion[1];

    private float[] m_Time = new float[1];

    private float[] m_Test = new float[1];

    private float m_ElapsedTime = 0;

    [SerializeField]
    private GameObject m_Transform;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (true == m_IsStart)
            {
                m_IsStart = false;
                Debug.Log("録画終了");
                ExportHumanoidAnim();
            }
            else
            {
                m_IsStart = true;
                m_ElapsedTime = 0;
                Debug.Log("録画開始");
                // m_Time = Time.deltaTime;

            }


        }
        if (true == m_IsStart)
        {
            m_TestPostion[m_TestPostion.Length - 1] = this.transform.position;
            Array.Resize(ref m_TestPostion, m_TestPostion.Length + 1);

            m_TestRotation[m_TestRotation.Length - 1] = this.transform.rotation;
            Array.Resize(ref m_TestRotation, m_TestRotation.Length + 1);

            m_ElapsedTime += Time.deltaTime;
            m_Time[m_Time.Length - 1] = m_ElapsedTime;
            Array.Resize(ref m_Time, m_Time.Length + 1);
        }
    }

    public void ExportHumanoidAnim()
    {
        var clip = new AnimationClip { frameRate = 30 };
        AnimationUtility.SetAnimationClipSettings(clip, new AnimationClipSettings { loopTime = false, keepOriginalPositionY = true });

        {
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            for (int i = 0; i < m_TestPostion.Length; i++)
            {
                curveX.AddKey(m_Time[i], m_TestPostion[i].x);
                curveY.AddKey(m_Time[i], m_TestPostion[i].y);
                curveZ.AddKey(m_Time[i], m_TestPostion[i].z);
            }
            //Pos
            const string muscleX = "localPosition.x";
            clip.SetCurve("", typeof(Transform), muscleX, curveX);
            const string muscleY = "localPosition.y";
            clip.SetCurve("", typeof(Transform), muscleY, curveY);
            const string muscleZ = "localPosition.z";
            clip.SetCurve("", typeof(Transform), muscleZ, curveZ);
        }

        {
            var curve_rotX = new AnimationCurve();
            var curve_rotY = new AnimationCurve();
            var curve_rotZ = new AnimationCurve();
            var curve_rotW = new AnimationCurve();
            for (int i = 0; i < m_TestRotation.Length; i++)
            {
                curve_rotX.AddKey(m_Time[i], m_TestRotation[i].x);
                curve_rotY.AddKey(m_Time[i], m_TestRotation[i].y);
                curve_rotZ.AddKey(m_Time[i], m_TestRotation[i].z);
                curve_rotW.AddKey(m_Time[i], m_TestRotation[i].w);
            }
            //Rot
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

        //Debug.Log("Assets/Resources/Scene" + m_MotionDataRecorder.GetScene()
        //                         + "/Cat" + m_MotionDataRecorder.GetCat() + "/" + ".anim");

        var path = string.Format("Assets/Resources/Scene" + ".anim", DateTime.Now);
        // var path = string.Format("Assets/Resources/RecordMotion_{0:yyyy_MM_dd_HH_mm_ss}_Humanoid.anim", DateTime.Now);
        var uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(path);
        //m_Motion = clip;
        AssetDatabase.CreateAsset(clip, uniqueAssetPath);
        AssetDatabase.SaveAssets();

    }
}
