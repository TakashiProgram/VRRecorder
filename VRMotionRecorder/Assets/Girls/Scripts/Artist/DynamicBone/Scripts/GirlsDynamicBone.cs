using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// １つのコンポーネントで複数のDynamicBoneの更新を行うようにしたクラス
/// </summary>
[AddComponentMenu("Dynamic Bone/Girls Dynamic Bone")]
public class GirlsDynamicBone : MonoBehaviour
{
    /// <summary>
    /// DynamicBoneオブジェクトリスト
    /// </summary>
    [SerializeField]
    private List<GirlsDynamicBoneObject> boneObjects = new List<GirlsDynamicBoneObject>();

    /// <summary>
    /// DynamicBoneオブジェクト配列
    /// </summary>
    public GirlsDynamicBoneObject[] BoneObjects
    {
        get { return this.boneObjects.ToArray(); }
    }

    /// <summary>
    /// 更新レートの設定
    /// </summary>
    /// <param name="rate"></param>
    public void SetUpdateRate(float rate)
    {
        foreach (var obj in this.boneObjects)
        {
            obj.UpdateRate = rate;
        }
    }

    /// <summary>
    /// 起動処理
    /// </summary>
    private void Awake()
    {
        // 読み込み中に動作されると余計な負荷がかかるので各パーツが読み終わるまでは止めておく
        // アーティストさんが一時的に手動で構築する可能性を考えてROMのときのみ処理を行うようにする
        // Editor時はCharacter.LoadAndInstantiateAsyncで行う
//#if !UNITY_EDITOR
//        this.enabled = false;
//#endif
    }

    /// <summary>
    /// 初回Update前処理
    /// </summary>
    private void Start()
    {
        foreach (var obj in this.boneObjects)
        {
            obj.Start();
        }
    }

    /// <summary>
    /// 固定間隔更新
    /// </summary>
    private void FixedUpdate()
    {
        foreach (var obj in this.boneObjects)
        {
            obj.FixedUpdate();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        foreach (var obj in this.boneObjects)
        {
            obj.Update();
        }
    }

    /// <summary>
    /// 後更新
    /// </summary>
    private void LateUpdate()
    {
        foreach (var obj in this.boneObjects)
        {
            obj.LateUpdate();
        }
    }

    /// <summary>
    /// 有効化時処理
    /// </summary>
    private void OnEnable()
    {
        foreach (var obj in this.boneObjects)
        {
            obj.OnEnable();
        }
    }

    /// <summary>
    /// 無効化時処理
    /// </summary>
    private void OnDisable()
    {
        foreach (var obj in this.boneObjects)
        {
            obj.OnDisable();
        }
    }

    /// <summary>
    /// インスペクター変更時呼び出し
    /// </summary>
    private void OnValidate()
    {
        foreach (var obj in this.boneObjects)
        {
            obj.OnValidate();
        }
    }

    /// <summary>
    /// 選択時のGizmo表示
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        foreach (var obj in this.boneObjects)
        {
            obj.OnDrawGizmosSelected();
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// オブジェクト追加
    /// </summary>
    public GirlsDynamicBoneObject AddObject()
    {
        var obj = new GirlsDynamicBoneObject(this);
        this.boneObjects.Add(obj);

        return obj;
    }

    /// <summary>
    /// オブジェクト削除
    /// </summary>
    public void RemoveObject()
    {
        this.boneObjects.RemoveAt(this.boneObjects.Count - 1);
    }
#endif
}
