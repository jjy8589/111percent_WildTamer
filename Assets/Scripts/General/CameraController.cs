using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    private void LateUpdate()
    {
        transform.position = GameManager.Instance.GetPlayerTransform().position + _offset;
        transform.LookAt(GameManager.Instance.GetPlayerTransform());
    }
}
