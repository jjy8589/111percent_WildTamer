using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected float _maxHeart;
    protected float _currentHeart; // 현재 체력
    protected float _moveSpeed;
    protected float _attackDamage;
    protected float _attackRange;
    protected float _attackSpeed;

    protected LayerMask _allyMask;    // 아군의 layer
    protected LayerMask _enemyMask;   // 적군의 layer

    protected float _lastAttackTime;

    protected const float DETECT_RANGE = 5f;
    [SerializeField] protected Collider _detectTrigger;

    protected virtual void Awake()
    {
        SetLayerAndMask();
    }

    protected abstract void SetLayerAndMask();

    public void Damaged(float amount)
    {
        if (_currentHeart <= 0) return;

        _currentHeart -= amount;

        if (_currentHeart <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();

    public Character FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, DETECT_RANGE, _enemyMask);
        if (hits.Length == 0) return null;

        float minDist = float.MaxValue;
        Character target = null;

        foreach(var hit in hits)
        {
            if (hit.TryGetComponent(out Character character))
            {
                float dist = Vector3.SqrMagnitude(character.transform.position - transform.position);

                if(dist < minDist)
                {
                    target = character;
                }
            }
        }

        return target;
    }

    public bool CanAttack() => Time.time >= _lastAttackTime + _attackSpeed;

    public void Attack(Character target)
    {
        if (!CanAttack()) return;
        if (target == null) return;

        _lastAttackTime = Time.time;
        target.Damaged(_attackDamage);
    }
}
