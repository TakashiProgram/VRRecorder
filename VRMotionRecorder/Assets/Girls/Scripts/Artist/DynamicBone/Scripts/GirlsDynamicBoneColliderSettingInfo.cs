using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GirlsDynamicBoneが保持している複数のGirlsDynamicBoneObjectのCollidersに
/// 登録するコライダの情報
/// 別パーツが持っているコライダを登録することになるため、
/// キャラクターを構成する各パーツのプレハブの読み込みが完了したあとに、
/// このクラスの設定情報を元にコライダ設定を行う
/// </summary>
public class GirlsDynamicBoneColliderSettingInfo : MonoBehaviour
{
    /// <summary>
    /// コライダを保持しているパーツのタイプ
    /// </summary>
    public enum PartsType
    {
        /// <summary>
        /// トップス
        /// </summary>
        Tops,

        /// <summary>
        /// ボトムス
        /// </summary>
        Bottoms,

        /// <summary>
        /// シューズ
        /// </summary>
        Shoes,
    }

    /// <summary>
    /// 情報リスト
    /// GirlsDynamicBoneが保持しているGirlsDynamicBoneObjectのリストと
    /// 並びが同じである前提
    /// </summary>
    [SerializeField]
    private List<DynamicBoneColliderInfo> infoList = new List<DynamicBoneColliderInfo>();

    /// <summary>
    /// 情報リスト
    /// </summary>
    public List<DynamicBoneColliderInfo> InfoList
    {
        get { return this.infoList; }
        set { this.infoList = value; }
    }

    /// <summary>
    /// 追加
    /// </summary>
    /// <returns>追加したInfo</returns>
    public DynamicBoneColliderInfo Add()
    {
        var info = new DynamicBoneColliderInfo();

        this.infoList.Add(info);

        return info;
    }

    /// <summary>
    /// 1つのGirlsDynamicBoneObjectに設定するコライダ情報
    /// </summary>
    [Serializable]
    public class DynamicBoneColliderInfo
    {
        /// <summary>
        /// コライダ情報のリスト
        /// </summary>
        [SerializeField]
        private List<ColliderElement> elementList;

        /// <summary>
        /// コライダ情報のリスト
        /// </summary>
        public List<ColliderElement> ElementList
        {
            get { return this.elementList; }
            set { this.elementList = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DynamicBoneColliderInfo()
        {
            this.elementList = new List<ColliderElement>();
        }

        public ColliderElement Add()
        {
            var element = new ColliderElement();

            this.elementList.Add(element);

            return element;
        }
    }

    /// <summary>
    /// コライダ情報
    /// コライダを保持しているパーツとそのボーン名を持つ
    /// </summary>
    [Serializable]
    public class ColliderElement
    {
        /// <summary>
        /// コライダを保持しているパーツのタイプ
        /// </summary>
        [SerializeField]
        private PartsType type;

        /// <summary>
        /// ボーン名
        /// </summary>
        [SerializeField]
        private string boneName;

        /// <summary>
        /// コライダを保持しているパーツのタイプ
        /// </summary>
        public PartsType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>
        /// ボーン名
        /// </summary>
        public string BoneName
        {
            get { return this.boneName; }
            set { this.boneName = value; }
        }
    }
}
