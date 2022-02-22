using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GirlsDynamicBone))]
public class GirlsDynamicBoneInspector : UnityEditor.Editor
{
    /// <summary>
    /// 表示モード
    /// </summary>
    private enum DrawMode
    {
        /// <summary>
        /// 編集モード
        /// </summary>
        Edit,

        /// <summary>
        /// 入れ替え、追加、削除モード
        /// </summary>
        Reorder,

        /// <summary>
        /// Unityのデフォルト表示
        /// </summary>
        UnityDefault,
    }

    /// <summary>
    /// 表示モード
    /// </summary>
    private DrawMode drawMode = DrawMode.Edit;

    /// <summary>
    /// 表示UIと折りたたみ情報
    /// </summary>
    private List<ObjectUIState> objectUIStateList = new List<ObjectUIState>();

    /// <summary>
    /// 入れ替え、追加、削除モードのUI
    /// </summary>
    private GirlsDynamicBoneObjectReorderUI reorderUI = new GirlsDynamicBoneObjectReorderUI();

    /// <summary>
    /// 有効化時処理
    /// </summary>
    public void OnEnable()
    {
        this.Setup();
    }

    /// <summary>
    /// 無効化時処理
    /// </summary>
    public void OnDisable()
    {
        this.objectUIStateList.Clear();
    }

    /// <summary>
    /// インスペクター表示
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawMode mode = (DrawMode)GUILayout.Toolbar((int)this.drawMode, new string[] { "Edit", "Reorder", "UnityDefault" }, EditorStyles.toolbarButton);

        if (mode != this.drawMode)
        {
            this.drawMode = mode;
            this.Setup();
        }

        switch (this.drawMode)
        {
            case DrawMode.Edit:
                this.DrawEditUI();
                break;
            case DrawMode.Reorder:
                this.DrawReorderUI();
                break;
            case DrawMode.UnityDefault:
                base.OnInspectorGUI();
                break;
        }
    }

    /// <summary>
    /// セットアップ
    /// </summary>
    private void Setup()
    {
        this.objectUIStateList.Clear();

        var bone = target as GirlsDynamicBone;

        foreach (var obj in bone.BoneObjects)
        {
            this.objectUIStateList.Add(new ObjectUIState(obj));
        }

        this.reorderUI.Setup(target as GirlsDynamicBone);
    }

    /// <summary>
    /// 入れ替え、追加、削除モード時表示
    /// </summary>
    private void DrawReorderUI()
    {
        this.reorderUI.OnGUI();
    }

    /// <summary>
    /// 編集モード
    /// </summary>
    private void DrawEditUI()
    {
        var bone = target as GirlsDynamicBone;
        var objects = bone.BoneObjects;

        for (int i = 0; i < this.objectUIStateList.Count; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            this.objectUIStateList[i].Foldout = EditorGUILayout.Foldout(this.objectUIStateList[i].Foldout, objects[i].Root != null ? objects[i].Root.name : "None");
            if (this.objectUIStateList[i].Foldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(GUI.skin.box);
                this.objectUIStateList[i].ObjectUI.DrawUI();
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add"))
        {
            bone.AddObject();
            this.Setup();
        }

        if (GUILayout.Button("Remove"))
        {
            bone.RemoveObject();
            this.Setup();
        }

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// UIの状態
    /// </summary>
    public class ObjectUIState
    {
        /// <summary>
        /// 折りたたみ情報
        /// </summary>
        public bool Foldout
        {
            get;
            set;
        } = false;

        /// <summary>
        /// UI表示クラス
        /// </summary>
        public GirlsDynamicBoneObjectUI ObjectUI
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">ターゲット</param>
        public ObjectUIState(GirlsDynamicBoneObject target)
        {
            this.Foldout = false;
            this.ObjectUI = new GirlsDynamicBoneObjectUI(target);
        }
    }
}
