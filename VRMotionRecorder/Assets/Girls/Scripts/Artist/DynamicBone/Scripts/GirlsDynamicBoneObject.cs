using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviourを継承しないDynamicBone
/// GirlsDynamicBone側で管理、更新を行う
/// </summary>
[Serializable]
public class GirlsDynamicBoneObject
{
    /// <summary>
    /// 更新レートのデフォルト値
    /// </summary>
    public const float DefaultUpdateRate = 60.0f;

    /// <summary>
    /// オーナーとなるGirlsDynamicBoneクラス
    /// </summary>
    [SerializeField]
    private GirlsDynamicBone owner = null;

    /// <summary>
    /// 有効か
    /// </summary>
    [SerializeField]
    private bool enabled = true;

#if UNITY_5
	[Tooltip("The root of the transform hierarchy to apply physics.")]
#endif
    /// <summary>
    /// 設定先のTransform
    /// </summary>
    [SerializeField]
    private Transform root = null;

#if UNITY_5
	[Tooltip("Internal physics simulation rate.")]
#endif
    /// <summary>
    /// 更新レート
    /// </summary>
    [SerializeField]
    private float updateRate = DefaultUpdateRate;

    /// <summary>
    /// 更新モード
    /// </summary>
    public enum UpdateMode
    {
        /// <summary>
        /// 通常モード
        /// </summary>
        Normal,

        /// <summary>
        /// アニメーションと物理演算を連携させる
        /// </summary>
        AnimatePhysics,

        /// <summary>
        /// timeScaleに影響されない
        /// </summary>
        UnscaledTime
    }

    /// <summary>
    /// 更新モード
    /// </summary>
    [SerializeField]
    private UpdateMode updateMode = UpdateMode.Normal;

#if UNITY_5
	[Tooltip("How much the bones slowed down.")]
#endif
    /// <summary>
    /// 減衰値
    /// </summary>
    [Range(0, 1)]
    [SerializeField]
    private float damping = 0.1f;

    /// <summary>
    /// 減衰値カーブ
    /// </summary>
    [SerializeField]
    private AnimationCurve dampingDistrib = null;

#if UNITY_5
	[Tooltip("How much the force applied to return each bone to original orientation.")]
#endif
    /// <summary>
    /// 弾性値
    /// </summary>
    [Range(0, 1)]
    [SerializeField]
    private float elasticity = 0.1f;

    /// <summary>
    /// 弾性値カーブ
    /// </summary>
    [SerializeField]
    private AnimationCurve elasticityDistrib = null;

#if UNITY_5
	[Tooltip("How much bone's original orientation are preserved.")]
#endif
    /// <summary>
    /// 剛性値
    /// </summary>
    [Range(0, 1)]
    [SerializeField]
    private float stiffness = 0.1f;

    /// <summary>
    /// 剛性値カーブ
    /// </summary>
    [SerializeField]
    private AnimationCurve stiffnessDistrib = null;

#if UNITY_5
	[Tooltip("How much character's position change is ignored in physics simulation.")]
#endif
    /// <summary>
    /// 不活性値
    /// </summary>
    [Range(0, 1)]
    [SerializeField]
    private float inert = 0;

    /// <summary>
    /// 不活性値カーブ
    /// </summary>
    [SerializeField]
    private AnimationCurve inertDistrib = null;

#if UNITY_5
	[Tooltip("Each bone can be a sphere to collide with colliders. Radius describe sphere's size.")]
#endif
    /// <summary>
    /// 半径
    /// </summary>
    [SerializeField]
    private float radius = 0;

    /// <summary>
    /// 半径値カーブ
    /// </summary>
    [SerializeField]
    private AnimationCurve radiusDistrib = null;

#if UNITY_5
	[Tooltip("If End Length is not zero, an extra bone is generated at the end of transform hierarchy.")]
#endif
    /// <summary>
    /// End Length
    /// </summary>
    [SerializeField]
    private float endLength = 0;

#if UNITY_5
	[Tooltip("If End Offset is not zero, an extra bone is generated at the end of transform hierarchy.")]
#endif
    /// <summary>
    /// End Offset
    /// </summary>
    [SerializeField]
    private Vector3 endOffset = Vector3.zero;

#if UNITY_5
	[Tooltip("The force apply to bones. Partial force apply to character's initial pose is cancelled out.")]
#endif
    /// <summary>
    /// 重力
    /// </summary>
    [SerializeField]
    private Vector3 gravity = Vector3.zero;

#if UNITY_5
	[Tooltip("The force apply to bones.")]
#endif
    /// <summary>
    /// 力
    /// </summary>
    [SerializeField]
    private Vector3 force = Vector3.zero;

#if UNITY_5
	[Tooltip("Collider objects interact with the bones.")]
#endif
    /// <summary>
    /// コライダーリスト
    /// </summary>
    [SerializeField]
    private List<DynamicBoneColliderBase> colliders = null;

#if UNITY_5
	[Tooltip("Bones exclude from physics simulation.")]
#endif
    /// <summary>
    /// 適用しないTransformリスト
    /// </summary>
    [SerializeField]
    private List<Transform> exclusions = null;

    /// <summary>
    /// フリーズする軸
    /// </summary>
    public enum FreezeAxis
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// X軸
        /// </summary>
        X,

        /// <summary>
        /// Y軸
        /// </summary>
        Y,

        /// <summary>
        /// Z軸
        /// </summary>
        Z
    }
#if UNITY_5
	[Tooltip("Constrain bones to move on specified plane.")]
#endif
    /// <summary>
    /// フリーズする軸
    /// </summary>
    [SerializeField]
    private FreezeAxis freezeAxis = FreezeAxis.None;

#if UNITY_5
	[Tooltip("Disable physics simulation automatically if character is far from camera or player.")]
#endif
    /// <summary>
    /// 距離が遠かったら自動的に無効にするか
    /// </summary>
    [SerializeField]
    private bool distantDisable = false;

    /// <summary>
    /// 参照するオブジェクト
    /// </summary>
    [SerializeField]
    private Transform referenceObject = null;

    /// <summary>
    /// 無効にする距離
    /// </summary>
    [SerializeField]
    private float distanceToObject = 20;

    /// <summary>
    /// ローカルの重力値
    /// </summary>
    private Vector3 localGravity = Vector3.zero;

    /// <summary>
    /// オブジェクトの移動
    /// </summary>
    private Vector3 objectMove = Vector3.zero;

    /// <summary>
    /// オブジェクトの前回の位置
    /// </summary>
    private Vector3 objectPrevPosition = Vector3.zero;

    /// <summary>
    /// ボーンの長さ
    /// </summary>
    private float boneTotalLength = 0;

    /// <summary>
    /// オブジェクトのスケール
    /// </summary>
    private float objectScale = 1.0f;

    /// <summary>
    /// 時間
    /// </summary>
    private float time = 0;

    /// <summary>
    /// ウェイト
    /// </summary>
    private float weight = 1.0f;

    /// <summary>
    /// 遠い場合に無効にした
    /// </summary>
    private bool distantDisabled = false;

    /// <summary>
    /// パーティクル
    /// </summary>
    private class Particle
    {
        public Transform Transform { get; set; } = null;
        public int ParentIndex { get; set; } = -1;
        public float Damping { get; set; } = 0;
        public float Elasticity { get; set; } = 0;
        public float Stiffness { get; set; } = 0;
        public float Inert { get; set; } = 0;
        public float Radius { get; set; } = 0;
        public float BoneLength { get; set; } = 0;

        public Vector3 Position { get; set; } = Vector3.zero;
        public Vector3 PrevPosition { get; set; } = Vector3.zero;
        public Vector3 EndOffset { get; set; } = Vector3.zero;
        public Vector3 InitLocalPosition { get; set; } = Vector3.zero;
        public Quaternion InitLocalRotation { get; set; } = Quaternion.identity;
    }

    /// <summary>
    /// パーティクルリスト
    /// </summary>
    private List<Particle> particles = new List<Particle>();

    /// <summary>
    /// 有効か
    /// </summary>
    public bool Enabled
    {
        get { return this.enabled; }
        set { this.enabled = value; }
    }

    /// <summary>
    /// 設定先のTransform
    /// </summary>
    public Transform Root
    {
        get { return this.root; }
        set { this.root = value; }
    }

    /// <summary>
    /// 更新レート
    /// </summary>
    public float UpdateRate
    {
        get { return this.updateRate; }
        set { this.updateRate = value; }
    }

    /// <summary>
    /// アップデートモード
    /// </summary>
    public UpdateMode UpdateModeType
    {
        get { return this.updateMode; }
        set { this.updateMode = value; }
    }

    /// <summary>
    /// 減衰値
    /// </summary>
    public float Damping
    {
        get { return this.damping; }
        set { this.damping = value; }
    }

    /// <summary>
    /// 減衰値カーブ
    /// </summary>
    public AnimationCurve DampingDistrib
    {
        get { return this.dampingDistrib; }
        set { this.dampingDistrib = value; }
    }

    /// <summary>
    /// 弾性値
    /// </summary>
    public float Elasticity
    {
        get { return this.elasticity; }
        set { this.elasticity = value; }
    }

    /// <summary>
    /// 弾性値カーブ
    /// </summary>
    public AnimationCurve ElasticityDistrib
    {
        get { return this.elasticityDistrib; }
        set { this.elasticityDistrib = value; }
    }

    /// <summary>
    /// 剛性値
    /// </summary>
    public float Stiffness
    {
        get { return this.stiffness; }
        set { this.stiffness = value; }
    }

    /// <summary>
    /// 剛性値カーブ
    /// </summary>
    public AnimationCurve StiffnessDistrib
    {
        get { return this.stiffnessDistrib; }
        set { this.stiffnessDistrib = value; }
    }

    /// <summary>
    /// 不活性値
    /// </summary>
    public float Inert
    {
        get { return this.inert; }
        set { this.inert = value; }
    }

    /// <summary>
    /// 不活性値カーブ
    /// </summary>
    public AnimationCurve InertDistrib
    {
        get { return this.inertDistrib; }
        set { this.inertDistrib = value; }
    }

    /// <summary>
    /// 半径
    /// </summary>
    public float Radius
    {
        get { return this.radius; }
        set { this.radius = value; }
    }

    /// <summary>
    /// 半径値カーブ
    /// </summary>
    public AnimationCurve RadiusDistrib
    {
        get { return this.radiusDistrib; }
        set { this.radiusDistrib = value; }
    }

    /// <summary>
    /// 終端の長さ
    /// </summary>
    public float EndLength
    {
        get { return this.endLength; }
        set { this.endLength = value; }
    }

    /// <summary>
    /// 終端オフセット
    /// </summary>
    public Vector3 EndOffset
    {
        get { return this.endOffset; }
        set { this.endOffset = value; }
    }

    /// <summary>
    /// 重力
    /// </summary>
    public Vector3 Gravity
    {
        get { return this.gravity; }
        set { this.gravity = value; }
    }

    /// <summary>
    /// 力
    /// </summary>
    public Vector3 Force
    {
        get { return this.force; }
        set { this.force = value; }
    }

    /// <summary>
    /// コライダーリスト
    /// </summary>
    public List<DynamicBoneColliderBase> Colliders
    {
        get { return this.colliders; }
        set { this.colliders = value; }
    }

    /// <summary>
    /// 適用しないTransformリスト
    /// </summary>
    public List<Transform> Exclusions
    {
        get { return this.exclusions; }
        set { this.exclusions = value; }
    }

    /// <summary>
    /// フリーズする軸
    /// </summary>
    public FreezeAxis FreezeAxisType
    {
        get { return this.freezeAxis; }
        set { this.freezeAxis = value; }
    }

    /// <summary>
    /// 距離が遠かったら自動的に無効にするか
    /// </summary>
    public bool DistantDisable
    {
        get { return this.distantDisable; }
        set { this.distantDisable = value; }
    }

    /// <summary>
    /// 参照するオブジェクト
    /// </summary>
    public Transform ReferenceObject
    {
        get { return this.referenceObject; }
        set { this.referenceObject = value; }
    }

    /// <summary>
    /// 無効にする距離
    /// </summary>
    public float DistanceToObject
    {
        get { return this.distanceToObject; }
        set { this.distanceToObject = value; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GirlsDynamicBoneObject()
    {
        this.localGravity = Vector3.zero;
        this.objectMove = Vector3.zero;
        this.objectPrevPosition = Vector3.zero;
        this.boneTotalLength = 0;
        this.objectScale = 1.0f;
        this.time = 0;
        this.weight = 1.0f;
        this.distantDisabled = false;

        this.particles = new List<Particle>();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="owner">オーナー</param>
    public GirlsDynamicBoneObject(GirlsDynamicBone owner)
        : this()
    {
        this.owner = owner;
    }

    /// <summary>
    /// 初回Update前処理
    /// </summary>
    public void Start()
    {
        SetupParticles();
    }

    /// <summary>
    /// 定期更新
    /// </summary>
    public void FixedUpdate()
    {
        if (updateMode == UpdateMode.AnimatePhysics)
            PreUpdate();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        if (updateMode != UpdateMode.AnimatePhysics)
            PreUpdate();
    }

    /// <summary>
    /// 後更新
    /// </summary>
    public void LateUpdate()
    {
        if (distantDisable)
            CheckDistance();

        if (weight > 0 && !(distantDisable && distantDisabled))
        {
#if UNITY_5
            float dt = m_UpdateMode == UpdateMode.UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
#else
            float dt = Time.deltaTime;
#endif
            UpdateDynamicBones(dt);
        }
    }

    /// <summary>
    /// 前更新
    /// </summary>
    public void PreUpdate()
    {
        if (weight > 0 && !(distantDisable && distantDisabled))
            InitTransforms();
    }

    /// <summary>
    /// 距離計算
    /// </summary>
    private void CheckDistance()
    {
        Transform rt = referenceObject;
        if (rt == null && Camera.main != null)
            rt = Camera.main.transform;
        if (rt != null)
        {
            float d = (rt.position - this.owner.transform.position).sqrMagnitude;
            bool disable = d > distanceToObject * distanceToObject;
            if (disable != distantDisabled)
            {
                if (!disable)
                    ResetParticlesPosition();
                distantDisabled = disable;
            }
        }
    }

    /// <summary>
    /// 有効化時処理
    /// </summary>
    public void OnEnable()
    {
        ResetParticlesPosition();
    }

    /// <summary>
    /// 無効化時処理
    /// </summary>
    public void OnDisable()
    {
        InitTransforms();
    }

    /// <summary>
    /// インスペクター更新時処理
    /// </summary>
    public void OnValidate()
    {
        updateRate = Mathf.Max(updateRate, 0);
        damping = Mathf.Clamp01(damping);
        elasticity = Mathf.Clamp01(elasticity);
        stiffness = Mathf.Clamp01(stiffness);
        inert = Mathf.Clamp01(inert);
        radius = Mathf.Max(radius, 0);

        if (Application.isEditor && Application.isPlaying)
        {
            InitTransforms();
            SetupParticles();
        }
    }

    /// <summary>
    /// 選択時のGizmo表示
    /// </summary>
    public void OnDrawGizmosSelected()
    {
        if (!enabled || root == null)
            return;

        if (Application.isEditor && !Application.isPlaying && this.owner.transform.hasChanged)
        {
            InitTransforms();
            SetupParticles();
        }

        Gizmos.color = Color.white;
        for (int i = 0; i < particles.Count; ++i)
        {
            Particle p = particles[i];
            if (p.ParentIndex >= 0)
            {
                Particle p0 = particles[p.ParentIndex];
                Gizmos.DrawLine(p.Position, p0.Position);
            }
            if (p.Radius > 0)
                Gizmos.DrawWireSphere(p.Position, p.Radius * objectScale);
        }
    }

    /// <summary>
    /// ウェイト設定
    /// </summary>
    /// <param name="w"></param>
    public void SetWeight(float w)
    {
        if (weight != w)
        {
            if (w == 0)
                InitTransforms();
            else if (weight == 0)
                ResetParticlesPosition();
            weight = w;
        }
    }

    /// <summary>
    /// ウェイト取得
    /// </summary>
    /// <returns></returns>
    public float GetWeight()
    {
        return weight;
    }

    /// <summary>
    /// ボーン計算
    /// </summary>
    /// <param name="t"></param>
    public void UpdateDynamicBones(float t)
    {
        if (root == null)
            return;

        objectScale = Mathf.Abs(this.owner.transform.lossyScale.x);
        objectMove = this.owner.transform.position - objectPrevPosition;
        objectPrevPosition = this.owner.transform.position;

        int loop = 1;
        if (updateRate > 0)
        {
            float dt = 1.0f / updateRate;
            time += t;
            loop = 0;

            while (time >= dt)
            {
                time -= dt;
                if (++loop >= 30)
                {
                    time = 0;
                    break;
                }
            }
        }

        if (loop > 0)
        {
            for (int i = 0; i < loop; ++i)
            {
                UpdateParticles1();
                UpdateParticles2();
                objectMove = Vector3.zero;
            }
        }
        else
        {
            SkipUpdateParticles();
        }

        ApplyParticlesToTransforms();
    }

    /// <summary>
    /// パーティクルのセットアップ
    /// </summary>
    public void SetupParticles()
    {
        particles.Clear();
        if (root == null)
            return;

        localGravity = root.InverseTransformDirection(gravity);
        objectScale = Mathf.Abs(this.owner.transform.lossyScale.x);
        objectPrevPosition = this.owner.transform.position;
        objectMove = Vector3.zero;
        boneTotalLength = 0;
        AppendParticles(root, -1, 0);
        UpdateParameters();
    }

    /// <summary>
    /// パーティクルの追加
    /// </summary>
    /// <param name="b">ボーン</param>
    /// <param name="parentIndex">親のインデックス</param>
    /// <param name="boneLength">ボーンの長さ</param>
    public void AppendParticles(Transform b, int parentIndex, float boneLength)
    {
        Particle p = new Particle();
        p.Transform = b;
        p.ParentIndex = parentIndex;
        if (b != null)
        {
            p.Position = p.PrevPosition = b.position;
            p.InitLocalPosition = b.localPosition;
            p.InitLocalRotation = b.localRotation;
        }
        else 	// end bone
        {
            Transform pb = particles[parentIndex].Transform;
            if (endLength > 0)
            {
                Transform ppb = pb.parent;
                if (ppb != null)
                    p.EndOffset = pb.InverseTransformPoint((pb.position * 2 - ppb.position)) * endLength;
                else
                    p.EndOffset = new Vector3(endLength, 0, 0);
            }
            else
            {
                p.EndOffset = pb.InverseTransformPoint(this.owner.transform.TransformDirection(endOffset) + pb.position);
            }
            p.Position = p.PrevPosition = pb.TransformPoint(p.EndOffset);
        }

        if (parentIndex >= 0)
        {
            boneLength += (particles[parentIndex].Transform.position - p.Position).magnitude;
            p.BoneLength = boneLength;
            boneTotalLength = Mathf.Max(boneTotalLength, boneLength);
        }

        int index = particles.Count;
        particles.Add(p);

        if (b != null)
        {
            for (int i = 0; i < b.childCount; ++i)
            {
                bool exclude = false;
                if (exclusions != null)
                {
                    for (int j = 0; j < exclusions.Count; ++j)
                    {
                        Transform e = exclusions[j];
                        if (e == b.GetChild(i))
                        {
                            exclude = true;
                            break;
                        }
                    }
                }
                if (!exclude)
                    AppendParticles(b.GetChild(i), index, boneLength);
                else if (endLength > 0 || endOffset != Vector3.zero)
                    AppendParticles(null, index, boneLength);
            }

            if (b.childCount == 0 && (endLength > 0 || endOffset != Vector3.zero))
                AppendParticles(null, index, boneLength);
        }
    }

    /// <summary>
    /// パラメータ更新
    /// </summary>
    public void UpdateParameters()
    {
        if (root == null)
            return;

        localGravity = root.InverseTransformDirection(gravity);

        for (int i = 0; i < particles.Count; ++i)
        {
            Particle p = particles[i];
            p.Damping = damping;
            p.Elasticity = elasticity;
            p.Stiffness = stiffness;
            p.Inert = inert;
            p.Radius = radius;

            if (boneTotalLength > 0)
            {
                float a = p.BoneLength / boneTotalLength;
                if (dampingDistrib != null && dampingDistrib.keys.Length > 0)
                    p.Damping *= dampingDistrib.Evaluate(a);
                if (elasticityDistrib != null && elasticityDistrib.keys.Length > 0)
                    p.Elasticity *= elasticityDistrib.Evaluate(a);
                if (stiffnessDistrib != null && stiffnessDistrib.keys.Length > 0)
                    p.Stiffness *= stiffnessDistrib.Evaluate(a);
                if (inertDistrib != null && inertDistrib.keys.Length > 0)
                    p.Inert *= inertDistrib.Evaluate(a);
                if (radiusDistrib != null && radiusDistrib.keys.Length > 0)
                    p.Radius *= radiusDistrib.Evaluate(a);
            }

            p.Damping = Mathf.Clamp01(p.Damping);
            p.Elasticity = Mathf.Clamp01(p.Elasticity);
            p.Stiffness = Mathf.Clamp01(p.Stiffness);
            p.Inert = Mathf.Clamp01(p.Inert);
            p.Radius = Mathf.Max(p.Radius, 0);
        }
    }

    /// <summary>
    /// Transform初期化
    /// </summary>
    public void InitTransforms()
    {
        for (int i = 0; i < particles.Count; ++i)
        {
            Particle p = particles[i];
            if (p.Transform != null)
            {
                p.Transform.localPosition = p.InitLocalPosition;
                p.Transform.localRotation = p.InitLocalRotation;
            }
        }
    }

    /// <summary>
    /// パーティクルの位置をリセット
    /// </summary>
    public void ResetParticlesPosition()
    {
        for (int i = 0; i < particles.Count; ++i)
        {
            Particle p = particles[i];
            if (p.Transform != null)
            {
                p.Position = p.PrevPosition = p.Transform.position;
            }
            else	// end bone
            {
                Transform pb = particles[p.ParentIndex].Transform;
                p.Position = p.PrevPosition = pb.TransformPoint(p.EndOffset);
            }
        }
        objectPrevPosition = this.owner.transform.position;
    }

    /// <summary>
    /// パーティクル更新その１
    /// </summary>
    public void UpdateParticles1()
    {
        Vector3 force = gravity;
        Vector3 fdir = gravity.normalized;
        Vector3 rf = root.TransformDirection(localGravity);
        Vector3 pf = fdir * Mathf.Max(Vector3.Dot(rf, fdir), 0);	// project current gravity to rest gravity
        force -= pf;	// remove projected gravity
        force = (force + this.force) * objectScale;

        for (int i = 0; i < particles.Count; ++i)
        {
            Particle p = particles[i];
            if (p.ParentIndex >= 0)
            {
                // verlet integration
                Vector3 v = p.Position - p.PrevPosition;
                Vector3 rmove = objectMove * p.Inert;
                p.PrevPosition = p.Position + rmove;
                p.Position += v * (1 - p.Damping) + force + rmove;
            }
            else
            {
                p.PrevPosition = p.Position;
                p.Position = p.Transform.position;
            }
        }
    }

    /// <summary>
    /// パーティクイル更新その２
    /// </summary>
    public void UpdateParticles2()
    {
        Plane movePlane = new Plane();

        for (int i = 1; i < particles.Count; ++i)
        {
            Particle p = particles[i];
            Particle p0 = particles[p.ParentIndex];

            float restLen;
            if (p.Transform != null)
                restLen = (p0.Transform.position - p.Transform.position).magnitude;
            else
                restLen = p0.Transform.localToWorldMatrix.MultiplyVector(p.EndOffset).magnitude;

            // keep shape
            float stiffness = Mathf.Lerp(1.0f, p.Stiffness, weight);
            if (stiffness > 0 || p.Elasticity > 0)
            {
                Matrix4x4 m0 = p0.Transform.localToWorldMatrix;
                m0.SetColumn(3, p0.Position);
                Vector3 restPos;
                if (p.Transform != null)
                    restPos = m0.MultiplyPoint3x4(p.Transform.localPosition);
                else
                    restPos = m0.MultiplyPoint3x4(p.EndOffset);

                Vector3 d = restPos - p.Position;
                p.Position += d * p.Elasticity;

                if (stiffness > 0)
                {
                    d = restPos - p.Position;
                    float len = d.magnitude;
                    float maxlen = restLen * (1 - stiffness) * 2;
                    if (len > maxlen)
                        p.Position += d * ((len - maxlen) / len);
                }
            }

            // collide
            if (colliders != null)
            {
                float particleRadius = p.Radius * objectScale;
                for (int j = 0; j < colliders.Count; ++j)
                {
                    DynamicBoneColliderBase c = colliders[j];
                    if (c != null && c.enabled)
                    {
                        var pos = p.Position;
                        c.Collide(ref pos, particleRadius);
                        p.Position = pos;
                    }
                }
            }

            // freeze axis, project to plane
            if (freezeAxis != FreezeAxis.None)
            {
                switch (freezeAxis)
                {
                    case FreezeAxis.X:
                        movePlane.SetNormalAndPosition(p0.Transform.right, p0.Position);
                        break;
                    case FreezeAxis.Y:
                        movePlane.SetNormalAndPosition(p0.Transform.up, p0.Position);
                        break;
                    case FreezeAxis.Z:
                        movePlane.SetNormalAndPosition(p0.Transform.forward, p0.Position);
                        break;
                }
                p.Position -= movePlane.normal * movePlane.GetDistanceToPoint(p.Position);
            }

            // keep length
            Vector3 dd = p0.Position - p.Position;
            float leng = dd.magnitude;
            if (leng > 0)
                p.Position += dd * ((leng - restLen) / leng);
        }
    }

    /// <summary>
    /// 剛性のみを更新して骨の長さを維持する
    /// </summary>
    public void SkipUpdateParticles()
    {
        for (int i = 0; i < particles.Count; ++i)
        {
            Particle p = particles[i];
            if (p.ParentIndex >= 0)
            {
                p.PrevPosition += objectMove;
                p.Position += objectMove;

                Particle p0 = particles[p.ParentIndex];

                float restLen;
                if (p.Transform != null)
                    restLen = (p0.Transform.position - p.Transform.position).magnitude;
                else
                    restLen = p0.Transform.localToWorldMatrix.MultiplyVector(p.EndOffset).magnitude;

                // keep shape
                float stiffness = Mathf.Lerp(1.0f, p.Stiffness, weight);
                if (stiffness > 0)
                {
                    Matrix4x4 m0 = p0.Transform.localToWorldMatrix;
                    m0.SetColumn(3, p0.Position);
                    Vector3 restPos;
                    if (p.Transform != null)
                        restPos = m0.MultiplyPoint3x4(p.Transform.localPosition);
                    else
                        restPos = m0.MultiplyPoint3x4(p.EndOffset);

                    Vector3 d = restPos - p.Position;
                    float len = d.magnitude;
                    float maxlen = restLen * (1 - stiffness) * 2;
                    if (len > maxlen)
                        p.Position += d * ((len - maxlen) / len);
                }

                // keep length
                Vector3 dd = p0.Position - p.Position;
                float leng = dd.magnitude;
                if (leng > 0)
                    p.Position += dd * ((leng - restLen) / leng);
            }
            else
            {
                p.PrevPosition = p.Position;
                p.Position = p.Transform.position;
            }
        }
    }

    /// <summary>
    /// 任意軸で反転
    /// </summary>
    /// <param name="v">ベクトル</param>
    /// <param name="axis">反転軸</param>
    /// <returns></returns>
    public static Vector3 MirrorVector(Vector3 v, Vector3 axis)
    {
        return v - axis * (Vector3.Dot(v, axis) * 2);
    }

    /// <summary>
    /// パーティクルをTransformに適用
    /// </summary>
    public void ApplyParticlesToTransforms()
    {
        for (int i = 1; i < particles.Count; ++i)
        {
            Particle p = particles[i];
            Particle p0 = particles[p.ParentIndex];

            if (p0.Transform.childCount <= 1)		// do not modify bone orientation if has more then one child
            {
                Vector3 v;
                if (p.Transform != null)
                    v = p.Transform.localPosition;
                else
                    v = p.EndOffset;
                Vector3 v2 = p.Position - p0.Position;
                Quaternion rot = Quaternion.FromToRotation(p0.Transform.TransformDirection(v), v2);
                p0.Transform.rotation = rot * p0.Transform.rotation;
            }

            if (p.Transform != null)
                p.Transform.position = p.Position;
        }
    }
}
