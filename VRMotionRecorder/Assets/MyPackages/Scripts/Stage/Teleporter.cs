using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform m_Dst;

    private static readonly string PLAYER = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (null == m_Dst)
        {
            return;
        }

        GameObject parent = other.transform.root.gameObject;

        if (false == parent.CompareTag(PLAYER))
        {
            return;
        }

        parent.transform.position = m_Dst.position;

        //var v_motor = parent.gameObject.GetComponent<Invector.vCharacterController.vThirdPersonController>();
        //if (null != v_motor)
        //{
            
        //}
    }
}
