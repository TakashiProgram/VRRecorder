using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackSettingsLoader : MonoBehaviour
{
    [SerializeField] private BlinkController m_BlinkController = null;

    private EyeTrackSettings m_EyeTrackSettings;
    
    private static readonly string SETTINGS_PATH = "EyeTrackSettings.json";

    void Start()
    {
        Load();
    }

    public void Save()
    {
        JsonHelper<EyeTrackSettings>.Write(SETTINGS_PATH, m_EyeTrackSettings);
    }

    public void Load()
    {
        m_EyeTrackSettings = JsonHelper<EyeTrackSettings>.Read(SETTINGS_PATH);

        if (null != m_BlinkController)
        {
            m_BlinkController.Init(m_EyeTrackSettings.s_BlinkRate, m_EyeTrackSettings.s_IsOneEyedBlink);
        }
    }

    [System.Serializable]
    private struct EyeTrackSettings
    {
        public float s_BlinkRate;
        public bool s_IsOneEyedBlink;
    }
}
