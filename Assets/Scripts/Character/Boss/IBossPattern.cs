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

public class BulletPattern : IBossPattern
{
    private Monster _monster;

    private int _minBulletCount;
    private int _maxBulletCount;
    private float _bulletSpeed; 
    private float _range;
    private int _damage;

    private bool _isFinished;

    public BulletPattern(Monster boss, int minBulletCount, int maxBulletCount, float bulletSpeed, float range, int damage)
    {
        _monster = boss;
        _minBulletCount = minBulletCount;
        _maxBulletCount = maxBulletCount;
        _bulletSpeed = bulletSpeed;
        _range = range;
        _damage = damage;
    }

    public bool IsFinished => _isFinished;

    public void Enter()
    {
        // 부채꼴로 발사하기
        int bulletCount = ChooseBulletCount();

        Vector3 targetDir = (GameManager.Instance.GetPlayerTransform().position - _monster.transform.position).normalized;
        targetDir.y = 0; // 수평 발사 고정

        Quaternion centerRotation = Quaternion.LookRotation(targetDir);

        for (int i = 0; i < bulletCount; i++)
        {
            float randomAngle = Random.Range(-_range * 0.5f, _range * 0.5f);

            // 중앙 회전값에 무작위 각도를 더함
            Quaternion finalRotation = centerRotation * Quaternion.Euler(0, randomAngle, 0);

            // 총알 생성 위치 (보스 가슴 높이)
            Vector3 spawnPos = _monster.transform.position;

            // 총알 생성
            var bullet = ObjectPool.Instance.GetBulletObject();
            bullet.transform.rotation = finalRotation;
            bullet.transform.position = spawnPos;
            bullet.Initialize(_bulletSpeed, _damage, _monster.EnemyMask);
            bullet.gameObject.SetActive(true);
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

    private int ChooseBulletCount()
    {
        return Random.Range(_minBulletCount, _maxBulletCount + 1);
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
