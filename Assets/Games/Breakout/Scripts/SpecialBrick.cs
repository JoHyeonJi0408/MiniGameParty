using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBrick : Brick
{
    [HideInInspector] public bool IsFirst = true;

    public virtual void SetFirstLevel() { }

    public virtual void SetSecondLevel() { }

    public override void OnBrickDestroyed()
    {
        if(IsFirst)
        {
            IsFirst = false;
            SetSecondLevel();
        }
        else
        {
            base.OnBrickDestroyed();
        }
    }
}
