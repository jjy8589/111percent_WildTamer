using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : Singleton<ObjectPool>
{
    [Header("Objects Prefabs")]
    [SerializeField] private GameObject _monsterPrefab;

    private const int _monsterSpawn = 128;

    private Queue<Monster> _monsterQueue = new(128);

    protected override void Awake()
    {
        base.Awake();

        Initialized();
    }

    private void Initialized()
    {
        for (int i = 0; i < _monsterSpawn; i++)
            _monsterQueue.Enqueue(CreateNewMonster());

    }

    private Monster CreateNewMonster()
    {
        var newObj = Instantiate(_monsterPrefab).GetComponent<Monster>();
        newObj.gameObject.SetActive(false);
        newObj.enabled = false;
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public Monster GetEnemyObject()
    {
        if (Instance._monsterQueue.Count > 0)
        {
            var obj = _monsterQueue.Dequeue();
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            var newObj = CreateNewMonster();
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(Monster obj)
    {
        obj.transform.SetParent(this.transform);
        _monsterQueue.Enqueue(obj);
    }
}
