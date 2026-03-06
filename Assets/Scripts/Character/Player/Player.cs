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
        MovePlayer();
    }

    private void MovePlayer()
    {
        // 조이스틱에서 방향 데이터 가져오기
        Vector3 direction = _joyStick.GetMoveDirection();

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
        _maxHeart = 30f;
        _currentHeart = 30f;
        _moveSpeed = 5f;
        _attackDamage = 3f;
        _attackRange = 3f;
        _attackSpeed = 1f;
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
}
