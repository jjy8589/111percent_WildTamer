using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Data/BossPattern/AreaAttackPatternSO", menuName = "ScriptableObject/AreaAttackPattern")]
public class BulletPatternSO : BossPatternSO
{
    public int MinBulletCount;
    public int MaxBulletCount;
    public float BulletSpeed;
    public float Range;
    public int Damage;

    public override IBossPattern CreatePattern(Monster boss)
    {
        return new BulletPattern(boss, MinBulletCount, MaxBulletCount, BulletSpeed, Range, Damage);
    }
}
