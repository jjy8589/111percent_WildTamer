using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Data/Monster/MonsterData", menuName = "ScriptableObject/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("Info")]
    public string MonsterId;
    public string MonsterName;

    [Header("Common Stats")]
    public float MaxHeart;
    public float MoveSpeed;
    public float AttackDamage;
    public float AttackRange;
    public float AttackSpeed;

    [Header("Enemy Move Type")]
    public EnemyMovePattern MovePattern;

    [Range(0f, 1f)]
    public float TameChance;
}

