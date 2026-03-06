using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterBehavior
{
    public void Enter();
    public void Update();
    public void Exit();
}

public class AllyMonsterBehavior : IMonsterBehavior
{
    private Monster _monster;

    public AllyMonsterBehavior(Monster monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (_monster.DetectedEnemy())
        {
            _monster.MoveTowardAndAttack();
        }
        else
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (!GameManager.Instance.IsPlayerMoving()) return;

        Vector3 desiredPos = _monster.Target.position - _monster.Target.right * 1.5f;
        _monster.transform.position = Vector3.Lerp(_monster.transform.position, desiredPos, Time.deltaTime);
    }
}

public class EnemyMonsterBehavior : IMonsterBehavior
{
    private Monster _monster;
    private Vector3 _spawnPosition;
    private IMovementPattern _movePattern;

    public EnemyMonsterBehavior(Monster monster, EnemyMovePattern movePattern)
    {
        _monster = monster;
        _movePattern = SelectMovePattern(movePattern);
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (_monster.DetectedEnemy())
        {
            _monster.MoveTowardAndAttack();
        }

        else
        {
            // TODO : 본인의 자리로 돌아가는 코드 넣기
            _movePattern?.Move();
        }
    }

    private void GoBackToSpawnPosition()
    {
        AIMoveCalculator.MoveToTarget(_monster.transform.position, _spawnPosition, _monster.MoveSpeed);
    }

    private IMovementPattern SelectMovePattern(EnemyMovePattern movePattern)
    {
        switch (movePattern)
        {
            case EnemyMovePattern.Random:
                return new RandomWalkPattern(_monster);
            case EnemyMovePattern.DashAndPause:
                return new DashAndPausePattern(_monster);
            case EnemyMovePattern.AccelDecel:
                return new AccelDecelPattern(_monster);
            case EnemyMovePattern.Circular:
                return new CircularMovePattern(_monster);
            default:
                Debug.Log("정의되지 않은 패턴");
                return null;
        }
    }
}

public class BossMonsterBehavior : IMonsterBehavior
{
    private Monster _monster;

    private List<IBossPattern> _patternList = new();
    private float _patternTimer;

    private float _delayTime;
    private IBossPattern _currentPattern;

    private bool _isStartPattern;

    public BossMonsterBehavior(Monster monster, List<IBossPattern> patternList, float patternDelayTime)
    {
        _monster = monster;
        _patternList = patternList;
        _patternTimer = patternDelayTime;

        _isStartPattern = false;
    }

    public void Enter() 
    {
    }
    public void Exit() { }

    public void Update()
    {
        // 공격 대상이 없으면 그냥 있음
        if (!_monster.DetectedEnemy())
        {
            _patternTimer = 0;
            return;
        }

        // 공격 대상이 있으면 패턴 선택 후 수행
        if (!_isStartPattern)
        {
            _currentPattern = ChoosePattern();
            _currentPattern.Update();
            _isStartPattern = true;
        }

        // 패턴이 종료됐다면 delay
        if (_currentPattern != null && _currentPattern.IsFinished)
        {
            _patternTimer += Time.deltaTime;

            // 정해진 대기 시간 지나면 
            if(_patternTimer > _delayTime)
            {
                _isStartPattern = false;
                _patternTimer = 0;
            }
        }
    }

    private IBossPattern ChoosePattern()
    {
        int index = Random.Range(0, _patternList.Count);

        return _patternList[index];
    }
}
