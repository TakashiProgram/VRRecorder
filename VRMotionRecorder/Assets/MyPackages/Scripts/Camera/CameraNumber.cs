using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraNumber : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private Sprite[] m_Numbers;

    [SerializeField]
    private Image[] m_Images;

    [SerializeField]
    private Texture2D m_CursorTexture;

    [SerializeField]
    private int size;

    private static readonly string CAMERA_NUM = "CAMERA_NUM";
    void Start()
    {
        int num = int.Parse(monobitView.owner.customParameters[CAMERA_NUM].ToString());
        for (int i = 0; i < m_Images.Length; i++)
        {
            m_Images[i].sprite = m_Numbers[num];
        }
        Cursor.SetCursor(m_CursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
}
