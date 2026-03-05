using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private bool _isLookingPlayer;
    private void LateUpdate()
    {
        transform.position = GameManager.Instance.GetPlayerTransform().position + _offset;
        if (_isLookingPlayer)
        {
            transform.LookAt(GameManager.Instance.GetPlayerTransform());
        }
    }
}
