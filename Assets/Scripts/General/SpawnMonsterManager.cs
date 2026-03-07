using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossMonsterSpawner
{
    public Vector3 SpawnPosition;
    public MonsterData BossData;
}

public class SpawnMonsterManager : Singleton<SpawnMonsterManager>
{
    [SerializeField] private List<MonsterData> _monsterDatas;
    [SerializeField] private List<BossMonsterSpawner> _bossMonsterSpawner;

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            int index = UnityEngine.Random.Range(0, _monsterDatas.Count);
            SpawnMonster(MapManager.Instance.GetRandomPosition(), _monsterDatas[index]);
        }

        for(int i = 0; i < _bossMonsterSpawner.Count; i++)
        {
            SpawnBoss(_bossMonsterSpawner[i].SpawnPosition, _bossMonsterSpawner[i].BossData);
        }

        SpawnSavedAllies();
    }

    private void SpawnSavedAllies()
    {
        List<string> allyIds = DataManager.Instance.GetSavedAllyList();

        foreach (var id in allyIds)
        {
            MonsterData data = DataManager.Instance.GetMonsterData(id);

            if (data != null)
            {
                Vector3 offset = UnityEngine.Random.insideUnitSphere * 2f;
                Vector3 spawnPos = GameManager.Instance.GetPlayerTransform().position + offset;
                
                SpawnAlly(spawnPos, data);
            }
        }
    }

    public void SpawnMonster(Vector3 pos, MonsterData monsterData)
    {
        Monster monster = ObjectPool.Instance.GetEnemyObject();
        monster.transform.position = pos + Vector3.up;
        monster.gameObject.SetActive(true);
        monster.SetMonsterData(monsterData);
        monster.SetMonsterTeam(MonsterType.Enemy);
    }
    
    public void SpawnBoss(Vector3 pos, MonsterData monsterData)
    {
        Monster monster = ObjectPool.Instance.GetEnemyObject();
        monster.transform.position = pos + Vector3.up;
        monster.gameObject.SetActive(true);
        monster.SetMonsterData(monsterData);
        monster.SetMonsterTeam(MonsterType.Boss);
    }

    private void SpawnAlly(Vector3 pos, MonsterData monsterData)
    {
        Monster monster = ObjectPool.Instance.GetEnemyObject();
        monster.transform.position = pos + Vector3.up;
        monster.gameObject.SetActive(true);
        monster.SetMonsterData(monsterData);
        monster.SetMonsterTeam(MonsterType.Ally);

        AllyManager.Instance.AddMonsterAlly(monster);
    }
}
