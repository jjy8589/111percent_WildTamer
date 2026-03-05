using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsterManager : Singleton<SpawnMonsterManager>
{
    [SerializeField] private List<MonsterData> _monsterDatas;
    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 2f;
            Vector3 spawnPos = GameManager.Instance.GetPlayerTransform().position + offset;
            SpawnAllys(new Vector3(spawnPos.x, 0.25f, spawnPos.z));   
        }

        SpawnMonsters(new Vector3(22.54f, 0.25f, 0));
    }

    private void SpawnMonsters(Vector3 pos)
    {
        Monster monster = ObjectPool.Instance.GetEnemyObject();
        monster.transform.position = pos;
        monster.gameObject.SetActive(true);
        monster.SetMonsterData(_monsterDatas[0]);
        monster.SetMonsterTeam(MonsterTeam.Enemy);
    }

    private void SpawnAllys(Vector3 pos)
    {
        Monster monster = ObjectPool.Instance.GetEnemyObject();
        monster.transform.position = pos;
        monster.gameObject.SetActive(true);
        monster.SetMonsterData(_monsterDatas[0]);
        monster.SetMonsterTeam(MonsterTeam.Ally);

        AllyManager.Instance.AddMonsterAlly(monster);
    }
}
