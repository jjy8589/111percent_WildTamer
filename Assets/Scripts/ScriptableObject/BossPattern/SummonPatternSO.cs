using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Data/BossPattern/SummonPatternSO", menuName = "ScriptableObject/SummonPattern")]
public class SummonPatternSO : BossPatternSO
{
    public List<MonsterData> SummonMonsterList;
    public int MinSummonCount;
    public int MaxSummonCount;
    public float SummonRadius; // 현재 위치에서 얼마나 떨어진 곳까지 소환 가능한지

    public override IBossPattern CreatePattern(Monster boss)
    {
        return new SummonPattern(boss, SummonMonsterList, MinSummonCount, MaxSummonCount, SummonRadius);
    }
}
