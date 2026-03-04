using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveController : MonoBehaviour
{
    private IMonsterState currentState;

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IMonsterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
