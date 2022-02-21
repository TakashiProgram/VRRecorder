using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAvatarLoader : MonoBehaviour
{
    [SerializeField] private AvatarMap m_Map;
    [SerializeField] private PlaybackCamera m_PlaybackCamera;
    [SerializeField] private Transform m_CameraChildren;

    public void Load(int id)
    {
        if (null == m_Map)
        {
            return;
        }

        GameObject avatar = Instantiate(m_Map.m_Avatars[id].prefab, transform);

        var settings = avatar.GetComponentInChildren<CameraSettings>();
        if (null != settings)
        {
            var cam = settings.GetMainCamera();
            if ( null != cam )
            {
                if (null != m_PlaybackCamera)
                {
                    m_PlaybackCamera.SetMainCamera(cam);
                }

                if (null != m_CameraChildren)
                {
                    m_CameraChildren.SetParent(cam.transform);
                    m_CameraChildren.localPosition = Vector3.zero;
                    m_CameraChildren.localRotation = Quaternion.identity;
                    m_CameraChildren.localScale = Vector3.one;
                }

            }
        }
    }

    public void Unload()
    {
        while (0 < transform.childCount)
        {
            Transform child = transform.GetChild(0);

            child.parent = null;

            GameObject.Destroy(child.gameObject);

        }
    }
}
