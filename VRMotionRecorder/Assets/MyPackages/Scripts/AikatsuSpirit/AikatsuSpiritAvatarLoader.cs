using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AikatsuSpiritAvatarLoader : MonoBehaviour
{
    [SerializeField] private AvatarMap m_Map;


    public void Load(int id)
    {
        if (null == m_Map)
        {
            return;
        }

        GameObject avatar = Instantiate(m_Map.m_Avatars[id].prefab, transform);
    }

    public void Unload()
    {
        while ( 0 < transform.childCount )
        {
            Transform child = transform.GetChild(0);

            child.parent = null;

            GameObject.Destroy(child.gameObject);

        }
    }

    public int GetAvatarCount()
    {
        if (null == m_Map)
        {
            return 0;
        }

        return m_Map.m_Avatars.Count;
    }
}
