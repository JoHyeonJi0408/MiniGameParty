using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBrick : SpecialBrick
{
    public override void SetFirstLevel()
    {
        
    }

    public override void SetSecondLevel()
    {
        subSprite.gameObject.SetActive(false);
    }
}
