using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Data/BossPattern/AreaAttackPatternSO", menuName = "ScriptableObject/AreaAttackPattern")]
public class AreaAttackPatternSO : BossPatternSO
{
    public float Radius;
    public float Duration;
    public int Damage;

    public override IBossPattern CreatePattern(Monster boss)
    {
        return new AreaAttackPattern(boss, Radius, Duration, Damage);
    }
}
