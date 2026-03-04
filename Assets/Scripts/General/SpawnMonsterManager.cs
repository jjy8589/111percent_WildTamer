using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsterManager : Singleton<SpawnMonsterManager>
{
    private void SpawnMonsters()
    {
        Monster monster = ObjectPool.Instance.GetEnemyObject();
    }
}
