using Entum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MotionConverter : MonoBehaviour
{
    [SerializeField]
    private MotionDataRecorder m_MotionDataRecorder;

    private Motion m_Motion;

    void Update()
    {
     
    }

    public void ExportHumanoidAnim()
    {
        var clip = new AnimationClip { frameRate = 30 };
        AnimationUtility.SetAnimationClipSettings(clip, new AnimationClipSettings { loopTime = false ,keepOriginalPositionY = true});

        
        {
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            foreach (var item in m_MotionDataRecorder.GetPoses().Poses)
            {
                curveX.AddKey(item.Time, item.BodyPosition.x);
                curveY.AddKey(item.Time, item.BodyPosition.y);
                curveZ.AddKey(item.Time, item.BodyPosition.z);
            }

            const string muscleX = "RootT.x";
            clip.SetCurve("", typeof(Animator), muscleX, curveX);
            const string muscleY = "RootT.y";
            clip.SetCurve("", typeof(Animator), muscleY, curveY);
            const string muscleZ = "RootT.z";
            clip.SetCurve("", typeof(Animator), muscleZ, curveZ);
        }
        // Leftfoot position
        {
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            foreach (var item in m_MotionDataRecorder.GetPoses().Poses)
            {
                curveX.AddKey(item.Time, item.LeftfootIK_Pos.x);
                curveY.AddKey(item.Time, item.LeftfootIK_Pos.y);
                curveZ.AddKey(item.Time, item.LeftfootIK_Pos.z);
            }

            const string muscleX = "LeftFootT.x";
            clip.SetCurve("", typeof(Animator), muscleX, curveX);
            const string muscleY = "LeftFootT.y";
            clip.SetCurve("", typeof(Animator), muscleY, curveY);
            const string muscleZ = "LeftFootT.z";
            clip.SetCurve("", typeof(Animator), muscleZ, curveZ);
        }
        // Rightfoot position
        {
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            foreach (var item in m_MotionDataRecorder.GetPoses().Poses)
            {
                curveX.AddKey(item.Time, item.RightfootIK_Pos.x);
                curveY.AddKey(item.Time, item.RightfootIK_Pos.y);
                curveZ.AddKey(item.Time, item.RightfootIK_Pos.z);
            }

            const string muscleX = "RightFootT.x";
            clip.SetCurve("", typeof(Animator), muscleX, curveX);
            const string muscleY = "RightFootT.y";
            clip.SetCurve("", typeof(Animator), muscleY, curveY);
            const string muscleZ = "RightFootT.z";
            clip.SetCurve("", typeof(Animator), muscleZ, curveZ);
        }
        // body rotation
        {
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            var curveW = new AnimationCurve();
            foreach (var item in m_MotionDataRecorder.GetPoses().Poses)
            {
                curveX.AddKey(item.Time, item.BodyRotation.x);
                curveY.AddKey(item.Time, item.BodyRotation.y);
                curveZ.AddKey(item.Time, item.BodyRotation.z);
                curveW.AddKey(item.Time, item.BodyRotation.w);
            }

            const string muscleX = "RootQ.x";
            clip.SetCurve("", typeof(Animator), muscleX, curveX);
            const string muscleY = "RootQ.y";
            clip.SetCurve("", typeof(Animator), muscleY, curveY);
            const string muscleZ = "RootQ.z";
            clip.SetCurve("", typeof(Animator), muscleZ, curveZ);
            const string muscleW = "RootQ.w";
            clip.SetCurve("", typeof(Animator), muscleW, curveW);
        }
        // Leftfoot rotation
        {
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            var curveW = new AnimationCurve();
            foreach (var item in m_MotionDataRecorder.GetPoses().Poses)
            {
                curveX.AddKey(item.Time, item.LeftfootIK_Rot.x);
                curveY.AddKey(item.Time, item.LeftfootIK_Rot.y);
                curveZ.AddKey(item.Time, item.LeftfootIK_Rot.z);
                curveW.AddKey(item.Time, item.LeftfootIK_Rot.w);
            }

            const string muscleX = "LeftFootQ.x";
            clip.SetCurve("", typeof(Animator), muscleX, curveX);
            const string muscleY = "LeftFootQ.y";
            clip.SetCurve("", typeof(Animator), muscleY, curveY);
            const string muscleZ = "LeftFootQ.z";
            clip.SetCurve("", typeof(Animator), muscleZ, curveZ);
            const string muscleW = "LeftFootQ.w";
            clip.SetCurve("", typeof(Animator), muscleW, curveW);
        }
        // Rightfoot rotation
        {
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            var curveW = new AnimationCurve();
            foreach (var item in m_MotionDataRecorder.GetPoses().Poses)
            {
                curveX.AddKey(item.Time, item.RightfootIK_Rot.x);
                curveY.AddKey(item.Time, item.RightfootIK_Rot.y);
                curveZ.AddKey(item.Time, item.RightfootIK_Rot.z);
                curveW.AddKey(item.Time, item.RightfootIK_Rot.w);
            }

            const string muscleX = "RightFootQ.x";
            clip.SetCurve("", typeof(Animator), muscleX, curveX);
            const string muscleY = "RightFootQ.y";
            clip.SetCurve("", typeof(Animator), muscleY, curveY);
            const string muscleZ = "RightFootQ.z";
            clip.SetCurve("", typeof(Animator), muscleZ, curveZ);
            const string muscleW = "RightFootQ.w";
            clip.SetCurve("", typeof(Animator), muscleW, curveW);
        }

        // muscles
        for (int i = 0; i < HumanTrait.MuscleCount; i++)
        {
            var curve = new AnimationCurve();
            foreach (var item in m_MotionDataRecorder.GetPoses().Poses)
            {
                curve.AddKey(item.Time, item.Muscles[i]);
            }

            var muscle = HumanTrait.MuscleName[i];
            if (MotionDataSettings.TraitPropMap.ContainsKey(muscle))
            {
                muscle = MotionDataSettings.TraitPropMap[muscle];
            }

            clip.SetCurve("", typeof(Animator), muscle, curve);
        }

        clip.EnsureQuaternionContinuity();

        //Debug.Log("Assets/Resources/Scene" + m_MotionDataRecorder.GetScene()
        //                         + "/Cat" + m_MotionDataRecorder.GetCat() + "/" + ".anim");

        var path = string.Format("Assets/Resources/Scene" + m_MotionDataRecorder.GetScene() 
                                 + "/Cat" + m_MotionDataRecorder.GetCat() + "/" + m_MotionDataRecorder.GetAnimatorName() +"/"+ "Humanoid.anim", DateTime.Now);
       // var path = string.Format("Assets/Resources/RecordMotion_{0:yyyy_MM_dd_HH_mm_ss}_Humanoid.anim", DateTime.Now);
        var uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(path);
        m_Motion = clip;
        AssetDatabase.CreateAsset(clip, uniqueAssetPath);
        AssetDatabase.SaveAssets();
    }

    public Motion GetMotion()
    {
        return m_Motion;
    }
}