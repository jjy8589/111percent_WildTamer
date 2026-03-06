using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Slider _heartSlider;
    [SerializeField] protected Image _sliderFillColor;

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

    public float MoveSpeed => _moveSpeed;

    public LayerMask EnemyMask => _enemyMask;

    protected virtual void Awake()
    {
        SetLayerAndMask();
    }

    protected abstract void SetLayerAndMask();

    public void Damaged(float amount)
    {
        if (_currentHeart <= 0) return;

        _currentHeart -= amount;
        _heartSlider.value = _currentHeart;

        if (_currentHeart <= 0)
        {
            _heartSlider.gameObject.SetActive(false);
            Die();
        }
    }

    protected abstract void Die();

    public bool DetectedEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, DETECT_RANGE, _enemyMask);
        
        if (hits.Length == 0) return false;
        return true;
    }

    public Character FindAttackTarget(float range)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, _enemyMask);

        Debug.Log(hits.Length);
        if (hits.Length == 0) return null;

        return FindClosestCharacter(hits);
    }

    private Character FindClosestCharacter(Collider[] hits)
    {
        float minDist = float.MaxValue;
        Character target = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Character character))
            {
                float dist = Vector3.SqrMagnitude(character.transform.position - transform.position);

                if (dist < minDist)
                {
                    target = character;
                }
            }
        }
        Debug.Log(target);

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
