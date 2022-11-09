using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(11, new string[] { "...zzZ", "...zzZ?" });
        talkData.Add(12, new string[]{"Ah...?", "Ahchoo!!" });
        talkData.Add(101, new string[] { "Warning / Dungeon Trap" });
    }
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
        {
            return null;
        }
        else 
        {
            return talkData[id][talkIndex];
        }      
    }
}
