using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIMoveCalculator
{
    public static Vector3 CalculateNormalizedDir(Vector3 nowPosition, Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - nowPosition;
        dir.y = 0f;

        return dir.normalized;
    }

    public static Vector3 MoveToTarget(Vector3 nowPosition, Vector3 targetPosition, float speed)
    {
        Vector3 dir = CalculateNormalizedDir(nowPosition, targetPosition);

        return nowPosition + dir.normalized * speed * Time.deltaTime;
    }

    public static Vector3 CalcBoid(Monster self, List<Monster> allyMonsterList)
    {
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        int count = 0;

        foreach (var ally in allyMonsterList)
        {
            if (ally == self) continue;

            Vector3 diff = self.transform.position - ally.transform.position;
            float dist = diff.magnitude;

            if (dist < BoidConfig.SEPARATION_RADIUS && dist > 0.001f)
            {
                separation += diff.normalized / dist;
            }

            alignment += ally.transform.forward;
            cohesion += ally.transform.position;
            count++;
        }

        if (count == 0)
        {
            return Vector3.zero;
        }
        
        alignment = (alignment / count).normalized;
        cohesion = ((cohesion / count) - self.transform.position).normalized;

        return separation * BoidConfig.SEPARATION_WEIGHT
             + alignment * BoidConfig.ALIGNMENT_WEIGHT
             + cohesion * BoidConfig.COHESION_WEIGHT;
    }
}
