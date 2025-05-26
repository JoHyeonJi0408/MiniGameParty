using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public SpriteRenderer mainSprite;
    public SpriteRenderer subSprite;

    public virtual void SetColors(Color32 mainColor, Color32 subColor)
    {
        mainSprite.color = mainColor;

        if(subSprite != null)
        {
            subSprite.color = subColor;
        }
    }

    public virtual void OnBrickDestroyed() {}
}
