using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MonobitDelayDestroy : MonobitEngine.MonoBehaviour
{
    [SerializeField] private float m_Delay = 10f;

    void Start()
    {
        if ( false == monobitView.isOwner )
        {
            return;
        }

        StartCoroutine(DelayDestroy(m_Delay));
    }

    private IEnumerator DelayDestroy(float time)
    {
        yield return new WaitForSeconds(time);

        if (null != this)
        {
            Destroy(gameObject);
        }
    }
}
