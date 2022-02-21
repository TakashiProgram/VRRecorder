using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAvatarButtons : MonoBehaviour
{
    [SerializeField] private Button[] m_Buttons;

    void Start()
    {
        DoActiveButton(0, true);
    }

    public void DeactiveAll()
    {
        for (int i = 0; i < m_Buttons.Length; ++i)
        {
            DoActiveButton(i, false);
        }
    }

    public void ActiveButton( int index )
    {
        DoActiveButton(index, true);
    }

    private void DoActiveButton(int index, bool is_pressed)
    {
        if ((null == m_Buttons) ||
            (null == m_Buttons[index]))
        {
            return;
        }

        m_Buttons[index].interactable = !is_pressed;
    }
}
