using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonobitEngine;

public class MonobitNetworkConnectManager : MonobitEngine.MonoBehaviour
{
    [Header("Network")]
    [SerializeField] private MonobitEngine.ServerSettings m_MonobitServerSettings;
    [SerializeField] private InputField m_LocalHostInputField = null;
    //[SerializeField] private bool m_IsOfflineMode = false;
    [SerializeField] private int m_SendRate = 10; //初期値20
    [SerializeField] private int m_UpdateStreamRate = 30; //初期値10

    [Header("Components")]
    [SerializeField] private GameObject m_Camera = null;
    [SerializeField] private PlayerSelectManager m_PlayerSelectManager;
    [SerializeField] private StageSelectManager m_StageSelectManager;
    [SerializeField] private SceneLoader m_SceneLoader;

    [SerializeField] private GameObject m_ServerCanvas = null;
    [SerializeField] private GameObject m_LobbyCanvas = null;
    [SerializeField] private InputField m_NewRoomName = null;
    [SerializeField] private MunRoomPanel m_RoomPanel = null;
    [SerializeField] private Transform m_RoomPanelParent = null;

    private Dictionary<string, MunRoomPanel> m_RoomPanels = new Dictionary<string, MunRoomPanel>();

    private bool m_IsCustomParameterChanged;
    private string m_RoomName = "";

    private SelectedPlayerInfo m_SelectedPlayerInfo;
    private SelectedStageInfo m_SelectedStageInfo;

    private NetworkSettings m_Settings;
    private static readonly string SETTINGS_PATH = "NetworkSettings.json";
    private static readonly string DEBUG_MODE_KEY = "DebugMode";
    private static readonly string DRESS2D_KEY = "Dress2D";
    private static readonly string SURVEILLANCE_KEY = "Surveillance";

    //Monobit Room/Player Custom Parameters
    private static readonly string STAGE = "STAGE";
    private static readonly string IS_DEBUG_MODE = "IS_DEBUG_MODE";
    private static readonly string IS_SURVEILLANCE = "IS_SURVEILLANCE";
    private static readonly string IS_DRESS2D = "IS_DRESS2D";
    private static readonly string OFFSET_POS = "OFFSET_POS";
    private static readonly string OFFSET_ROT = "OFFSET_ROT";
    private static readonly string PLAYER_TYPE = "PLAYER_TYPE";
    private static readonly string AVATAR_ID = "AVATAR_ID";


    void Start()
    {
        MonobitEngine.MonobitNetwork.sendRate = m_SendRate;
        MonobitEngine.MonobitNetwork.updateStreamRate = m_UpdateStreamRate;

        m_Settings = JsonHelper<NetworkSettings>.Read(SETTINGS_PATH);
        Debug.Log(m_LocalHostInputField);
        m_LocalHostInputField.text = m_Settings.localHost;

        //if (m_IsAutoJoinRoom)
        //{
        //    m_PlayerType = m_AutoJoinPlayerType;
        //    ConnectServer();
        //    return;
        //}

        m_ServerCanvas.SetActive(true);
        m_LobbyCanvas.SetActive(false);

        MouseCursorSettings.Instance.UnHide();
    }

    void Update()
    {
        if( false == MonobitNetwork.inRoom)
        {
            return;
        }

        if (Input.GetButtonDown(DEBUG_MODE_KEY))
        {
            Hashtable customParams = MonobitNetwork.room.customParameters;

            if (false == customParams.ContainsKey(IS_DEBUG_MODE))
            {
                return;
            }
            var is_debug_mode = (bool)customParams[IS_DEBUG_MODE];

            customParams[IS_DEBUG_MODE] = !is_debug_mode;
            MonobitNetwork.room.SetCustomParameters(customParams);
        }



        if (Input.GetButtonDown(DRESS2D_KEY))
        {
            Hashtable customParams = MonobitNetwork.room.customParameters;

            if (false == customParams.ContainsKey(IS_DRESS2D))
            {
                return;
            }
            var is_surveillance = (bool)customParams[IS_DRESS2D];

            customParams[IS_DRESS2D] = !is_surveillance;
            MonobitNetwork.room.SetCustomParameters(customParams);
        }

        if (Input.GetButtonDown(SURVEILLANCE_KEY))
        {

            Hashtable customParams = MonobitNetwork.room.customParameters;

            if (false == customParams.ContainsKey(IS_SURVEILLANCE))
            {
                return;
            }

            var is_surveillance = (bool)customParams[IS_SURVEILLANCE];

            customParams[IS_SURVEILLANCE] = !is_surveillance;
            MonobitNetwork.room.SetCustomParameters(customParams);
        }


    }

    void OnEnable()
    {
        if (null != m_PlayerSelectManager)
        {
            m_PlayerSelectManager.OnPlayerSelected += OnPlayerSelected;
            m_PlayerSelectManager.OnCancelPlayerSelect += OnCancelPlayerSelect;
        }

        if (null != m_StageSelectManager)
        {
            m_StageSelectManager.OnStageSelected += OnStageSelected;
            m_StageSelectManager.OnCancelStageSelect += OnCancelStageSelect;
        }
    }

    void OnDisable()
    {
        if (null != m_PlayerSelectManager)
        {
            m_PlayerSelectManager.OnPlayerSelected -= OnPlayerSelected;
            m_PlayerSelectManager.OnCancelPlayerSelect -= OnCancelPlayerSelect;
        }

        if (null != m_StageSelectManager)
        {
            m_StageSelectManager.OnStageSelected -= OnStageSelected;
            m_StageSelectManager.OnCancelStageSelect -= OnCancelStageSelect;
        }
    }

    void OnDestroy()
    {
        if (null != m_MonobitServerSettings)
            m_MonobitServerSettings.HostType = ServerSettings.MunHostingOption.MunTestServer;
    }



    public void ConnectMunTestServer()
    {
        if (null == m_MonobitServerSettings) return;

        m_MonobitServerSettings.HostType = ServerSettings.MunHostingOption.MunTestServer;

        ConnectServer();
    }

    public void ConnectSelfServer()
    {
        if (null == m_MonobitServerSettings) return;

        m_MonobitServerSettings.HostType = ServerSettings.MunHostingOption.SelfServer;
        m_MonobitServerSettings.SelfServerAddress = m_LocalHostInputField.text;
        m_MonobitServerSettings.SelfServerPort = m_Settings.port;

        ConnectServer();
    }

    private void ConnectServer()
    {
        MonobitNetwork.autoJoinLobby = false;
        MonobitNetwork.ConnectServer(m_Settings.serverName);

        m_ServerCanvas.SetActive(false);
    }

    void OnConnectedToMonobit()
    {
        Debug.Log("OnConnectedToMonobit");
        MonobitNetwork.JoinLobby();
    }

    void OnConnectToServerFailed()
    {
        Debug.Log("OnConnectToServerFailed");
        m_ServerCanvas.SetActive(true);
    }

    void OnConnectionFail()
    {
        Debug.Log("OnConnectionFail");
        m_ServerCanvas.SetActive(true);
        m_LobbyCanvas.SetActive(false);
    }

    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");

        //SelfServerに接続成功した場合はlocalHostを保存する
        if (ServerSettings.MunHostingOption.SelfServer == m_MonobitServerSettings.HostType)
        {
            m_Settings.localHost = m_LocalHostInputField.text;
            JsonHelper<NetworkSettings>.Write(SETTINGS_PATH, m_Settings);
        }

        //if (m_IsAutoJoinRoom)
        //{
        //    CreateAndJoinRoom();
        //    return;
        //}

        GoLobby();
    }

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");

        Hashtable customParams = new Hashtable();
        customParams[PLAYER_TYPE] = m_SelectedPlayerInfo.playerType;
        customParams[AVATAR_ID] = m_SelectedPlayerInfo.avatarID;
        MonobitEngine.MonobitNetwork.SetPlayerCustomParameters(customParams);

        StartCoroutine(LoadStage());
        StartCoroutine(JoinPlayer());
    }

    void OnMonobitPlayerParametersChanged(object[] playerAndUpdatedParams)
    {
        Debug.Log("OnMonobitPlayerParametersChanged");

        m_IsCustomParameterChanged = true;
    }

    void OnJoinRoomFailed()
    {
        Debug.Log("OnJoinRoomFailed");

        GoLobby();
    }

    //TODO:なぜか呼ばれないため、ボタンで更新に変更した
    void OnReceivedRoomListUpdate()
    {
        Debug.Log("OnReceivedRoomListUpdate");

        UpdateRoomList();
    }

    public void ReloadRoomList()
    {
        UpdateRoomList();
    }

    private void UpdateRoomList()
    {
        RoomData[] datas = MonobitNetwork.GetRoomData();
        List<string> room_names = new List<string>();

        for (int i = 0; i < datas.Length; ++i)
        {
            string room_name = datas[i].name;
            room_names.Add(room_name);

            if (false == m_RoomPanels.ContainsKey(room_name))
            {
                MunRoomPanel panel = Instantiate(m_RoomPanel, m_RoomPanelParent);
                panel.Init(room_name);
                panel.OnJoinRoom += JoinRoom;

                m_RoomPanels.Add(room_name, panel);
            }

            Text text = m_RoomPanels[room_name].GetConnectNumText();
            text.text = datas[i].playerCount.ToString();
        }

        foreach(string key in m_RoomPanels.Keys )
        {
            if ( false == room_names.Contains(key) )
            {
                Destroy(m_RoomPanels[key].gameObject);
            }
        }
    }

    //ルームリスト内のルーム参加ボタンを押した際のイベント処理
    private void JoinRoom(string room_name)
    {
        m_RoomName = room_name;
        GoPlayerSelect();
    }

    //ルーム作成ボタンを押した際のイベント処理（同名のルームがあれば既存のルームに参加する）
    public void CreateAndJoinRoom()
    {
        string room_name = "";

        if (null != m_NewRoomName)
        {
            room_name = m_NewRoomName.text;
        }

        if ("" == room_name)
        {
            return;
        }

        m_RoomName = room_name;

        if (IsNewRoom(room_name))
        {
            GoStageSelect();
        }
        else
        {
            GoPlayerSelect();
        }
        
    }

    private IEnumerator JoinPlayer()
    {
        //カスタムパラメータが反映されるまでのラグ待ち
        while(false == m_IsCustomParameterChanged)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);


        var spawn_pos = Vector3.zero;

        //var points = FindObjectsOfType<SpawnPos>();
        //if (0 < points.Length)
        //{
        //    int random = UnityEngine.Random.Range(0, points.Length);
        //    spawn_pos = points[random].transform.position;
        //}

        MonobitNetwork.Instantiate(m_SelectedPlayerInfo.prefabName, spawn_pos, Quaternion.identity, 0);

        if (null != m_Camera)
        {
            m_Camera.SetActive(false);
        }

        MouseCursorSettings.Instance.Hide();
    }

    private IEnumerator LoadStage()
    {
        while (false == MonobitNetwork.room.customParameters.ContainsKey(STAGE))
        {
            yield return null;
        }

        if (null != m_SceneLoader)
        {
            m_SceneLoader.LoadStageScene((string)MonobitNetwork.room.customParameters[STAGE]);
        }
    }

    private void GoLobby()
    {
        m_LobbyCanvas.SetActive(true);

        UpdateRoomList();
    }

    private void GoPlayerSelect()
    {
        m_LobbyCanvas.SetActive(false);
       
        if (null != m_PlayerSelectManager)
        {
            
            m_PlayerSelectManager.StartSelect();

        }
    }

    private void OnPlayerSelected(SelectedPlayerInfo args)
    {
        m_SelectedPlayerInfo = args;

        if (IsNewRoom(m_RoomName))
        {
            //ステージ選択スキップ後に、参加するルームがなくなった場合の対応
            if (false == m_StageSelectManager.IsSelected())
            {
                m_PlayerSelectManager.OnCancelPlayerSelect();
                return;
            }

            var room_settings = new RoomSettings();
            Hashtable customParams = new Hashtable();
            customParams[STAGE] = m_SelectedStageInfo.stagePath;
            customParams[IS_DEBUG_MODE] = true;
            customParams[IS_SURVEILLANCE] = false;
            customParams[IS_DRESS2D] = false;
            customParams[OFFSET_POS] = Vector3.zero.ToString(); //vector3 to string
            customParams[OFFSET_ROT] = Vector3.zero.ToString(); //vector3 to string
            room_settings.roomParameters = customParams;

            MonobitNetwork.JoinOrCreateRoom(m_RoomName, room_settings, LobbyInfo.Default);
        }
        else
        {
            MonobitNetwork.JoinRoom(m_RoomName);
        }
    }

    private void OnCancelPlayerSelect()
    {
        GoLobby();
    }

    private void GoStageSelect()
    {
        m_LobbyCanvas.SetActive(false);

        if (null != m_StageSelectManager)
        {
            m_StageSelectManager.StartSelect();
        }
    }

    private void OnStageSelected(SelectedStageInfo args)
    {
        
        m_SelectedStageInfo = args;
        
        GoPlayerSelect();
    }

    private void OnCancelStageSelect()
    {
        GoLobby();
    }

    private bool IsNewRoom(string room_name)
    {
        var rooms = MonobitEngine.MonobitNetwork.GetRoomData();
        if ((null == rooms)||
            (0 >= rooms.Length))
        {
            return true;
        }

        foreach (RoomData room in rooms)
        {
            if (true == string.Equals( room_name, room.name, System.StringComparison.CurrentCulture ))
            {
                return false;
            }
        }

        return true;
    }
}

[System.Serializable]
public struct NetworkSettings
{
    public string serverName;
    public int port;
    public string localHost;
}
