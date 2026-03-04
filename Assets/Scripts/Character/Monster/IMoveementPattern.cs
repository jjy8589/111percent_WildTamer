using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementPattern
{
    void Move(Transform self);
}

public class StraightMovement : IMovementPattern
{
    public void Move(Transform self)
    {

    }
}

public class DashMovement : IMovementPattern
{
    public void Move(Transform self)
    {
        
    }
}
