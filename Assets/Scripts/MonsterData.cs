using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Data/Monster/MonsterData", menuName = "ScriptableObject/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("Info")]
    public string MonsterId;
    public string MonsterName;

    [Header("Stats")]
    public float MaxHp;
    public float MoveSpeed;
    public float AttackDamage;
    public float AttackRange;
    public float AttackSpeed;

    [Header("Wild Stats")]
    public float DetectionRange;

    [Range(0f, 1f)]
    public float TameChance;
}

