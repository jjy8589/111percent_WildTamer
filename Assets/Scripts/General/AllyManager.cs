using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : Singleton<AllyManager>
{
    private const int MAX_ALLY_COUNT = 10;

    [SerializeField] private List<Monster> _monsterAllyList = new(MAX_ALLY_COUNT);

    public bool IsEnemyNear = false;

    public List<Monster> MonsterAllyList => _monsterAllyList;

    private void Update()
    {
        UpdateBoid();
    }

    public bool IsMaxAlly()
    {
        return MAX_ALLY_COUNT <= _monsterAllyList.Count;
    }

    public void AddMonsterAlly(Monster monster)
    {
        if (IsMaxAlly()) return;

        _monsterAllyList.Add(monster);

        DataManager.Instance.UpdateAllyList(GetAllyMonsterID());
    }

    public void RemoveMonsterAlly(Monster monster)
    {
        _monsterAllyList.Remove(monster);

        DataManager.Instance.UpdateAllyList(GetAllyMonsterID());
    }

    private void UpdateBoid()
    {
        for (int i = 0; i < _monsterAllyList.Count; i++)
        {
            Monster monster = _monsterAllyList[i];
            Vector3 boid = AIMoveCalculator.CalcBoid(monster, _monsterAllyList);

            monster.BoidForce = boid;
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
