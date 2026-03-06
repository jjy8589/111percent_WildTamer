using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPatternSO : ScriptableObject
{
    public abstract IBossPattern CreatePattern(Monster boss);
}
