using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Vector3 _inputDirection;
    private bool _isMoving;

    [SerializeField] private DynamicJoystick _joyStick;

    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    protected override void Awake()
    {
        base.Awake();

        _joyStick.OnDragJoystick += OnDirectionChanged;
        _joyStick.OnEndDragJoystick += () => _isMoving = false;

        InitializeStat();
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
        _attackDamage = 0f;
        _attackRange = 1f;
        _attackSpeed = 1f;
    }

    /// 조이스틱 입력 처리
    void OnDirectionChanged(Vector3 direction)
    {
        _inputDirection = direction;
        MovePlayer();
    }

    /// 플레이어 이동
    void MovePlayer()
    {
        Vector3 newPosition = transform.position + _inputDirection * Time.deltaTime * _moveSpeed;

        transform.position = newPosition;
        transform.rotation = Quaternion.LookRotation(_inputDirection);

        _isMoving = true;
    }

    //private Vector2 moveInput; 
    //void Update()
    //{
    //    // 조이스틱 또는 키보드 입력 (모바일이면 조이스틱 컴포넌트로 교체)
    //    moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),
    //                            Input.GetAxisRaw("Vertical")).normalized;
    //}

    //void FixedUpdate()
    //{
    //    var rb = GetComponent<Rigidbody>();
    //    rb.MovePosition(rb.position + (Vector3)moveInput * _moveSpeed * Time.fixedDeltaTime);
    //}

    protected override void Die()
    {
        Debug.Log("플레이어 죽음");
    }

    private IEnumerator CheckAttackPosible()
    {
        while (true)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, DETECT_RANGE, _enemyMask);
            if (hits.Length == 0)
            {
                AllyManager.Instance.IsEnemyNear = false;
            }
            else
            {
                AllyManager.Instance.IsEnemyNear = true;
            }

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
