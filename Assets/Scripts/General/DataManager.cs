using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


#region 저장 데이터 형식

[Serializable]

public class SaveData
{
    public List<CollectionData> MonsterCollection = new();
    public List<VistVector> VisitedMaps = new();
    public List<string> AllyList = new();
}

[Serializable]
public class VistVector
{
    public int X;
    public int Z;
}

#endregion


public class DataManager : Singleton<DataManager>
{
    private const string PATH = "/SaveFile/";
    private const string FILE_NAME = "PlayerData.json";

    public void JsonLoad()
    {
        string path = Application.dataPath + PATH;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        if (!File.Exists(path + FILE_NAME))
        {
            InitializeData();
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path + FILE_NAME);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                // 도감, 맵, 부대
                //GameManager.Instance.MonsterCollection = saveData.MonsterCollection;
                //GameManager.Instance.VisitedMaps = saveData.VisitedMaps;
                //GameManager.Instance.ArmyUnits = saveData.ArmyUnits;

            }
        }
    }

    public void JsonSave()
    {
        string path = Application.dataPath + PATH;

        SaveData saveData = new SaveData();

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // 도감, 맵, 부대
        //saveData.MonsterCollection = GameManager.Instance.MonsterCollection;
        //saveData.VisitedMaps = MapManager.Instance.VisitCell();
        //saveData.AllyList = AllyManager.Instance.;

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(path + FILE_NAME, json);
    }

    public void InitializeData()
    {
        
    }
}
