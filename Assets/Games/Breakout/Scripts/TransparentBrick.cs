using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentBrick : SpecialBrick
{
    public Sprite BrickImage;
    public Collider2D BrickCollider;

    public override void SetSecondLevel()
    {
        mainSprite.sprite = BrickImage;
        BrickCollider.isTrigger = false;
        IsFirst = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetSecondLevel();
    }
}
