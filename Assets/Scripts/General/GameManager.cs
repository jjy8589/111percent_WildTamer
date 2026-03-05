using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializableManager
{
    public void Initialize();
}

public class GameManager : Singleton<GameManager>, IInitializableManager
{
    [SerializeField] private List<GameObject> _managerList;
    
    // TODO : PlayerManager 만들어서 빼기
    [SerializeField] private Player _player;

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    public void Initialize()
    {
        InitializeAllManagers();
    }

    private void InitializeAllManagers()
    {
        foreach (var prefab in _managerList)
        {
            var instance = Instantiate(prefab);

            if (instance.TryGetComponent(out IInitializableManager initializableManager))
            {
                initializableManager.Initialize();
            }
        }
    }

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
