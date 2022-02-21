using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StageMap")]
public class StageMap : ScriptableObject
{
    [Serializable]
    public struct StagePair
    {
        public string name;
        public Texture2D image;
        public string stagePath;
    }

    public List<StagePair> m_Stages;
}
