using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


#region 저장 데이터 형식

[Serializable]
public class SaveData
{
    public List<string> FoundMonsterList = new();
    public List<string> HasBeenAllyMonsterList = new();

    public List<VistVector> VisitedMaps = new();
    public List<string> AllyList = new();
}

[Serializable]
public class VistVector
{
    public int X;
    public int Z;

    public VistVector(int x, int z)
    {
        X = x;
        Z = z;
    }
}

#endregion


public class DataManager : Singleton<DataManager>, IInitializableManager
{
    [SerializeField] private List<MonsterData> _monsterDataList;

    private const string PATH = "/SaveFile/";
    private const string FILE_NAME = "PlayerData.json";

    private SaveData _saveData = new();

    public List<MonsterData> MonsterDataList => _monsterDataList;


    public void Initialize()
    {
        JsonLoad();
    }

    public List<string> GetSavedAllyList()
    {
        return _saveData.AllyList;
    }

    public MonsterData GetMonsterData(string id)
    {
        foreach (var data in _monsterDataList)
        {
            if (data.MonsterId == id)
                return data;
        }

        return null;
    }

    public bool IsMonsterFound(string id)
    {
        return _saveData.FoundMonsterList.Contains(id);
    }

    public void SetMonsterFound(string id)
    {
        if (!_saveData.FoundMonsterList.Contains(id))
        {
            _saveData.FoundMonsterList.Add(id);
            JsonSave();
        }
    }

    public bool IsMonsterAlly(string id)
    {
        return _saveData.HasBeenAllyMonsterList.Contains(id);
    }

    public void SetMonsterAlly(string id)
    {
        if (!_saveData.HasBeenAllyMonsterList.Contains(id))
        {
            _saveData.HasBeenAllyMonsterList.Add(id);
            JsonSave();
        }
    }

    public List<VistVector> GetVisitedMaps()
    {
        return _saveData.VisitedMaps;
    }

    public void VisitMap(Vector3Int key)
    {
        foreach (var v in _saveData.VisitedMaps)
        {
            if (v.X == key.x && v.Z == key.z)
                return;
        }

        _saveData.VisitedMaps.Add(new VistVector(key.x, key.z));
        JsonSave();
    }

    public void UpdateAllyList(List<string> allyList)
    {
        _saveData.AllyList = allyList;

        foreach(var id in allyList)
        {
            SetMonsterAlly(id);
        }

        JsonSave();
    }

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
        }
        else
        {
            string loadJson = File.ReadAllText(path + FILE_NAME);
            _saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (_saveData != null)
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

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // 도감, 맵, 부대
        //saveData.MonsterCollection = GameManager.Instance.MonsterCollection;
        //saveData.VisitedMaps = MapManager.Instance.VisitCell();
        //_saveData.AllyList = AllyManager.Instance.GetAllyMonsterID();

        string json = JsonUtility.ToJson(_saveData);

        File.WriteAllText(path + FILE_NAME, json);
    }

    public void InitializeData()
    {
        string path = Application.dataPath + PATH;

        SaveData saveData = new SaveData();

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(path + FILE_NAME, json);
    }

}
