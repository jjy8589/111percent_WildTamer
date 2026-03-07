using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionData
{
    public string MonsterID;
    public string MonsterName;
    public float MaxHeart;

    public bool IsFound; // 마주친 적 있는지
    public bool HasBeenAlly; // 동료가 된 적 있는지

    public CollectionData(string id, string name, float heart, bool isFound, bool hasBeenAlly)
    {
        MonsterID = id;
        MonsterName = name;
        MaxHeart = heart;
        IsFound = isFound;
        HasBeenAlly = hasBeenAlly;
    }
}
