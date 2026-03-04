using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player _player;


    public bool IsPlayerMoving()
    {
        if (_player == null) return false;

        return _player.IsMoving;
    }

    public Transform GetPlayerTransform()
    {
        if (_player == null) return null;

        return _player.transform;
    }
}
