using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterTeam
{
    Ally,
    Enemy,
}

public class Monster : Character
{
    [SerializeField] private GameObject _askTamerObject;
    [SerializeField] private Button _askTamerButton;
    [SerializeField] private Collider _triggerRange;

    [SerializeField] private MonsterData _monsterData;
    private float _detectionRange;

    private MonsterTeam _monsterTeam;
    private IMonsterBehavior _monsterBehavior;
    private MonsterMoveController _moveController;

    private Transform _target;

    public Transform Target { get => _target; set => _target = value; }

    protected override void Awake()
    {
        base.Awake();

        _askTamerButton.onClick.AddListener(SuccessTamer);
        _moveController = GetComponent<MonsterMoveController>();
    }

    public void SetMonsterData(MonsterData monsterData)
    {
        _monsterData = monsterData;
        
        InitializeMonsterStat();
    }

    private void InitializeMonsterStat()
    {
        _maxHeart = _monsterData.MaxHeart;
        _currentHeart = _monsterData.MaxHeart;
        _moveSpeed = _monsterData.MoveSpeed;
        _attackDamage = _monsterData.AttackDamage;
        _attackRange = _monsterData.AttackRange;
        _attackSpeed = _monsterData.AttackSpeed;
        _detectionRange = _monsterData.DetectionRange;
    }

    public void SetMonsterTeam(MonsterTeam team)
    {
        _monsterTeam = team;

        switch (team)
        {
            case MonsterTeam.Ally:
                _monsterBehavior = new AllyMonsterBehavior(this, _moveController);
                break;
            case MonsterTeam.Enemy:
                _monsterBehavior = new EnemyMonsterBehavior(this, _moveController);
                break;
        }

        SetLayerAndMask();
    }

    protected override void SetLayerAndMask()
    {
        switch (_monsterTeam)
        {
            case MonsterTeam.Ally:
                gameObject.layer = LayerMask.NameToLayer("Ally");
                _allyMask = LayerMask.GetMask("Ally");
                _enemyMask = LayerMask.GetMask("Enemy");
                break;
            case MonsterTeam.Enemy:
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                _allyMask = LayerMask.GetMask("Enemy");
                _enemyMask = LayerMask.GetMask("Ally");
                break;
        }
    }

    protected override void Die()
    {
        if (_monsterTeam.Equals(MonsterTeam.Enemy) && !AllyManager.Instance.IsMaxAlly() && IsAskTamer())
        {
            _askTamerObject.SetActive(true);
        }
        else
        {
            AllyManager.Instance.RemoveMonsterAlly(this);
            ObjectPool.Instance.ReturnObject(this);
            gameObject.SetActive(false);
        }
    }

    public bool IsEnemyDetected()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRange, _enemyMask);
        return hits.Length > 0;
    }


    public bool IsAskTamer()
    {
        float chance = Random.Range(0f, 1f);

        if (chance < _monsterData.TameChance)
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }

    public void SuccessTamer()
    {
        SetMonsterTeam(MonsterTeam.Ally);
        _askTamerObject.SetActive(false);

        _currentHeart = _maxHeart;

        AllyManager.Instance.AddMonsterAlly(this);
    }

    public void EngageEnemy()
    {
        Character enemy = FindClosestEnemy();

        if (enemy == null) return;

        Vector3 dir = enemy.transform.position - transform.position;
        dir.y = 0f;

        float dist = dir.magnitude;

        if (dist > _attackRange)
        {
            // 적 방향으로 이동
            dir.Normalize();
            transform.position += dir * (_moveSpeed * Time.deltaTime);
        }

        // 적 방향 회전
        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * 10f
            );
        }

        // 사거리 안이면 공격
        if (dist <= _attackRange)
        {
            Attack(enemy);
        }
    }
}
