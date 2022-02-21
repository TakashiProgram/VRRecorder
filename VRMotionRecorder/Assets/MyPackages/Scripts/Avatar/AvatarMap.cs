using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AvatarMap")]
public class AvatarMap : ScriptableObject
{
    [Serializable]
    public struct AvatarPair
    {
        public string name;
        public Texture2D image;
        public GameObject prefab;
    }

    public List<AvatarPair> m_Avatars;
}
