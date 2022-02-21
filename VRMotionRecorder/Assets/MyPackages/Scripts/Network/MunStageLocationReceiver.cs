using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MunStageLocationReceiver : MonobitEngine.MonoBehaviour
{
    private static readonly string OFFSET_POS = "OFFSET_POS";
    private static readonly string OFFSET_ROT = "OFFSET_ROT";

    void Start()
    {
        StartCoroutine(SetUp());
    }

    private IEnumerator SetUp()
    {
        while ((false == MonobitEngine.MonobitNetwork.room.customParameters.ContainsKey(OFFSET_POS)) ||
            (false == MonobitEngine.MonobitNetwork.room.customParameters.ContainsKey(OFFSET_ROT)))
        {
            yield return null;
        }

        var offset_pos_str = (string)MonobitEngine.MonobitNetwork.room.customParameters[OFFSET_POS];
        var offset_rot_str = (string)MonobitEngine.MonobitNetwork.room.customParameters[OFFSET_ROT];

        SetPos(StringToVector3(offset_pos_str));
        SetRot(StringToVector3(offset_rot_str));
    }


    //ルーム内プレイヤーのパラメータが変更された際のコールバック
    void OnMonobitCustomRoomParametersChanged(Hashtable peopertiesThatChanged)
    {
        if (true == peopertiesThatChanged.ContainsKey(OFFSET_POS))
        {
            var offset_pos_str = (string)peopertiesThatChanged[OFFSET_POS];
            SetPos(StringToVector3(offset_pos_str));
        }

        if (true == peopertiesThatChanged.ContainsKey(OFFSET_ROT))
        {
            var offset_rot_str = (string)peopertiesThatChanged[OFFSET_ROT];
            SetRot(StringToVector3(offset_rot_str));
        }
    }

    private void SetPos(Vector3 offset)
    {
        offset.y = transform.position.y; //暫定：ルームの高さズレを吸収するため、オフセットの高さはない。
        transform.position = offset;
    }

    private void SetRot(Vector3 offset)
    {
        transform.rotation = Quaternion.Euler(offset);
    }

    private Vector3 StringToVector3(string str)
    {
        if (str.StartsWith("(") && str.EndsWith(")"))
        {
            str = str.Substring(1, str.Length - 2);
        }

        // split the items
        string[] array = str.Split(',');

        return new Vector3(
            float.Parse(array[0]),
            float.Parse(array[1]),
            float.Parse(array[2]));
    }
}
