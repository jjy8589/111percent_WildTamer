using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : Singleton<ObjectPool>, IInitializableManager
{
    [Header("Objects Prefabs")]
    [SerializeField] private GameObject _monsterPrefab;
    [SerializeField] private GameObject _circleAttackAreaPrefab;

    private const int _monsterSpawn = 128;
    private const int _circleAttackAreaSpawn = 4;

    private Queue<Monster> _monsterQueue = new(_monsterSpawn);
    private Queue<CircleAttackArea> _circleAttackAreaQueue = new(_circleAttackAreaSpawn);

    protected override void Awake()
    {
        base.Awake();
    }

    public void Initialize()
    {
        for (int i = 0; i < _monsterSpawn; i++)
            _monsterQueue.Enqueue(CreateNewMonster()); 
        
        for (int i = 0; i < _circleAttackAreaSpawn; i++)
            _circleAttackAreaQueue.Enqueue(CreateCircleAttackArea());
    }

    private Monster CreateNewMonster()
    {
        var newObj = Instantiate(_monsterPrefab).GetComponent<Monster>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }
    
    private CircleAttackArea CreateCircleAttackArea()
    {
        var newObj = Instantiate(_circleAttackAreaPrefab).GetComponent<CircleAttackArea>();
        newObj.gameObject.SetActive(false);
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
    
    public CircleAttackArea GetCircleAttackAreaObject()
    {
        if (Instance._circleAttackAreaQueue.Count > 0)
        {
            var obj = _circleAttackAreaQueue.Dequeue();
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            var newObj = CreateCircleAttackArea();
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(Monster obj)
    {
        obj.transform.SetParent(this.transform);
        _monsterQueue.Enqueue(obj);
    }
    
    public void ReturnObject(CircleAttackArea obj)
    {
        obj.transform.SetParent(this.transform);
        _circleAttackAreaQueue.Enqueue(obj);
    }
}
