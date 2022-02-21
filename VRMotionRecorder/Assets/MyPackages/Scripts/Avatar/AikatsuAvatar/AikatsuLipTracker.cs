//========= Copyright 2019, HTC Corporation. All rights reserved. ===========
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ViveSR
{
    namespace anipal
    {
        namespace Lip
        {
            public class AikatsuLipTracker : MonoBehaviour
            {
                [SerializeField] private SkinnedMeshRenderer m_SkinnedMeshRenderer;
                //[SerializeField] private LipPatternSample[] m_Samples;
                [SerializeField] private float m_LerpRate = 10f;
                [SerializeField] private int[] m_LipIndexes;

                //[SerializeField] private LipIndex[] m_Lips;
                //[SerializeField] private int m_DefaultIndex = 0;
                //[SerializeField] private float m_Threshold = 0.2f;

                private Dictionary<LipShape_v2, float> LipWeightings;

                private int m_CurrentIndex = 0;

                private void Start()
                {
                    if (!SRanipal_Lip_Framework.Instance.EnableLip)
                    {
                        enabled = false;
                        return;
                    }

                }

                private void Update()
                {
                    if (SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING) return;

                    SRanipal_Lip_v2.GetLipWeightings(out LipWeightings);

                    UpdateLipShapes();
                    SmoothBlend();
                }

                private void UpdateLipShapes()
                {
                    

                    if ( (IsMouthLowerDown()) ||
                            (IsMouthSmile()) )
                    {
                        if (IsJawOpen())
                        {
                            m_CurrentIndex = m_LipIndexes[3];
                            return;
                        }

                        m_CurrentIndex = m_LipIndexes[1];
                        return;
                    }

                    if (IsJawOpen())
                    {
                        m_CurrentIndex = m_LipIndexes[0];
                        return;
                    }

                    if (IsMouthPout())
                    {
                        m_CurrentIndex = m_LipIndexes[2];
                        return;
                    }

                    if (IsMouthOverturn())
                    {
                        m_CurrentIndex = m_LipIndexes[4];
                        return;
                    }

                    m_CurrentIndex = m_LipIndexes[5];
                }

                private bool IsJawOpen()
                {
                    if ( 0.3f < LipWeightings[LipShape_v2.Jaw_Open])
                    {
                        return true;
                    }

                    return false;
                }

                private bool IsMouthLowerDown()
                {
                    if ( (0.5f < LipWeightings[LipShape_v2.Mouth_Lower_DownLeft]) ||
                         (0.5f < LipWeightings[LipShape_v2.Mouth_Lower_DownRight]))
                    {
                        return true;
                    }

                    return false;
                }

                private bool IsMouthSmile()
                {
                    if ((0.3f < LipWeightings[LipShape_v2.Mouth_Smile_Left]) ||
                         (0.3f < LipWeightings[LipShape_v2.Mouth_Smile_Right]) )
                    {
                        return true;
                    }

                    return false;
                }

                private bool IsMouthPout()
                {
                    if (0.5f < LipWeightings[LipShape_v2.Mouth_Pout])
                    {
                        return true;
                    }

                    return false;
                }

                private bool IsMouthOverturn()
                {
                    if ((0.1f < LipWeightings[LipShape_v2.Mouth_Upper_Overturn]) ||
                         (0.1f < LipWeightings[LipShape_v2.Mouth_Lower_Overturn]))
                    {
                        return true;
                    }

                    return false;
                }

                //private void UpdateLipShapes(Dictionary<LipShape_v2, float> lipWeightings)
                //{
                //    int index = m_CurrentIndex;
                //    bool is_match = false;

                //    for (int i = 0; i < m_Lips.Length; ++i)
                //    {
                //        var required_shapes = m_Lips[i].requiredShape;

                //        for ( int j = 0; j < required_shapes.Length; ++j )
                //        {
                //            float weight = lipWeightings[required_shapes[j]];

                //            if (m_Threshold > weight)
                //            {
                //                break;
                //            }

                //            //全て条件を満たした場合
                //            if ( (required_shapes.Length - 1) == j)
                //            {
                //                is_match = true;
                //            }
                //        }

                //        if ( true == is_match )
                //        {
                //            index = m_Lips[i].index;
                //            break;
                //        }
                //    }

                //    if (false == is_match)
                //    {
                //        index = m_DefaultIndex;
                //    }

                //    if (m_CurrentIndex == index) return;

                //    m_CurrentIndex = index;
                //}

                //private void UpdateLipShapes(Dictionary<LipShape_v2, float> lipWeightings)
                //{
                //    int index = m_CurrentIndex;
                //    float min_dif = Mathf.Infinity;

                //    for (int i = 0; i < m_Samples.Length; ++i)
                //    {
                //        float dif = 0f;
                //        for (int j = 0; j < 25; ++j )
                //        {
                //            LipShape_v2 shape = (LipShape_v2)j;
                //            bool is_match = false;
                //            for (int k = 0; k < m_Samples[i].weightPairs.Length; ++k)
                //            {
                //                if (shape != m_Samples[i].weightPairs[k].shape)
                //                {
                //                    continue;
                //                }

                //                dif += Mathf.Abs(m_Samples[i].weightPairs[k].weight - lipWeightings[shape] * 100f);

                //                is_match = true;
                //                break;
                //            }

                //            if (false == is_match)
                //            {
                //                dif += lipWeightings[shape] * 100f;
                //            }
                //        }

                //        if (dif < min_dif)
                //        {
                //            min_dif = dif;
                //            index = m_Samples[i].blendShapeIndex;
                //        }
                //    }

                //    if ( m_CurrentIndex == index ) return;

                //    m_CurrentIndex = index;


                //}

                //private void SmoothBlend()
                //{
                //    for (int i = 0; i < m_Lips.Length; ++i)
                //    {
                //        if (m_CurrentIndex == m_Lips[i].index)
                //        {
                //            float current_value = m_SkinnedMeshRenderer.GetBlendShapeWeight(m_CurrentIndex);
                //            if (100f <= current_value)
                //            {
                //                continue;
                //            }
                //            float value = Mathf.Lerp(current_value, 100f, m_LerpRate * Time.deltaTime);
                //            m_SkinnedMeshRenderer.SetBlendShapeWeight(m_CurrentIndex, value);
                //        }
                //        else
                //        {
                //            float current_value = m_SkinnedMeshRenderer.GetBlendShapeWeight(m_Lips[i].index);
                //            if (0f >= current_value)
                //            {
                //                continue;
                //            }
                //            float value = Mathf.Lerp(current_value, 0f, m_LerpRate * Time.deltaTime);
                //            m_SkinnedMeshRenderer.SetBlendShapeWeight(m_Lips[i].index, value);
                //        }
                //    }
                //}

                private void SmoothBlend()
                {
                    for (int i = 0; i < m_LipIndexes.Length; ++i)
                    {
                        if (m_CurrentIndex == m_LipIndexes[i])
                        {
                            float current_value = m_SkinnedMeshRenderer.GetBlendShapeWeight(m_CurrentIndex);
                            if (100f <= current_value)
                            {
                                continue;
                            }
                            float value = Mathf.Lerp(current_value, 100f, m_LerpRate * Time.deltaTime);
                            m_SkinnedMeshRenderer.SetBlendShapeWeight(m_CurrentIndex, value);
                        }
                        else
                        {
                            float current_value = m_SkinnedMeshRenderer.GetBlendShapeWeight(m_LipIndexes[i]);
                            if (0f >= current_value)
                            {
                                continue;
                            }
                            float value = Mathf.Lerp(current_value, 0f, m_LerpRate * Time.deltaTime);
                            m_SkinnedMeshRenderer.SetBlendShapeWeight(m_LipIndexes[i], value);
                        }
                    }
                }

                private void RenderModelLipShape(LipShapeTable_v2 lipShapeTable, Dictionary<LipShape_v2, float> weighting)
                {
                    for (int i = 0; i < lipShapeTable.lipShapes.Length; i++)
                    {
                        int targetIndex = (int)lipShapeTable.lipShapes[i];
                        if (targetIndex > (int)LipShape_v2.Max || targetIndex < 0) continue;
                        lipShapeTable.skinnedMeshRenderer.SetBlendShapeWeight(i, weighting[(LipShape_v2)targetIndex] * 100);
                    }
                }

                //[Serializable]
                //private enum LipPattern : int
                //{
                //    DEFAULT = 0,
                //    A,
                //    I,
                //    U,
                //    E,
                //    O,
                //    SMILE,
                //    SAD
                //}

                //[Serializable]
                //private struct LipPatternSample
                //{
                //    public LipPattern lipPattern;
                //    public int blendShapeIndex;
                //    public LipWeightPair[] weightPairs;
                //}

                //[Serializable]
                //private struct LipWeightPair
                //{
                //    public LipShape_v2 shape;
                //    public float weight;
                //}

                //[Serializable]
                //private struct LipIndex
                //{
                //    public int index;
                //    public LipShape_v2[] requiredShape;
                //}
            }
        }
    }
}