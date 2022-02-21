using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    

    [System.Serializable]
    public class StagePathPair : Serialize.KeyAndValue<GameSettingsController.Stage, string>
    {
        public StagePathPair(GameSettingsController.Stage key, string value) : base( key, value ) { }
    }

    /// <summary>
    /// UnityのInspectorで、Dictionary形式による設定を行うためのクラス
    /// </summary>
    [System.Serializable]
    public class StagePathMap : Serialize.TableBase<GameSettingsController.Stage, string, StagePathPair>
    {
    }

    [SerializeField] private string m_CoreScenePath = "";
    [SerializeField] private StagePathMap m_StagePathMap = null;

    public void LoadStageScene(GameSettingsController.Stage stage)
    {
        Dictionary<GameSettingsController.Stage, string> pair = m_StagePathMap.GetTable();
        string path = pair[stage];

        StartCoroutine( LoadAsync( path, LoadSceneMode.Additive, true ) );
    }

    public void LoadStageScene(string path)
    {
        StartCoroutine(LoadAsync(path, LoadSceneMode.Additive, true));
    }

    //void Update()
    //{
    //    if( true == Input.GetKeyDown( KeyCode.Space ) )
    //    {
    //        LoadSync( m_CoreScenePath, LoadSceneMode.Single );
    //    }
    //}

    private void LoadSync(string path, LoadSceneMode mode)
    {
        SceneManager.LoadScene( path, mode );
    }

    private IEnumerator LoadAsync(string path, LoadSceneMode mode, bool is_active)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync( path, mode );

        while( !op.isDone )
        {
            yield return null;
        }

        SceneManager.SetActiveScene( SceneManager.GetSceneByPath( path ) );
    }

    private IEnumerator UnloadAsync(string path)
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync( path );

        while( !op.isDone )
        {
            yield return null;
        }
    }

    private IEnumerator LoadAsyncAndUnload(string load_path, LoadSceneMode mode, string unload_path, bool is_active)
    {
        yield return StartCoroutine( LoadAsync( load_path, mode, is_active ) );
        yield return StartCoroutine( UnloadAsync( unload_path ) );
    }
}
