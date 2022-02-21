using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialModulesController : MonoBehaviour
{

    private void Awake()
    {
        var settings = GetComponentInParent<AvatarSettings>();

        if (null != settings)
        {
            settings.OnOwnerChanged += OnOwnerChanged;
        }
    }

    private void OnOwnerChanged(bool is_owner)
    {
        if (is_owner == gameObject.activeSelf)
        {
            return;
        }

        //再帰できないため暫定
        gameObject.SetActive(is_owner);
    }
}
