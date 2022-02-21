using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpressionController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_Renderer;
    [SerializeField] private FacialExpressionMap m_Map;

    private List<FacialExpression> m_CurrentFacials = new List<FacialExpression>();

    void Update()
    {
        if ( ( null == m_Map ) ||
            ( 0 >= m_Map.m_FacialExpressions.Count ))
        {
            return;
        }

        for ( int i = 0; i < m_Map.m_FacialExpressions.Count; ++i )
        {
            FacialExpression facial = m_Map.m_FacialExpressions[i];
            if (Input.GetKeyDown(facial.key))
            {
                SetFacial(facial.indexes);
                if ( false == m_CurrentFacials.Contains(facial) )
                {
                    m_CurrentFacials.Add(facial);
                }
            }
            else if (Input.GetKeyUp(m_Map.m_FacialExpressions[i].key))
            {
                ResetFacial(m_Map.m_FacialExpressions[i].indexes);
                if (true == m_CurrentFacials.Contains(facial))
                {
                    m_CurrentFacials.Remove(facial);
                }
            }
        }
    }

    public bool CanBlink( bool is_left )
    {
        if ( ( null == m_CurrentFacials ) ||
            ( 0 >= m_CurrentFacials .Count ))
        {
            return true;
        }

        for (int i = 0; i < m_CurrentFacials.Count; ++i)
        {
            bool is_stop = (is_left) ? m_CurrentFacials[i].isStopBlinkL : m_CurrentFacials[i].isStopBlinkR;
            if (true == is_stop)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsIgnoreLerpIndex( int index )
    {
        if ((null == m_Map) ||
            (null == m_Map.m_IgnoreLerpIndexes) ||
            (0 >= m_Map.m_IgnoreLerpIndexes.Count))
        {
            return false;
        }

        return m_Map.m_IgnoreLerpIndexes.Contains(index);
    }

    private void SetFacial(int[] indexes)
    {
        if ( null == m_Renderer )
        {
            return;
        }

        for (int i = 0; i < indexes.Length; ++i)
        {
            m_Renderer.SetBlendShapeWeight(indexes[i], 100f);
        }
    }

    private void ResetFacial(int[] indexes)
    {
        if (null == m_Renderer)
        {
            return;
        }

        for (int i = 0; i < indexes.Length; ++i)
        {
            m_Renderer.SetBlendShapeWeight(indexes[i], 0f);
        }
    }
}
