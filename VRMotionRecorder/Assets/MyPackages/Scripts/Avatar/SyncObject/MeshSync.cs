using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSync : SyncObject
{
    [SerializeField] private SkinnedMeshRenderer m_Renderer;
    [SerializeField] private float m_LerpRate = 10f;
    [SerializeField] private FacialExpressionController m_FacialExpressionController;

    /* 同期データ */
    private float[] m_BlendShapes;

    protected override void Start()
    {
        base.Start();

        Init();
    }

    private void Init()
    {
        var mesh = m_Renderer.sharedMesh;
        var count = mesh.blendShapeCount;

        m_BlendShapes = new float[count];
    }

    public override void UpdateForOwner()
    {
        //NOP
    }

    public override void UpdateForClient()
    {
        if ( ( null == m_BlendShapes) ||
            ( 0 >= m_BlendShapes.Length )) 
        {
            return;
        }

        for ( int i = 0; i < m_BlendShapes.Length; ++i )
        {
            SetBlendShapeWeight(i);
        }
    }

    public override void OnEnqueue(MonobitEngine.MonobitStream stream)
    {
        if ((null == m_BlendShapes) ||
            (0 >= m_BlendShapes.Length))
        {
            return;
        }

        for (int i = 0; i < m_BlendShapes.Length; ++i)
        {
           m_BlendShapes[i] = m_Renderer.GetBlendShapeWeight(i);
        }

        stream.Enqueue(m_BlendShapes);
    }

    public override void OnDequeue(MonobitEngine.MonobitStream stream)
    {
        m_BlendShapes = (float[])stream.Dequeue();
    }

    private void SetBlendShapeWeight(int shape_index)
    {
        if ( null == m_Renderer )
        {
            return;
        }

        float new_weight = 0f;

        bool is_lerp = true;
        if ( ( null != m_FacialExpressionController ) &&
             ( true == m_FacialExpressionController.IsIgnoreLerpIndex(shape_index) ))
        {
            is_lerp = false; //滑らかに遷移する必要のない表情を再生中の場合はLerpしないようにする
        }

        if (is_lerp)
        {
            float current_weight = m_Renderer.GetBlendShapeWeight(shape_index);
            new_weight = Mathf.Lerp(current_weight, m_BlendShapes[shape_index], m_LerpRate * Time.deltaTime); //blendShapeを滑らかに遷移            
        }
        else
        {
            new_weight = m_BlendShapes[shape_index]; //blendShapeを即座に遷移
        }

        m_Renderer.SetBlendShapeWeight(shape_index, new_weight);
    }
}
