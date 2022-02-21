using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using ViveSR.anipal.Eye;

public class AikatsuEyeTracker : MonoBehaviour
{
    [SerializeField] private Transform[] EyesModels = new Transform[0];
    [SerializeField] private BlinkController m_BlinkController = null;
    [SerializeField] private float m_EyeMaxMoveDistance = 0.01f;　//黒目が一番端にきたときの平面方向の移動量
    [SerializeField] private float m_EyeMaxMoveDepth = 0.01f; //黒目が一番端にきたときの奥行方向への移動量
    [SerializeField] private float m_EyeMaxRotation = 10f; //黒目が一番端にきたときの垂直軸に対する回転

    [SerializeField] private List<EyeShapeTable_v2> EyeShapeTables;
    /// <summary>
    /// Customize this curve to fit the blend shapes of your avatar.
    /// </summary>
    [SerializeField] private AnimationCurve EyebrowAnimationCurveUpper;
    /// <summary>
    /// Customize this curve to fit the blend shapes of your avatar.
    /// </summary>
    [SerializeField] private AnimationCurve EyebrowAnimationCurveLower;
    /// <summary>
    /// Customize this curve to fit the blend shapes of your avatar.
    /// </summary>
    [SerializeField] private AnimationCurve EyebrowAnimationCurveHorizontal;

    public bool NeededToGetData = true;
    private Dictionary<EyeShape_v2, float> EyeWeightings = new Dictionary<EyeShape_v2, float>();
    private AnimationCurve[] EyebrowAnimationCurves = new AnimationCurve[(int)EyeShape_v2.Max];
    private GameObject[] EyeAnchors;
    private const int NUM_OF_EYES = 2;
    private static EyeData_v2 eyeData = new EyeData_v2();
    private bool eye_callback_registered = false;

    private Vector3[] m_EyePosOffsets;

    private void Start()
    {
        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }

        SetEyesModels(EyesModels[0], EyesModels[1]);
        SetEyeShapeTables(EyeShapeTables);

        AnimationCurve[] curves = new AnimationCurve[(int)EyeShape_v2.Max];
        for (int i = 0; i < EyebrowAnimationCurves.Length; ++i)
        {
            if (i == (int)EyeShape_v2.Eye_Left_Up || i == (int)EyeShape_v2.Eye_Right_Up) curves[i] = EyebrowAnimationCurveUpper;
            else if (i == (int)EyeShape_v2.Eye_Left_Down || i == (int)EyeShape_v2.Eye_Right_Down) curves[i] = EyebrowAnimationCurveLower;
            else curves[i] = EyebrowAnimationCurveHorizontal;
        }
        SetEyeShapeAnimationCurves(curves);

        if (null != m_BlinkController)
        {
            foreach (var table in EyeShapeTables)
            {
                for (int i = 0; i < table.eyeShapes.Length; ++i)
                {
                    EyeShape_v2 eyeShape = table.eyeShapes[i];
                    if (eyeShape > EyeShape_v2.Max || eyeShape < 0) continue;

                    if (eyeShape == EyeShape_v2.Eye_Left_Blink || eyeShape == EyeShape_v2.Eye_Right_Blink)
                    {
                        m_BlinkController.SetBlinkIndex(table.skinnedMeshRenderer, i, (eyeShape == EyeShape_v2.Eye_Left_Blink));
                    }
                }
            }
                
        }
    }

    private void Update()
    {
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

        if (NeededToGetData)
        {
            if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
            {
                SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = true;
            }
            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
            {
                SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false)
                SRanipal_Eye_API.GetEyeData_v2(ref eyeData);

            bool isLeftEyeActive = false;
            bool isRightEyeAcitve = false;
            if (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING)
            {
                isLeftEyeActive = eyeData.no_user;
                isRightEyeAcitve = eyeData.no_user;
            }
            else if (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT)
            {
                isLeftEyeActive = true;
                isRightEyeAcitve = true;
            }

            if (isLeftEyeActive || isRightEyeAcitve)
            {
                if (eye_callback_registered == true)
                    SRanipal_Eye_v2.GetEyeWeightings(out EyeWeightings, eyeData);
                else
                    SRanipal_Eye_v2.GetEyeWeightings(out EyeWeightings);
                UpdateEyeShapes(EyeWeightings);
            }
            else
            {
                for (int i = 0; i < (int)EyeShape_v2.Max; ++i)
                {
                    bool isBlink = ((EyeShape_v2)i == EyeShape_v2.Eye_Left_Blink || (EyeShape_v2)i == EyeShape_v2.Eye_Right_Blink);
                    EyeWeightings[(EyeShape_v2)i] = isBlink ? 1 : 0;
                }

                UpdateEyeShapes(EyeWeightings);

                return;
            }

            Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal = Vector3.zero;
            if (eye_callback_registered == true)
            {
                if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
            }
            else
            {
                if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }

            }
            //UpdateGazeRay(GazeDirectionCombinedLocal);
            UpdateGazeRay(eyeData);
        }
    }
    private void Release()
    {
        if (eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }
    }
    private void OnDestroy()
    {
        //DestroyEyeAnchors();
    }

    public void SetEyesModels(Transform leftEye, Transform rightEye)
    {
        if (leftEye != null && rightEye != null)
        {
            EyesModels = new Transform[NUM_OF_EYES] { leftEye, rightEye };
            //DestroyEyeAnchors();
            //CreateEyeAnchors();

            m_EyePosOffsets = new Vector3[NUM_OF_EYES] { leftEye.localPosition, rightEye.localPosition };
        }
    }

    public void SetEyeShapeTables(List<EyeShapeTable_v2> eyeShapeTables)
    {
        bool valid = true;
        if (eyeShapeTables == null)
        {
            valid = false;
        }
        else
        {
            for (int table = 0; table < eyeShapeTables.Count; ++table)
            {
                if (eyeShapeTables[table].skinnedMeshRenderer == null)
                {
                    valid = false;
                    break;
                }
                for (int shape = 0; shape < eyeShapeTables[table].eyeShapes.Length; ++shape)
                {
                    EyeShape_v2 eyeShape = eyeShapeTables[table].eyeShapes[shape];
                    if (eyeShape > EyeShape_v2.Max || eyeShape < 0)
                    {
                        valid = false;
                        break;
                    }
                }
            }
        }
        if (valid)
            EyeShapeTables = eyeShapeTables;
    }

    public void SetEyeShapeAnimationCurves(AnimationCurve[] eyebrowAnimationCurves)
    {
        if (eyebrowAnimationCurves.Length == (int)EyeShape_v2.Max)
            EyebrowAnimationCurves = eyebrowAnimationCurves;
    }

    //public void UpdateGazeRay(Vector3 gazeDirectionCombinedLocal)
    //{
    //    for (int i = 0; i < EyesModels.Length; ++i)
    //    {
    //        Vector3 target = EyeAnchors[i].transform.TransformPoint(gazeDirectionCombinedLocal);
    //        EyesModels[i].LookAt(target);
    //    }
    //}

    public void UpdateGazeRay(EyeData_v2 eye_data)
    {
        var pos = Vector2.zero;

        for (int i = 0; i < EyesModels.Length; ++i)
        {
            if (0 == i)
            {
                if (!eye_data.verbose_data.left.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_PUPIL_POSITION_IN_SENSOR_AREA_VALIDITY))
                {
                    continue;
                }

                //pupil_position_in_sensor_area→0~1
                pos = eye_data.verbose_data.left.pupil_position_in_sensor_area;　
            }
            else
            {
                if (!eye_data.verbose_data.right.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_PUPIL_POSITION_IN_SENSOR_AREA_VALIDITY))
                {
                    continue;
                }

                //pupil_position_in_sensor_area→0~1
                pos = eye_data.verbose_data.right.pupil_position_in_sensor_area;　
            }
            
            var radian = Mathf.Atan2(pos.y - 0.5f, pos.x - 0.5f);
            var clamp_rate_x = Mathf.Abs(Mathf.Cos(radian));
            var clamp_rate_y = Mathf.Abs(Mathf.Sin(radian));

            //黒目の位置をx,yそれぞれ-1~1に変換（まぶた突き抜け防止のため暫定的に-0.5~0.5にClamp）
            pos = new Vector2(
                Mathf.Clamp((pos.x * 2f - 1f), -0.5f * clamp_rate_x, 0.5f * clamp_rate_x),
                Mathf.Clamp((pos.y * 2f - 1f), -0.5f * clamp_rate_y, 0.5f * clamp_rate_y)
                );

            //アイカツモデル用に軸を変更、X座標に応じて奥行を追加
            Vector3 move = new Vector3(
                pos.y * m_EyeMaxMoveDistance,
                -pos.x * m_EyeMaxMoveDistance,
                Mathf.Abs(pos.x) * m_EyeMaxMoveDepth
                );
            
            EyesModels[i].localPosition = m_EyePosOffsets[i] + move;
            EyesModels[i].localRotation = Quaternion.Euler(-pos.x * m_EyeMaxRotation, 0f, 0f);
        }
    }

    public void UpdateEyeShapes(Dictionary<EyeShape_v2, float> eyeWeightings)
    {
        foreach (var table in EyeShapeTables)
            RenderModelEyeShape(table, eyeWeightings);
    }

    private void RenderModelEyeShape(EyeShapeTable_v2 eyeShapeTable, Dictionary<EyeShape_v2, float> weighting)
    {
        for (int i = 0; i < eyeShapeTable.eyeShapes.Length; ++i)
        {
            EyeShape_v2 eyeShape = eyeShapeTable.eyeShapes[i];
            if (eyeShape > EyeShape_v2.Max || eyeShape < 0) continue;

            if (eyeShape == EyeShape_v2.Eye_Left_Blink || eyeShape == EyeShape_v2.Eye_Right_Blink)
            {
                if (null != m_BlinkController)
                {
                    m_BlinkController.UpdateEyeOpenRate(weighting[eyeShape], (eyeShape == EyeShape_v2.Eye_Left_Blink));
                }
                //eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, weighting[eyeShape] * 100f);
            }
            else
            {
                AnimationCurve curve = EyebrowAnimationCurves[(int)eyeShape];
                eyeShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, curve.Evaluate(weighting[eyeShape]) * 100f);
            }
        }
    }


    private void CreateEyeAnchors()
    {
        EyeAnchors = new GameObject[NUM_OF_EYES];
        for (int i = 0; i < NUM_OF_EYES; ++i)
        {
            EyeAnchors[i] = new GameObject();
            EyeAnchors[i].name = "EyeAnchor_" + i;
            EyeAnchors[i].transform.SetParent(gameObject.transform);
            EyeAnchors[i].transform.localPosition = EyesModels[i].localPosition;
            EyeAnchors[i].transform.localRotation = EyesModels[i].localRotation;
            EyeAnchors[i].transform.localScale = EyesModels[i].localScale;
        }
    }

    private void DestroyEyeAnchors()
    {
        if (EyeAnchors != null)
        {
            foreach (var obj in EyeAnchors)
                if (obj != null) Destroy(obj);
        }
    }
    private static void EyeCallback(ref EyeData_v2 eye_data)
    {
        eyeData = eye_data;
    }
}
