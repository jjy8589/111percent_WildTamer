using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIMoveCalculator
{
    public static Vector3 MoveToTarget(Vector3 nowPosition, Vector3 targetPosition, float speed)
    {
        Vector3 dir = targetPosition - nowPosition;
        dir.y = 0f;

        dir.Normalize();

        return nowPosition + dir * speed * Time.deltaTime;
    }
}
