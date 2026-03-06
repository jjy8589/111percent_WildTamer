using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementPattern
{
    public void Move();
    //public void Move(MonsterMoveController monster);
}

public class RandomWalkPattern : IMovementPattern
{
    private Monster _monster;
    private float _changeDirTime = 2f;
    private float _timer;
    private Vector3 _direction;

    public RandomWalkPattern(Monster monster)
    {
        _monster = monster;

        _direction = Random.insideUnitSphere;
        _direction.y = 0;
    }

    public void Move()
    {
        _timer += Time.deltaTime;
        if (_timer > _changeDirTime)
        {
            _direction = Random.insideUnitSphere;
            _direction.y = 0;
            _timer = 0f;
        }

        _monster.transform.position += _direction.normalized * _monster.MoveSpeed * Time.deltaTime;
    }
}

public class DashAndPausePattern : IMovementPattern
{
    private Monster _monster;
    private float _dashDuration = 1f;
    private float _pauseDuration = 1f;
    private float _timer;
    private bool _isDashing;
    private Vector3 _direction;

    public DashAndPausePattern(Monster monster)
    {
        _monster = monster;

        _direction = Random.insideUnitSphere;
        _direction.y = 0;
    }

    public void Move()
    {
        _timer += Time.deltaTime;

        if (_isDashing)
        {
            _monster.transform.position += _direction.normalized * (_monster.MoveSpeed * 2f) * Time.deltaTime;
            if (_timer > _dashDuration)
            {
                _isDashing = false;
                _timer = 0f;
            }
        }
        else
        {
            if (_timer > _pauseDuration)
            {
                _isDashing = true;
                _direction = Random.insideUnitSphere;
                _direction.y = 0;
                _timer = 0f;
            }
        }
    }
}

public class AccelDecelPattern : IMovementPattern
{
    private Monster _monster;
    private float _cycleTime = 3f;
    private float _timer;
    private Vector3 _direction;

    public AccelDecelPattern(Monster monster)
    {
        _monster = monster;
        _direction = Random.insideUnitSphere;
        _direction.y = 0;
    }

    public void Move()
    {
        _timer += Time.deltaTime;
        float t = Mathf.PingPong(_timer, _cycleTime) / _cycleTime; // 0~1 반복
        float speed = Mathf.Lerp(_monster.MoveSpeed * 0.5f, _monster.MoveSpeed * 2f, t);

        _monster.transform.position += _direction.normalized * speed * Time.deltaTime;

        if (_timer > _cycleTime * 2f)
        {
            _direction = Random.insideUnitSphere;
            _direction.y = 0;
            _timer = 0f;
        }
    }
}

public class CircularMovePattern : IMovementPattern
{
    private Monster _monster;
    private Vector3 _center;       // 원의 중심
    private float _radius;         // 반지름
    private float _angle;          // 현재 각도

    public CircularMovePattern(Monster monster)
    {
        _monster = monster;

        _radius = Random.Range(2f, 4f);
        _angle = 0;

        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.y = 0;
        randomDir.Normalize();

        _center = _monster.transform.position + randomDir * _radius;
    }

    public void Move()
    {
        float angularDelta = (_monster.MoveSpeed / _radius) * Time.deltaTime;
        _angle += angularDelta;

        float x = Mathf.Cos(_angle) * _radius;
        float z = Mathf.Sin(_angle) * _radius;

        Vector3 newPos = _center + new Vector3(x, 0, z);
        _monster.transform.position = newPos;

        // 이동 방향으로 회전
        Vector3 dir = newPos - _monster.transform.position;
        if (dir.sqrMagnitude > 0.001f)
        {
            _monster.transform.rotation = Quaternion.Slerp(
                _monster.transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * 5f
            );
        }
    }
}