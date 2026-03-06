using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _bulletSpeed;
    private int _bulletDamage;

    private LayerMask _attackLayer;

    private float _lifeTime = 5f;

    private void OnEnable()
    {
        StartCoroutine(MoveAndExpireRoutine());
    }

    public void Initialize(float bulletSpeed, int damage, LayerMask attackLayer)
    {
        _bulletSpeed = bulletSpeed;
        _bulletDamage = damage;
        _attackLayer = attackLayer;
    }


    private IEnumerator MoveAndExpireRoutine()
    {
        float timer = 0f;

        while (timer < _lifeTime)
        {
            transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        // 시간이 다 되면 오브젝트 비활성화
        Expire();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if((_attackLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if(collision.collider.TryGetComponent(out Character target))
            {
                target.Damaged(_bulletDamage);
            }
        }

        ObjectPool.Instance.ReturnObject(this);
        gameObject.SetActive(false);
    }

    private void Expire()
    {
        ObjectPool.Instance.ReturnObject(this);
        gameObject.SetActive(false);
    }
}
