using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

public class GirlsDynamicBoneObjectReorderUI
{
    /// <summary>
    /// プロパティフィールドの高さ
    /// </summary>
    private const int PropertyFieldHeight = 16;

    /// <summary>
    /// 要素の高さ（プロパティフィールドの高さ＋マージン分）
    /// </summary>
    private const int ElementHeight = PropertyFieldHeight + 4;

    /// <summary>
    /// シリアライズオブジェクト
    /// </summary>
    private SerializedObject serializedObject;

    /// <summary>
    /// 入れ替え可能リスト
    /// </summary>
    private ReorderableList reorderableList;

    /// <summary>
    /// 再描画メソッド
    /// </summary>
    public UnityAction RepaintFunction
    {
        get;
        set;
    }

    /// <summary>
    /// セットアップ
    /// </summary>
    /// <param name="target">ターゲット</param>
    public void Setup(GirlsDynamicBone target)
    {
        this.serializedObject = new SerializedObject(target);

        var prop = this.serializedObject.FindProperty("boneObjects");

        this.reorderableList = new ReorderableList(this.serializedObject, prop);

        this.reorderableList.elementHeight = ElementHeight;

        this.reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Reorder Mode");
        };

        this.reorderableList.onAddCallback = (list) =>
        {
            prop.arraySize++;
            list.index = prop.arraySize - 1;

            this.RepaintFunction?.Invoke();
        };

        this.reorderableList.onRemoveCallback = (list) =>
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(list);

            this.RepaintFunction?.Invoke();
        };

        this.reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = this.reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            if (element == null)
            {
                return;
            }

            rect.y += 2;
            rect.height -= 4;

            string name = "None";

            var transform = element.FindPropertyRelative("root").objectReferenceValue as Transform;

            if (transform != null)
            {
                name = transform.name;
            }

            EditorGUI.LabelField(new Rect(rect.x + 4, rect.y, rect.width - 4, PropertyFieldHeight), name);
        };
    }

    /// <summary>
    /// UI表示
    /// </summary>
    public void OnGUI()
    {
        if (this.serializedObject == null)
        {
            return;
        }

        this.serializedObject.Update();

        this.reorderableList.DoLayoutList();

        this.serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }
}
