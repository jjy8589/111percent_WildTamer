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
    private MonsterMoveController _moveController;

    public AllyMonsterBehavior(Monster monster, MonsterMoveController moveController)
    {
        _monster = monster;
        _moveController = moveController;

        _moveController.ChangeState(new AllyIdleState(_monster, _moveController));
    }

    public void Enter()
    {
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}

public class EnemyMonsterBehavior : IMonsterBehavior
{
    private Monster _monster;
    private MonsterMoveController _moveController;

    public EnemyMonsterBehavior(Monster monster, MonsterMoveController moveController)
    {
        _monster = monster;
        _moveController = moveController;

        _moveController.ChangeState(new EnemyIdleState(_monster, _moveController));
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
