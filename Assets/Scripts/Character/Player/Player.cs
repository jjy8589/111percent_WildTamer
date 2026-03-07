using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //private Vector3 _inputDirection;
    private bool _isMoving;

    [SerializeField] private DynamicJoystick _joyStick;

    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    protected override void Awake()
    {
        base.Awake();

        _joyStick.OnStartDragJoystick += () => _isMoving = true;
        _joyStick.OnEndDragJoystick += () => _isMoving = false;

        InitializeStat();

        _heartSlider.maxValue = _maxHeart;
        _heartSlider.value = _currentHeart;
        _sliderFillColor.color = Color.blue;
    }

    private void Update()
    {
        if (IsAlive)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        // 조이스틱에서 방향 데이터 가져오기
        Vector3 direction = _joyStick.GetMoveDirection();

        if (direction.magnitude <= 0.01f)
            return;

        // 이동 및 회전
        transform.position += direction * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Start()
    {
        StartCoroutine(CheckAttackPosible());
    }

    private void InitializeStat()
    {
        _maxHeart = PlayerConfig.MAX_HEART;
        _currentHeart = PlayerConfig.MAX_HEART;
        _moveSpeed = PlayerConfig.MOVE_SPEED;
        _attackDamage = PlayerConfig.ATTACK_DAMAGE;
        _attackRange = PlayerConfig.ATTACK_RANGE;
        _attackSpeed = PlayerConfig.ATTACK_SPEED;
    }

    protected override void Die()
    {
        Debug.Log("플레이어 죽음");
    }

    private IEnumerator CheckAttackPosible()
    {
        while (true)
        {
            AllyManager.Instance.IsEnemyNear = DetectedEnemy();
            Attack(FindAttackTarget(_attackRange));

            yield return new WaitForSeconds(0.3f);
        }
    }

    protected override void SetLayerAndMask()
    {
        gameObject.layer = LayerMask.NameToLayer("Ally");
        _allyMask = LayerMask.GetMask("Ally");
        _enemyMask = LayerMask.GetMask("Enemy");
    }

    public override void Attack(Character target)
    {
        base.Attack(target);

        if(target is Monster monster)
        {
            DataManager.Instance.SetMonsterFound(monster.MonsterId);
        }
    }
}
