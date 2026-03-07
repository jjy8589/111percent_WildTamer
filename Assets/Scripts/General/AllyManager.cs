using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : Singleton<AllyManager>
{
    private const int MAX_ALLY_COUNT = 10;

    [SerializeField] private List<Monster> _monsterAllyList = new(MAX_ALLY_COUNT);

    public bool IsEnemyNear = false;

    private void Start()
    {
        UpdateTarget();
    }

    public bool IsMaxAlly()
    {
        return MAX_ALLY_COUNT <= _monsterAllyList.Count;
    }

    public void AddMonsterAlly(Monster monster)
    {
        if (IsMaxAlly()) return;

        _monsterAllyList.Add(monster);
        UpdateTarget();

        DataManager.Instance.UpdateAllyList(GetAllyMonsterID());
    }

    public void RemoveMonsterAlly(Monster monster)
    {
        _monsterAllyList.Remove(monster);
        UpdateTarget();

        DataManager.Instance.UpdateAllyList(GetAllyMonsterID());
    }

    private void UpdateTarget()
    {
        for(int i = 0; i < _monsterAllyList.Count; i++)
        {
            _monsterAllyList[i].Target = (i == 0) ? GameManager.Instance.GetPlayerTransform() : _monsterAllyList[i - 1].transform;
        }
    }

    public List<string> GetAllyMonsterID()
    {
        List<string> idList = new();

        foreach (var value in _monsterAllyList)
        {
            idList.Add(value.MonsterId);
        }

        return idList;
    }
}
