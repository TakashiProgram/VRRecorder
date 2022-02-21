using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    public Action<SelectedStageInfo> OnStageSelected;
    public Action OnCancelStageSelect;

    [Header("Components")]
    [SerializeField] private StageMap m_StageMap;
    [SerializeField] private GameObject m_StageSelectCanvas = null;
    [SerializeField] private Text m_StageName = null;
    [SerializeField] private RawImage m_StageImage = null;

    [SerializeField]
    private GameObject m_ButtonPrefab;

    [SerializeField]
    private GameObject m_Content;

    private int m_CurrentStageIndex = 0;
    private bool m_IsSelected = false;
    [SerializeField]
    private Image m_ChoiceStageImage;

    private Vector3 m_SelectPos = new Vector3(-180, 0, 0);

    void Start()
    {
        m_StageSelectCanvas.SetActive(false);
    }

    public void StartSelect()
    {
        m_CurrentStageIndex = 0;
        ButtonGenerate();
        m_StageSelectCanvas.SetActive(true);
    }

    public void OnBack()
    {
        if (null != OnCancelStageSelect)
            OnCancelStageSelect();

        m_StageSelectCanvas.SetActive(false);
        m_IsSelected = false;
    }

    public void OnEnter()
    {
        
        var args = new SelectedStageInfo();
        args.stagePath = m_StageMap.m_Stages[m_CurrentStageIndex].stagePath;
        
        if (null != OnStageSelected)
            OnStageSelected(args);
       
        m_StageSelectCanvas.SetActive(false);
        m_IsSelected = true;
    }

    public bool IsSelected()
    {
        return m_IsSelected;
    }

    private void ButtonGenerate()
    {
        GameObject start_button = null;
        int start_num = -1;
        for (int i = 0; i < m_StageMap.m_Stages.Count; i++)
        {
            var obj = Instantiate(m_ButtonPrefab);
            obj.transform.parent = m_Content.transform;
            obj.transform.localScale = Vector3.one;

            var button = obj.GetComponent<Button>();
            int num = i;
            button.onClick.AddListener(() => ClickButton(num, obj));

            if (start_button == null)
            {
                start_button = obj;
                start_num = i;
            }

            var text = obj.GetComponentInChildren<Text>();
            text.text = m_StageMap.m_Stages[i].name;
        }
        ClickButton(start_num, start_button);
        SetStageUI();
    }

    public void ClickButton(int num, GameObject button)
    {
        m_CurrentStageIndex = num;
        m_ChoiceStageImage.transform.parent = button.transform;
        m_ChoiceStageImage.transform.localPosition = m_SelectPos;
        SetStageUI();
    }

    private void SetStageUI()
    {
        var stage = m_StageMap.m_Stages[m_CurrentStageIndex];
        
        if (null == stage.image)
        {

            m_StageImage.texture = null;
        }
        else
        {
            m_StageImage.texture = stage.image;
        }
        m_StageName.text = stage.name;
    }
}

public struct SelectedStageInfo
{
    public string stagePath;
}
