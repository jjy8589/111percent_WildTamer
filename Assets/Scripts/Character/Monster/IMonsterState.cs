using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    void Enter();
    void Update();
    void Exit();
}
public class EnemyIdleState : IMonsterState
{
    private Monster _monster;
    private MonsterMoveController _moveController;

    public EnemyIdleState(Monster monster, MonsterMoveController moveController)
    {
        _monster = monster;
        _moveController = moveController;
    }
    public void Enter() { }
    public void Update()
    {

    }
    public void Exit() { }
}

public class EnemyAttackState : IMonsterState
{
    private Monster _monster;
    private MonsterMoveController _moveController;

    public EnemyAttackState(Monster monster, MonsterMoveController moveController)
    {
        _monster = monster;
        _moveController = moveController;
    }

    public void Enter() { }
    public void Update()
    {
        // 공격 로직
        Debug.Log("Enemy attacking player!");

        _monster.EngageEnemy();
    }
    public void Exit() { }
}

// Idle 상태
public class AllyIdleState : IMonsterState
{
    private Monster _monster;
    private MonsterMoveController _moveController;
    
    public AllyIdleState(Monster monster, MonsterMoveController moveController) 
    {
        _monster = monster;
        _moveController = moveController;
    }

    public void Enter() { }
    public void Update()
    {
        Debug.Log("AllyIdleState");

        if (AllyManager.Instance.IsEnemyNear)
        {
            _moveController.ChangeState(new AllyAttackState(_monster, _moveController));
        }

        if (GameManager.Instance.IsPlayerMoving())
        {
            _moveController.ChangeState(new AllyMoveState(_monster, _moveController));
        }
    }
    public void Exit() { }
}

// Move 상태
public class AllyMoveState : IMonsterState
{
    private Monster _monster;
    private MonsterMoveController _moveController;

    public AllyMoveState(Monster monster, MonsterMoveController moveController)
    {
        _monster = monster;
        _moveController = moveController;
    }

    public void Enter() { }
    public void Update()
    {
        Debug.Log("AllyMoveState");

        Vector3 desiredPos = _monster.Target.position - _monster.Target.right * 1.5f;
        _monster.transform.position = Vector3.Lerp(_monster.transform.position, desiredPos, Time.deltaTime);

        if (AllyManager.Instance.IsEnemyNear)
        {
            _moveController.ChangeState(new AllyAttackState(_monster, _moveController));
        }

        if (!GameManager.Instance.IsPlayerMoving())
        {
            _moveController.ChangeState(new AllyIdleState(_monster, _moveController));
        }
    }
    public void Exit() { }
}

// Attack 상태
public class AllyAttackState : IMonsterState
{
    private Monster _monster;
    private MonsterMoveController _moveController;

    public AllyAttackState(Monster monster, MonsterMoveController moveController)
    {
        _monster = monster;
        _moveController = moveController;
    }

    public void Enter() { }
    public void Update()
    {
        Debug.Log("AllyAttackState");

        if (!AllyManager.Instance.IsEnemyNear)
        {
            _moveController.ChangeState(new AllyIdleState(_monster, _moveController));
        }

        _monster.EngageEnemy();
    }
    public void Exit() { }
}
