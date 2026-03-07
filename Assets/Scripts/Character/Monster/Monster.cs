using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Monster : Character
{
    [SerializeField] private Button _askTamerButton;

    [SerializeField] private MonsterData _monsterData;
    private float _detectionRange;

    private MonsterType _monsterTeam;
    private IMonsterBehavior _monsterBehavior;
    private MonsterMoveController _moveController;

    private Transform _target;

    public Transform Target { get => _target; set => _target = value; }
    public string MonsterId => _monsterData.MonsterId;

    protected override void Awake()
    {
        base.Awake();

        _askTamerButton.onClick.AddListener(SuccessTamer);
        _moveController = GetComponent<MonsterMoveController>();
    }

    private void LateUpdate()
    {
        if(IsAlive)
        {
            _monsterBehavior?.Update();
        }
    }

    public void SetMonsterData(MonsterData monsterData)
    {
        _monsterData = monsterData;
        
        InitializeMonsterStat();

        _heartSlider.maxValue = _maxHeart;
        _heartSlider.value = _currentHeart;
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

    public void SetMonsterTeam(MonsterType team)
    {
        _monsterTeam = team;

        switch (team)
        {
            case MonsterType.Ally:
                _monsterBehavior = new AllyMonsterBehavior(this);
                _sliderFillColor.color = Color.green;
                break;

            case MonsterType.Enemy:
                _monsterBehavior = new EnemyMonsterBehavior(this, _monsterData.MovePattern);
                _sliderFillColor.color = Color.red;
                break;

            case MonsterType.Boss:
                BossData boss = _monsterData as BossData;

                List<IBossPattern> bossPattern = new();
                foreach(var pattern in boss.BossPatternSOList)
                {
                    bossPattern.Add(pattern.CreatePattern(this));
                }

                _monsterBehavior = new BossMonsterBehavior(this, bossPattern, boss.PatternDelayTime);
                _sliderFillColor.color = Color.red;
                break;
        }

        SetLayerAndMask();
    }

    protected override void SetLayerAndMask()
    {
        switch (_monsterTeam)
        {
            case MonsterType.Ally:
                gameObject.layer = LayerMask.NameToLayer("Ally");
                _allyMask = LayerMask.GetMask("Ally");
                _enemyMask = LayerMask.GetMask("Enemy");
                break;
            case MonsterType.Enemy:
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                _allyMask = LayerMask.GetMask("Enemy");
                _enemyMask = LayerMask.GetMask("Ally");
                break;
            case MonsterType.Boss:
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                _allyMask = LayerMask.GetMask("Enemy");
                _enemyMask = LayerMask.GetMask("Ally");
                break;
        }
    }

    protected override void Die()
    {
        if (_monsterTeam.Equals(MonsterType.Enemy) && !AllyManager.Instance.IsMaxAlly() && IsAskTamer())
        {
            _askTamerButton.gameObject.SetActive(true);
        }
        else
        {
            AllyManager.Instance.RemoveMonsterAlly(this);
            ObjectPool.Instance.ReturnObject(this);
            gameObject.SetActive(false);
        }
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
        SetMonsterTeam(MonsterType.Ally);
        _askTamerButton.gameObject.SetActive(false);

        _currentHeart = _maxHeart;
        _heartSlider.value = _currentHeart;
        _heartSlider.gameObject.SetActive(true);

        AllyManager.Instance.AddMonsterAlly(this);
    }

    public void MoveTowardAndAttack()
    {
        if (!DetectedEnemy()) return;

        Character enemy = FindAttackTarget(DETECT_RANGE);
        Vector3 dir = enemy.transform.position - transform.position;
        dir.y = 0f;

        float dist = dir.magnitude;

        // 적 방향 회전
        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * 10f
            );
        }

        if (dist > _attackRange) // 사거리 밖이면 이동
        {
            dir.Normalize();
            
            var newPosition = transform.position + dir * (_moveSpeed * Time.deltaTime);
            if (CheckAvailableTile(newPosition))
            {
                transform.position = newPosition;
            }
        }
        else // 사거리 안이면 공격
        {
            Attack(enemy);
        }
    }

    private bool CheckAvailableTile(Vector3 newPosition)
    {
        return MapManager.Instance.IsInGround(newPosition);
    }
}
