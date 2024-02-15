using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomSaver
{

    static string roomOrder = "RoomOrderTag";
    public static void saveroom(int[] rOrder)
    {
        var temp = new RoomOrder();
        temp.roomOrder = rOrder;
        var d = JsonUtility.ToJson(temp);
        SaveData.setString(roomOrder, d);
    }

    public static int[] loadroom()
    {
        var d = SaveData.getString(roomOrder);
        if(string.IsNullOrEmpty(d)) {
            //Debug.LogError("No Room Saved");
            return null;
        }
        return JsonUtility.FromJson<RoomOrder>(d).roomOrder;
    }
}
