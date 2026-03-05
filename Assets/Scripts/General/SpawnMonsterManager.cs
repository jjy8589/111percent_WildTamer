using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossMonsterSpawner
{
    public Transform SpawnTransform;
    public MonsterData BossData;
}

public class SpawnMonsterManager : Singleton<SpawnMonsterManager>
{
    [SerializeField] private List<MonsterData> _monsterDatas;
    [SerializeField] private List<BossMonsterSpawner> _bossMonsterSpawner;

    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            int index = UnityEngine.Random.Range(0, _monsterDatas.Count - 1);
            SpawnMonsters(MapManager.Instance.GetRandomPosition(), _monsterDatas[index]);
        }

        for(int i = 0; i < _bossMonsterSpawner.Count; i++)
        {
            SpawnMonsters(_bossMonsterSpawner[i].SpawnTransform.position, _bossMonsterSpawner[i].BossData);
        }
    }

    private void SpawnMonsters(Vector3 pos, MonsterData monsterData)
    {
        Monster monster = ObjectPool.Instance.GetEnemyObject();
        monster.transform.position = pos + Vector3.up;
        monster.gameObject.SetActive(true);
        monster.SetMonsterData(monsterData);
        monster.SetMonsterTeam(MonsterTeam.Enemy);
    }

    private void SpawnAllys(Vector3 pos)
    {
        Monster monster = ObjectPool.Instance.GetEnemyObject();
        monster.transform.position = pos + Vector3.up;
        monster.gameObject.SetActive(true);
        monster.SetMonsterData(_monsterDatas[0]);
        monster.SetMonsterTeam(MonsterTeam.Ally);

        AllyManager.Instance.AddMonsterAlly(monster);
    }
}
