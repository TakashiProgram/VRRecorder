using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class AvatarCalibrator : MonoBehaviour
{
    public Action<AvatarCalibrationSettings> OnLoadCompleted;

    [SerializeField] private Transform m_HeadRotationOffset;

    //同期用プロパティ
    [HideInInspector] public bool m_IsSynced = true;
    [HideInInspector] public float[] m_FloatParams = new float[5];
    [HideInInspector] public Vector3[] m_Vector3Params = new Vector3[1];

    private VRIK m_VRIK;
    private AvatarCalibrationSettings m_AvatarCalibrationSettings;

    private static readonly string SETTINGS_PATH = "AvatarCalibrationSettings.json";
    

    public void SetTarget(VRIK target)
    {
        m_VRIK = target;
        //Load();
    }

    public void Save()
    {
        JsonHelper<AvatarCalibrationSettings>.Write(SETTINGS_PATH, m_AvatarCalibrationSettings);
    }

    public void Load()
    {
        m_AvatarCalibrationSettings = JsonHelper<AvatarCalibrationSettings>.Read(SETTINGS_PATH);

        ChangeScale(m_AvatarCalibrationSettings.s_Scale);
        ChangeHeadRotationOffset(m_AvatarCalibrationSettings.s_HeadRotationOffset);
        ChangeLeftShoulderRotationWeight(m_AvatarCalibrationSettings.s_LeftShoulderRotationWeight);
        ChangeRightShoulderRotationWeight(m_AvatarCalibrationSettings.s_RightShoulderRotationWeight);
        ChangeLeftArmLenghtMlp(m_AvatarCalibrationSettings.s_LeftArmLengthMlp);
        ChangeRightArmLenghtMlp(m_AvatarCalibrationSettings.s_RightArmLengthMlp);

        if ( null != OnLoadCompleted )
        {
            OnLoadCompleted(m_AvatarCalibrationSettings);
        }
    }

    public void ChangeScale(float value)
    {
        m_VRIK.gameObject.transform.localScale = new Vector3( value, value, value);
        m_AvatarCalibrationSettings.s_Scale = value;

        m_FloatParams[0] = value;
        m_IsSynced = false;
    }

    public void ChangeHeadRotationOffset(Vector3 value)
    {
        m_HeadRotationOffset.localRotation = Quaternion.Euler(value);
        m_AvatarCalibrationSettings.s_HeadRotationOffset = value;

        m_Vector3Params[0] = value;
        m_IsSynced = false;
    }

    public void ChangeLeftShoulderRotationWeight(float value)
    {
        m_VRIK.solver.leftArm.shoulderRotationWeight = value;
        m_AvatarCalibrationSettings.s_LeftShoulderRotationWeight = value;

        m_FloatParams[1] = value;
        m_IsSynced = false;
    }

    public void ChangeRightShoulderRotationWeight(float value)
    {
        m_VRIK.solver.rightArm.shoulderRotationWeight = value;
        m_AvatarCalibrationSettings.s_RightShoulderRotationWeight = value;

        m_FloatParams[2] = value;
        m_IsSynced = false;
    }

    public void ChangeLeftArmLenghtMlp(float value)
    {
        m_VRIK.solver.leftArm.armLengthMlp = value;
        m_AvatarCalibrationSettings.s_LeftArmLengthMlp = value;

        m_FloatParams[3] = value;
        m_IsSynced = false;
    }

    public void ChangeRightArmLenghtMlp(float value)
    {
        m_VRIK.solver.rightArm.armLengthMlp = value;
        m_AvatarCalibrationSettings.s_RightArmLengthMlp = value;

        m_FloatParams[4] = value;
        m_IsSynced = false;
    }

    public void OnEnqueue()
    {
        m_IsSynced = true;
    }

    public void OnDequeue()
    {
        m_VRIK.gameObject.transform.localScale = new Vector3(m_FloatParams[0], m_FloatParams[0], m_FloatParams[0]);
        m_HeadRotationOffset.localRotation = Quaternion.Euler(m_Vector3Params[0]);
        m_VRIK.solver.leftArm.shoulderRotationWeight = m_FloatParams[1];
        m_VRIK.solver.rightArm.shoulderRotationWeight = m_FloatParams[2];
        m_VRIK.solver.leftArm.armLengthMlp = m_FloatParams[3];
        m_VRIK.solver.rightArm.armLengthMlp = m_FloatParams[4];
    }
}

[System.Serializable]
public struct AvatarCalibrationSettings
{
    public float s_Scale;
    public float s_HeightOffset; //暫定：ルームセットアップの高さズレを手っ取り早く吸収するため
    public Vector3 s_HeadRotationOffset;
    public float s_LeftShoulderRotationWeight;
    public float s_RightShoulderRotationWeight;
    public float s_LeftArmLengthMlp;
    public float s_RightArmLengthMlp;
}
