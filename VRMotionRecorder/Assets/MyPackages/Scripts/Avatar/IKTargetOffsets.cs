using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/IKTargetOffsets")]
public class IKTargetOffsets : ScriptableObject
{
    public IKTargetOffset head;
    public IKTargetOffset handL;
    public IKTargetOffset handR;
    public IKTargetOffset footL;
    public IKTargetOffset footR;
    public IKTargetOffset waist;
    public IKTargetOffset elbowL;
    public IKTargetOffset elbowR;
}

[Serializable]
public struct IKTargetOffset
{
    public Vector3 pos;
    public Vector3 rot;
}
