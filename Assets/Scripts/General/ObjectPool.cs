using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : Singleton<ObjectPool>, IInitializableManager
{
    [Header("Objects Prefabs")]
    [SerializeField] private GameObject _monsterPrefab;
    [SerializeField] private GameObject _bulletPrefab;

    private const int _monsterSpawn = 128;
    private const int _bulletSpawn = 16;

    private Queue<Monster> _monsterQueue = new(_monsterSpawn);
    private Queue<Bullet> _bulletQueue = new(_bulletSpawn);

    protected override void Awake()
    {
        base.Awake();
    }

    public void Initialize()
    {
        for (int i = 0; i < _monsterSpawn; i++)
            _monsterQueue.Enqueue(CreateNewMonster()); 
        
        for (int i = 0; i < _bulletSpawn; i++)
            _bulletQueue.Enqueue(CreateBullet());
    }

    private Monster CreateNewMonster()
    {
        var newObj = Instantiate(_monsterPrefab).GetComponent<Monster>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }
    
    private Bullet CreateBullet()
    {
        var newObj = Instantiate(_bulletPrefab).GetComponent<Bullet>();
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
    
    public Bullet GetBulletObject()
    {
        if (Instance._bulletQueue.Count > 0)
        {
            var obj = _bulletQueue.Dequeue();
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            var newObj = CreateBullet();
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(Monster obj)
    {
        obj.transform.SetParent(this.transform);
        _monsterQueue.Enqueue(obj);
    }
    
    public void ReturnObject(Bullet obj)
    {
        obj.transform.SetParent(this.transform);
        _bulletQueue.Enqueue(obj);
    }
}
