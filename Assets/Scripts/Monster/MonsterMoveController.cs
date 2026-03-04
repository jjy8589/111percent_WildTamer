using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMovementPattern
{
    void Move(Transform self, Transform target);
}

public class StraightMovement : IMovementPattern
{
    private float speed = 1f;
    public void Move(Transform self, Transform target)
    {
        Vector3 dir = (target.position - self.position).normalized;
        self.position += dir * speed * Time.deltaTime;
    }
}

public class DashMovement : IMovementPattern
{
    private float dashSpeed = 3f;
    private float cooldown = 2f;
    private float timer = 0f;

    public void Move(Transform self, Transform target)
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            Vector3 dir = (target.position - self.position).normalized;
            self.position += dir * dashSpeed * Time.deltaTime;
            timer = 0f;
        }
    }
}

public interface IState
{
    void Enter();
    void Update();
    void Exit();
}
public class EnemyIdleState : IState
{
    private MonsterMoveController enemy;
    public EnemyIdleState(MonsterMoveController enemy) { this.enemy = enemy; }

    public void Enter() { }
    public void Update()
    {
        if (enemy.EnemyInRange()) enemy.ChangeState(new EnemyCombatState(enemy));
        else enemy.movementPattern?.Move(enemy.transform, enemy.player);
    }
    public void Exit() { }
}

public class EnemyCombatState : IState
{
    private MonsterMoveController enemy;
    public EnemyCombatState(MonsterMoveController enemy) { this.enemy = enemy; }

    public void Enter() { }
    public void Update()
    {
        if (!enemy.EnemyInRange()) enemy.ChangeState(new EnemyIdleState(enemy));
        else
        {
            // 공격 로직
            Debug.Log("Enemy attacking player!");
        }
    }
    public void Exit() { }
}

// Follow 상태
public class AllyFollowState : IState
{
    private MonsterMoveController ally;
    public AllyFollowState(MonsterMoveController ally) { this.ally = ally; }

    public void Enter() { }
    public void Update()
    {
        Debug.Log("follow");
        if (!ally.PlayerIsMoving())
        {
            if (ally.EnemyInRange())
            {
                ally.ChangeState(new AllyCombatState(ally));
            }
            else
            {
                ally.ChangeState(new AllyIdleState(ally));
            }
        }
        else
        {
            // 플레이어 따라가기
            Vector3 dir = (ally.player.position - ally.transform.position).normalized;
            ally.transform.position += dir * Time.deltaTime;
        }
    }
    public void Exit() { }
}

// Idle 상태
public class AllyIdleState : IState
{
    private MonsterMoveController ally;
    public AllyIdleState(MonsterMoveController ally) { this.ally = ally; }

    public void Enter() { }
    public void Update()
    {
        Debug.Log("AllyIdleState");
        if (ally.EnemyInRange())
        {
            ally.ChangeState(new AllyCombatState(ally));
        }
        else if (ally.PlayerIsMoving())
        {
            ally.ChangeState(new AllyFollowState(ally));
        }
    }
    public void Exit() { }
}

// Combat 상태
public class AllyCombatState : IState
{
    private MonsterMoveController ally;
    public AllyCombatState(MonsterMoveController ally) { this.ally = ally; }

    public void Enter() { }
    public void Update()
    {
        Debug.Log("AllyCombatState");
        if (!ally.EnemyInRange())
        {
            if (ally.PlayerIsMoving())
            {
                ally.ChangeState(new AllyFollowState(ally));
            }
            else
            {
                ally.ChangeState(new AllyIdleState(ally));
            }
        }
        else
        {
            // 공격 로직
            Debug.Log("Ally attacking!");
        }
    }
    public void Exit() { }
}

public class MonsterMoveController : MonoBehaviour
{
    public bool isAlly;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public Transform player;
    public IMovementPattern movementPattern;

    private IState currentState;

    void Start()
    {
        if (isAlly) ChangeState(new AllyFollowState(this));
        else ChangeState(new EnemyIdleState(this));
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public bool EnemyInRange()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var e in enemies)
        {
            if (isAlly && e.CompareTag("Enemy")) return true;
            if (!isAlly && e.CompareTag("Player")) return true;
        }
        return false;
    }

    public bool PlayerIsMoving()
    {
        if (player == null) return false;
        return player.GetComponent<Player>().IsMoving;
    }

    public bool PlayerTooFar(float maxDistance = 5f)
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) > maxDistance;
    }

}
