using UnityEditor;
using UnityEngine;

/// <summary>
/// GirlsDynamicBoneObjectのUI表示
/// </summary>
public class GirlsDynamicBoneObjectUI
{
    /// <summary>
    /// ターゲットオブジェクト
    /// </summary>
    private GirlsDynamicBoneObject target;

    /// <summary>
    /// Collidersの折りたたみ
    /// </summary>
    private bool collidersFoldout;

    /// <summary>
    /// Exclusionsの折りたたみ
    /// </summary>
    private bool exclusionsFoldout;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="target">表示対象のオブジェクト</param>
    public GirlsDynamicBoneObjectUI(GirlsDynamicBoneObject target)
    {
        this.target = target;
    }

    /// <summary>
    /// UI表示
    /// </summary>
    public virtual void DrawUI()
    {
        this.target.Enabled = EditorGUILayout.BeginToggleGroup("enable", this.target.Enabled);

        this.target.Root = (Transform)EditorGUILayout.ObjectField("Transform", this.target.Root, typeof(Transform), true);

        this.target.UpdateRate = EditorGUILayout.FloatField("Update Rate", this.target.UpdateRate);

        this.target.UpdateModeType = (GirlsDynamicBoneObject.UpdateMode)EditorGUILayout.EnumPopup("Update Mode", this.target.UpdateModeType);

        EditorGUILayout.BeginVertical(GUI.skin.box);

        this.target.Damping = EditorGUILayout.Slider("Damping", this.target.Damping, 0.0f, 1.0f);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        this.target.DampingDistrib = EditorGUILayout.CurveField("Damping Distrib", this.target.DampingDistrib);
        //if (GUILayout.Button("Select Curve"))
        //{
        //    UniExt.Editor.Curve.CurveAssetSelector.OpenSelector((curve) => { this.target.DampingDistrib = curve; });
        //}

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        this.target.Elasticity = EditorGUILayout.Slider("Elasticity", this.target.Elasticity, 0.0f, 1.0f);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        this.target.ElasticityDistrib = EditorGUILayout.CurveField("Elasticity Distrib", this.target.ElasticityDistrib);
        //if (GUILayout.Button("Select Curve"))
        //{
        //    UniExt.Editor.Curve.CurveAssetSelector.OpenSelector((curve) => { this.target.ElasticityDistrib = curve; });
        //}

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        this.target.Stiffness = EditorGUILayout.Slider("Stiffness", this.target.Stiffness, 0.0f, 1.0f);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        this.target.StiffnessDistrib = EditorGUILayout.CurveField("Stiffness Distrib", this.target.StiffnessDistrib);
        //if (GUILayout.Button("Select Curve"))
        //{
        //    UniExt.Editor.Curve.CurveAssetSelector.OpenSelector((curve) => { this.target.StiffnessDistrib = curve; });
        //}

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        this.target.Inert = EditorGUILayout.Slider("Inert", this.target.Inert, 0.0f, 1.0f);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        this.target.InertDistrib = EditorGUILayout.CurveField("Inert Distrib", this.target.InertDistrib);
        //if (GUILayout.Button("Select Curve"))
        //{
        //    UniExt.Editor.Curve.CurveAssetSelector.OpenSelector((curve) => { this.target.InertDistrib = curve; });
        //}

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        this.target.Radius = EditorGUILayout.FloatField("Radius", this.target.Radius);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        this.target.RadiusDistrib = EditorGUILayout.CurveField("Radius Distrib", this.target.RadiusDistrib);
        //if (GUILayout.Button("Select Curve"))
        //{
        //    UniExt.Editor.Curve.CurveAssetSelector.OpenSelector((curve) => { this.target.RadiusDistrib = curve; });
        //}

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        this.target.EndLength = EditorGUILayout.FloatField("End Length", this.target.EndLength);

        this.target.EndOffset = EditorGUILayout.Vector3Field("End Offset", this.target.EndOffset);

        this.target.Gravity = EditorGUILayout.Vector3Field("Gravity", this.target.Gravity);

        this.target.Force = EditorGUILayout.Vector3Field("Force", this.target.Force);

        // Colliders
        this.collidersFoldout = EditorGUILayout.Foldout(this.collidersFoldout, "Colliders");

        if (this.collidersFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(GUI.skin.box);

            for (int i = 0; i < this.target.Colliders.Count; i++)
            {
                this.target.Colliders[i] = (DynamicBoneColliderBase)EditorGUILayout.ObjectField("Element " + i, this.target.Colliders[i], typeof(DynamicBoneColliderBase), true);
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Add"))
            {
                this.target.Colliders.Add(null);
            }

            if (GUILayout.Button("Remove"))
            {
                this.target.Colliders.RemoveAt(this.target.Colliders.Count - 1);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        // Exclusions
        this.exclusionsFoldout = EditorGUILayout.Foldout(this.exclusionsFoldout, "Exclusions");

        if (this.exclusionsFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(GUI.skin.box);

            for (int i = 0; i < this.target.Exclusions.Count; i++)
            {
                this.target.Exclusions[i] = (Transform)EditorGUILayout.ObjectField("Element " + i, this.target.Exclusions[i], typeof(Transform), true);
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Add"))
            {
                this.target.Exclusions.Add(null);
            }

            if (GUILayout.Button("Remove"))
            {
                this.target.Exclusions.RemoveAt(this.target.Exclusions.Count - 1);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        this.target.FreezeAxisType = (GirlsDynamicBoneObject.FreezeAxis)EditorGUILayout.EnumPopup("Freeze Axis", this.target.FreezeAxisType);

        this.target.DistantDisable = EditorGUILayout.Toggle("Distant Disable", this.target.DistantDisable);

        this.target.ReferenceObject = (Transform)EditorGUILayout.ObjectField("Reference Object", this.target.ReferenceObject, typeof(Transform), true);

        this.target.DistanceToObject = EditorGUILayout.FloatField("Distance To Object", this.target.DistanceToObject);

        EditorGUILayout.EndToggleGroup();
    }
}
