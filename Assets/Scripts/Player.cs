using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;

    // 내부 변수
    private Vector3 inputDirection;

    [SerializeField] private DynamicJoystick _joyStick;

    private void Awake()
    {
        _joyStick.OnDragJoystick += OnDirectionChanged;
    }

    /// 조이스틱 입력 처리
    void OnDirectionChanged(Vector3 direction)
    {
        inputDirection = direction;
        MovePlayer();
    }

    /// 플레이어 이동
    void MovePlayer()
    {
        Vector3 velocity = inputDirection * moveSpeed;
        Vector3 newPosition = transform.position + velocity * Time.deltaTime;

        transform.position = newPosition;
        transform.rotation = Quaternion.LookRotation(inputDirection);
    }
}
