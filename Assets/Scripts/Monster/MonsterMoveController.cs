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
        enemy.movementPattern?.Move(enemy.transform, enemy.player);
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
            // 공격 로직
            Debug.Log("Enemy attacking player!");
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

            // 플레이어 따라가기
            Vector3 dir = (ally.player.position - ally.transform.position).normalized;
            ally.transform.position += dir * Time.deltaTime;
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

    //public bool EnemyInRange()
    //{
    //    Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange);
    //    foreach (var e in enemies)
    //    {
    //        if (isAlly && e.CompareTag("Enemy")) return true;
    //        if (!isAlly && e.CompareTag("Player")) return true;
    //    }
    //    return false;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (isAlly)
        {
            // 아군이면 적군 감지
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("적군 감지 → Combat 상태 전환");
                ChangeState(new AllyCombatState(this));
            }
        }
        else
        {
            // 적군이면 플레이어 감지
            if (other.CompareTag("Player"))
            {
                Debug.Log("플레이어 감지 → Combat 상태 전환");
                ChangeState(new EnemyCombatState(this));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isAlly && other.CompareTag("Enemy"))
        {
            Debug.Log("적군 사라짐 → Idle/Follow 상태 전환");
            ChangeState(new AllyIdleState(this));
        }
        if (!isAlly && other.CompareTag("Player"))
        {
            Debug.Log("플레이어 사라짐 → Patrol 상태 전환");
            ChangeState(new EnemyIdleState(this));
        }
    }


    public bool PlayerIsMoving()
    {
        if (player == null) return false;
        return player.GetComponent<Player>().IsMoving;
    }
}
