using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossPattern
{
    public void Enter();
    public void Update();
    public void Exit();

    public bool IsFinished { get; }
}

public class AreaAttackPattern : IBossPattern
{
    private Monster _monster;
    private float _radius;
    private float _duration;
    private int _damage;

    private float _timer;

    public AreaAttackPattern(Monster monster, float radius, float duration, int damage)
    {
        _monster = monster;
        _radius = radius;
        _duration = duration;
        _damage = damage;
    }

    public bool IsFinished => _timer >= _duration;

    public void Enter()
    {
        _timer = 0;

        var attackArea = ObjectPool.Instance.GetCircleAttackAreaObject();
        attackArea.Initialize(_radius, _duration, _damage, _monster.EnemyMask);
        attackArea.transform.position = _monster.transform.position;
        attackArea.gameObject.SetActive(true);
    }

    public void Exit()
    {
        _timer = 0;
    }

    public void Update()
    {
        _timer += Time.deltaTime;
    }
}

public class SummonPattern : IBossPattern
{
    private Monster _monster;
    private List<MonsterData> _summonMonsterList;
    private int _minSummonCount;
    private int _maxSummonCount;
    private float _summonRadius;

    private bool _isFinished;

    public SummonPattern(Monster monster, List<MonsterData> summonMonsterList, int minSummonCount, int maxSummonCount, float summonRadius)
    {
        _monster = monster;
        _summonMonsterList = new(summonMonsterList);
        _minSummonCount = minSummonCount;
        _maxSummonCount = maxSummonCount;
        _summonRadius = summonRadius;
    }

    public bool IsFinished => _isFinished;

    public void Enter()
    {
        _isFinished = false;

        int summonCount = ChooseSpawnCount();
        for (int i = 0; i < summonCount; i++)
        {
            SpawnMonsterManager.Instance.SpawnMonster(ChooseSpawnPosition(), ChooseSummonMonster());
        }

        _isFinished = true;
    }

    public void Exit()
    {
        _isFinished = false;
    }

    public void Update()
    {
        
    }

    private int ChooseSpawnCount()
    {
        return Random.Range(_minSummonCount, _maxSummonCount + 1);
    }

    private Vector3 ChooseSpawnPosition()
    {
        Vector3 spawnPos = _monster.transform.position + Random.insideUnitSphere * _summonRadius;
        spawnPos.y = 0;

        return spawnPos;
    }

    private MonsterData ChooseSummonMonster()
    {
        int index = Random.Range(0, _summonMonsterList.Count);
        return _summonMonsterList[index];
    }
}