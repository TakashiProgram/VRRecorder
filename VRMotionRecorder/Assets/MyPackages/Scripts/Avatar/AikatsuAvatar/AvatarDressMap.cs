using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/AvatarDressMap")]
public class AvatarDressMap : ScriptableObject
{
    [Serializable]
    public struct AvatarDressPair
    {
        public string name;
        public Texture2D image;
        public GameObject prefab;
    }
    public List<AvatarDressPair> m_AvatarDress;
}
