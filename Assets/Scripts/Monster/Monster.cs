using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterData _monsterData;
    [SerializeField] private GameObject _askTamerObject;
    [SerializeField] private Button _askTamerButton;
    [SerializeField] private Collider _triggerRange;

    private int _heart; // 현재 체력

    private bool _isAlly; // 아군인지 여부

    public int Heart
    {
        get => _heart;
        set
        {
            _heart = value;

            if (_heart < 0)
            {
                Die();
            }
        }
    }

    private void Awake()
    {
        _askTamerButton.onClick.AddListener(SuccessTamer);
    }

    private void Die()
    {
        if (_isAlly || !IsAskTamer())
        {
            ObjectPool.Instance.ReturnObject(this);
        }
        else
        {
            _askTamerObject.SetActive(true);
        }
    }

    public bool IsAskTamer()
    {
        float chance = Random.Range(0f, 1f);

        if (chance < _monsterData.TameChance)
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }

    public void SuccessTamer()
    {
        _isAlly = true;
        _askTamerObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
