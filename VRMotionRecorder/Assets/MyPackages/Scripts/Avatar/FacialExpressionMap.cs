using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FacialExpressionMap")]
public class FacialExpressionMap : ScriptableObject
{
    public List<FacialExpression> m_FacialExpressions;
    public List<int> m_IgnoreLerpIndexes;
}


[Serializable]
public struct FacialExpression
{
    public string name;
    public KeyCode key;
    public int[] indexes;
    public bool isStopBlinkL;
    public bool isStopBlinkR;
}
