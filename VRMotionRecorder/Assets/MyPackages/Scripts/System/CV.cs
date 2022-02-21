using UnityEngine;

/// <summary>
/// エンジンコードをキャッシュするクラス.
/// これらの値を取得するときはキャッシュしたものを利用した方が速い.
/// 参考文献:http://japan.unity3d.com/unite/unite2016/files/DAY1_1700_room1_Yasuhara.pdf
/// </summary>
public class CV
{
	public static Vector3 zero    = Vector3.zero;
	public static Vector3 one     = Vector3.one;
	public static Vector3 left    = Vector3.left;
    public static Vector3 right   = Vector3.right;
    public static Vector3 up      = Vector3.up;
    public static Vector3 down    = Vector3.down;
    public static Vector3 forward = Vector3.forward;
	public static Vector3 back    = Vector3.back;


	public static Vector2 zero2 = Vector2.zero;
	public static Vector2 one2  = Vector2.one;
	public static Vector2 left2 = Vector2.left;
	public static Vector2 up2   = Vector2.up;

    public static Quaternion identity = Quaternion.identity;
}

