using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttackArea : MonoBehaviour
{
    private float _maxScale;   // 최종 크기
    private float _duration;   // 차오르는 시간
    private float _timer = 0f;

    private float _damage;
    private LayerMask _attackLayer;

    private void OnEnable()
    {
        _timer = 0f;
        transform.localScale = Vector3.zero; // 시작은 0

        StartCoroutine(FillArea());
    }

    public void Initialize(float maxScale, float duration, float damage, LayerMask attackLayer)
    {
        _maxScale = maxScale;
        _duration = duration;

        _damage = damage;
        _attackLayer = attackLayer;
    }

    private IEnumerator FillArea()
    {
        while (true)
        {
            _timer += Time.deltaTime;
            float ratio = Mathf.Clamp01(_timer / _duration);
            transform.localScale = Vector3.one * Mathf.Lerp(0f, _maxScale, ratio);

            if (ratio >= 0.9f)
            {
                // 공격 판정 실행
                Collider[] hits = Physics.OverlapSphere(transform.position, _maxScale / 2f, _attackLayer);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent(out Character character))
                    {
                        character.Damaged(_damage);
                    }
                }

                ObjectPool.Instance.ReturnObject(this);
                gameObject.SetActive(false);
            }

            yield return null;
        }
    }
}
