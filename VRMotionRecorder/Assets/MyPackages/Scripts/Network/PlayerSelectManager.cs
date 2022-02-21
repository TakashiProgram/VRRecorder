using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectManager : MonoBehaviour
{
    public Action<SelectedPlayerInfo> OnPlayerSelected;
    public Action OnCancelPlayerSelect;

    [Header("Players")]
    [SerializeField] private PlayerTypeInfo[] m_PlayerMap;
    [SerializeField] private PlayerType m_AutoJoinPlayerType = PlayerType.VR_PLAYER;

    [Header("Components")]
    [SerializeField] private GameObject m_PlayerSelectCanvas = null;
    [SerializeField] private Text m_AvatarName = null;
    [SerializeField] private RawImage m_AvatarImage = null;

    [SerializeField]
    private GameObject m_ButtonPrefab;

    [SerializeField]
    private GameObject m_PlayerContent;

    [SerializeField]
    private GameObject m_DressContent;

    [SerializeField]
    private AvatarNameMap m_AvatarNameMap;

    [SerializeField]
    private AvatarMap[] m_AvatarMap;

    [SerializeField]
    private AvatarMap[] m_DressMap;

    private int m_TypeNum;

    private int m_CurrentPlayerTypeIndex = 0;
    private int m_CurrentAvatarIndex = 0;
    [SerializeField]
    private InputField m_CameraNum;

    [SerializeField]
    private Image m_ChoicePlayerImage;

    [SerializeField]
    private Image m_ChoiceDreesImage;

    private Vector3 m_SelectPos = new Vector3(-180, 0, 0);
    void Start()
    {
        m_PlayerSelectCanvas.SetActive(false);
    }

    public void StartSelect()
    {
        

        OnSelectPlayerType(0);
        m_PlayerSelectCanvas.SetActive(true);
        ButtonGenerate();

    }

    //ボタン処理
    public void OnSelectPlayerType(int index)
    {
        m_CurrentPlayerTypeIndex = index;
        m_CurrentAvatarIndex = 0;
        m_ChoiceDreesImage.transform.parent = null;
        Debug.Log("通っている");
        int num = int.Parse(m_CameraNum.text);
        
        if (index != 1)
        {
            m_CameraNum.gameObject.SetActive(false);
        }
        else
        {
            m_CameraNum.gameObject.SetActive(true);
        }
        
        int child_count = m_DressContent.transform.childCount;
        if (child_count != 0)
        {
            foreach (Transform n in m_DressContent.transform)
            {
                Destroy(n.gameObject);
            }
        }
        m_AvatarImage.texture = null;
        DisplayCurrentAvatar();
        for (int i = 0; i < m_PlayerMap.Length; ++i)
        {
            m_PlayerMap[i].button.interactable = (i != index);
        }
    }

    public void OnBack()
    {
        if (null != OnCancelPlayerSelect)
            OnCancelPlayerSelect();

        m_PlayerSelectCanvas.SetActive(false);
    }

    public void OnEnter()
    {
        var args = new SelectedPlayerInfo();
        var current_player = m_PlayerMap[m_CurrentPlayerTypeIndex];

        args.prefabName = current_player.prefab.name;
        args.playerType = (int)current_player.type;
        args.avatarID = m_CurrentAvatarIndex;
        args.cameraNum = int.Parse(m_CameraNum.text);
        args.playerNum = m_TypeNum;

        if (null != OnPlayerSelected)
            OnPlayerSelected(args);

        m_PlayerSelectCanvas.SetActive(false);
    }

    private void DisplayCurrentAvatar()
    {
        var avatar_map = m_PlayerMap[m_CurrentPlayerTypeIndex].avatarMap;

        if ((null == avatar_map) ||
            (0 >= avatar_map.m_Avatars.Count))
        {
            return;
        }
        ButtonGenerate();
    }

    private void ButtonGenerate()
    {
        int child_count = m_PlayerContent.transform.childCount;
        m_ChoicePlayerImage.transform.parent = null;
        if (child_count != 0)
        {
            foreach (Transform n in m_PlayerContent.transform)
            {
                Destroy(n.gameObject);
            }
        }

        if (m_CurrentPlayerTypeIndex == 0)
        {
            var avatar_map = m_AvatarNameMap;
            GameObject start_button = null;
            int start_num = -1;
            for (int i = 0; i < avatar_map.m_AvatarNames.Count; i++)
            {
                var obj = Instantiate(m_ButtonPrefab);
                obj.transform.parent = m_PlayerContent.transform;
                obj.transform.localScale = Vector3.one;

                var button = obj.GetComponent<Button>();
                int num = i;
                button.onClick.AddListener(() => ClickDreesButton(num, obj));
                if (start_button == null)
                {
                    start_button = obj;
                    start_num = i;
                }

                var text = obj.GetComponentInChildren<Text>();
                text.text = avatar_map.m_AvatarNames[i].name;
            }

            ClickDreesButton(start_num, start_button);
        }
        else
        {
            var avatar_map = m_AvatarMap[m_CurrentPlayerTypeIndex - 1];
            GameObject start_button = null;
            int start_num = -1;

            for (int i = 0; i < avatar_map.m_Avatars.Count; i++)
            {
                var obj = Instantiate(m_ButtonPrefab);
                obj.transform.parent = m_PlayerContent.transform;
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
                text.text = avatar_map.m_Avatars[i].name;
            }
            ClickButton(start_num, start_button);
        }
    }

    private void ClickDreesButton(int num, GameObject button)
    {
        m_TypeNum = num;

        m_ChoicePlayerImage.transform.parent = button.transform;
        m_ChoicePlayerImage.transform.localPosition = m_SelectPos;
        DressButtonGenerate(num);
    }

    private void ClickButton(int num, GameObject button)
    {
        m_ChoicePlayerImage.transform.parent = button.transform;
        m_ChoicePlayerImage.transform.localPosition = m_SelectPos;
        SetPlayerUI(num);
    }

    private void DressButtonGenerate(int dress_count)
    {
        int child_count = m_DressContent.transform.childCount;
        m_ChoiceDreesImage.transform.parent = null;
        if (child_count != 0)
        {
            foreach (Transform n in m_DressContent.transform)
            {
                Destroy(n.gameObject);
            }
        }
        var dress_map = m_DressMap;

        GameObject start_button = null;
        int start_num = -1;
        int start_dress_count = -1;
        for (int i = 0; i < dress_map[dress_count].m_Avatars.Count; i++)
        {
            var obj = Instantiate(m_ButtonPrefab);
            obj.transform.parent = m_DressContent.transform;
            obj.transform.localScale = Vector3.one;

            var button = obj.GetComponent<Button>();
            var num = i;
            button.onClick.AddListener(() => SetDressUI(dress_count, num, obj));


            if (start_button == null)
            {
                start_button = obj;
                start_num = i;
                start_dress_count = dress_count;
            }

            var text = obj.GetComponentInChildren<Text>();
            text.text = dress_map[dress_count].m_Avatars[i].name;
        }

        SetDressUI(start_dress_count, start_num, start_button);
    }

    private void SetDressUI(int dress_count, int num, GameObject button)
    {
        m_CurrentAvatarIndex = num;
        m_ChoiceDreesImage.transform.parent = button.transform;
        m_ChoiceDreesImage.transform.localPosition = m_SelectPos;
        var image = m_DressMap[dress_count].m_Avatars[num].image;
        if (null == image)
        {

            m_AvatarImage.texture = null;
        }
        else
        {

            m_AvatarImage.texture = image;
        }
    }

    private void SetPlayerUI(int num)
    {
        m_CurrentAvatarIndex = num;
        var image = m_AvatarMap[m_CurrentPlayerTypeIndex - 1].m_Avatars[num].image;
        if (null == image)
        {

            m_AvatarImage.texture = null;
        }
        else
        {

            m_AvatarImage.texture = image;
        }
    }

    [Serializable]
    private struct PlayerTypeInfo
    {
        public PlayerType type;
        public GameObject prefab;
        public Button button;
        public AvatarMap avatarMap;
    }
}

public struct SelectedPlayerInfo
{
    public string prefabName;
    public int playerType;
    public int avatarID;
    public int cameraNum;
    public int playerNum;
}

public enum PlayerType : int
{
    VR_PLAYER = 0,
    FULL_BODY_VR_PLAYER,
    DESKTOP_PLAYER,
    CAMERA,
    SPECTATOR,
    TABLET_PLAYER,
    AIKATSU_SPIRIT
}
