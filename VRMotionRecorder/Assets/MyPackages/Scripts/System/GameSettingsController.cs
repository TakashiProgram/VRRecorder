using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsController : MonoBehaviour
{
    public enum Stage : int
    {
        UNKNOWN = 0,
        SAMPLE
    }

    [SerializeField] private SceneLoader m_Loader = null;
    [SerializeField] private Stage m_Stage;
    
    void Start()
    {
        if( null != m_Loader )
        {
            m_Loader.LoadStageScene( m_Stage );
        }
    }
}
