using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Data/Boss/BossData", menuName = "ScriptableObject/BossData")]
public class BossData : MonsterData
{
    [Header("Attaack Pattern")]
    public float PatternDelayTime; // 몇 초 간격으로 패턴 동작하는지
    public List<BossPatternSO> BossPatternSOList;
}
