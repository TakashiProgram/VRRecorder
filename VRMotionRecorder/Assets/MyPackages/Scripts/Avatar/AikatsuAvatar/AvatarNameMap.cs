using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AvatarNameMap")]
public class AvatarNameMap : ScriptableObject
{
    [Serializable]
    public struct AvatarNamePair
    {
        public string name;
    }
    //[Serializable]
    //public struct AvatarDressPair
    //{
    //    public string name;
    //}
    public List<AvatarNamePair> m_AvatarNames;
  //  public List<AvatarDressPair> m_AvatarDresss;
}
